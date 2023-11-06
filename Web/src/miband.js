import { ADVERTISEMENT_SERVICE, CHAR_UUIDS, UUIDS } from "./constants.js";

export class MiBand6 {
  /**
   * @param {String} authKey
   *   Hex representation of the auth key (https://codeberg.org/Freeyourgadget/Gadgetbridge/wiki/Huami-Server-Pairing)
   *   Example: '94359d5b8b092e1286a43cfb62ee7923'
   */
  constructor(authKey) {
    if (!authKey.match(/^[a-zA-Z0-9]{32}$/)) {
      throw new Error(
        "Invalid auth key, must be 32 hex characters such as '94359d5b8b092e1286a43cfb62ee7923'"
      );
    }
    this.authKey = authKey;
    this.services = {};
    this.chars = {};
    this.handle = 0;

    this.reassembleBuffer = new Uint8Array(512);
    this.lastSequenceNumber = 0;
    this.reassembleBuffer_pointer = 0;
    this.reassembleBuffer_expectedBytes = 0;

    this.prv_buf = null;
    this.pub_buf = null;
    this.sec_buf = null;
    this.prv = null;
    this.pub = null;
    this.sec = null;

    //---------
    this.previousSteps = 0;
    this.heartRate = 0;
    this.stepDifference = 0;
    //---------
  }

  getNextHandle() {
    return this.handle++;
  }

  async init() {
    const device = await navigator.bluetooth.requestDevice({
      filters: [
        {
          services: [ADVERTISEMENT_SERVICE],
        },
      ],
      //optionalServices: [UUIDS.miband2, UUIDS.heartrate, UUIDS.miband1],
      // --------------------------------
      optionalServices: [UUIDS.miband2, UUIDS.heartrate, UUIDS.miband1, UUIDS.steps],
      // --------------------------------
    });
    window.dispatchEvent(new CustomEvent("connected"));
    await device.gatt.disconnect();
    const server = await device.gatt.connect();
    console.log("Connected through gatt");

    this.services.miband1 = await server.getPrimaryService(UUIDS.miband1);
    this.services.miband2 = await server.getPrimaryService(UUIDS.miband2);
    this.services.heartrate = await server.getPrimaryService(UUIDS.heartrate);
    // -----------------------------------
    this.services.steps = await server.getPrimaryService(UUIDS.steps);
    // -----------------------------------
    console.log("Services initialized");

    this.chars.auth = await this.services.miband2.getCharacteristic(
      CHAR_UUIDS.auth
    );
    this.chars.chunkedWrite = await this.services.miband1.getCharacteristic(
      CHAR_UUIDS.chunked_transfer_2021_write
    );
    this.chars.chunkedRead = await this.services.miband1.getCharacteristic(
      CHAR_UUIDS.chunked_transfer_2021_read
    );
    this.chars.hrControl = await this.services.heartrate.getCharacteristic(
      CHAR_UUIDS.heartrate_control
    );
    this.chars.hrMeasure = await this.services.heartrate.getCharacteristic(
      CHAR_UUIDS.heartrate_measure
    );
    this.chars.sensor = await this.services.miband1.getCharacteristic(
      CHAR_UUIDS.sensor
    );
    // -------------------------
    this.chars.steps = await this.services.steps.getCharacteristic(
      CHAR_UUIDS.steps
    );
    // -------------------------
    console.log("Characteristics initialized");
    await this.authenticate();
  }

  async authenticate() {
    const CHUNKED2021_ENDPOINT_AUTH = 0x82;

    await this.startNotifications(this.chars.chunkedRead, async (e) => {
      const value = new Uint8Array(e.target.value.buffer);

      if (value.length > 1 && value[0] == 0x03) {
        const sequenceNumber = value[4];
        let headerSize;
        if (
          sequenceNumber == 0 &&
          value[9] == CHUNKED2021_ENDPOINT_AUTH &&
          value[10] == 0x00 &&
          value[11] == 0x10 &&
          value[12] == 0x04 &&
          value[13] == 0x01
        ) {
          console.log("A");
          this.reassembleBuffer_pointer = 0;
          headerSize = 14;
          this.reassembleBuffer_expectedBytes = value[5] - 3;
        } else if (sequenceNumber > 0) {
          if (sequenceNumber != this.lastSequenceNumber + 1) {
            console.log("Unexpected sequence number");
            return;
          }
          headerSize = 5;
        } else if (
          value[9] == CHUNKED2021_ENDPOINT_AUTH &&
          value[10] == 0x00 &&
          value[11] == 0x10 &&
          value[12] == 0x05 &&
          value[13] == 0x01
        ) {
          console.log("Successfully authenticated");
          await this.onAuthenticated();
          return true;
        } else {
          console.log("Unhandled characteristic change");
          return false;
        }

        const bytesToCopy = value.length - headerSize;
        this.reassembleBuffer.set(
          new Uint8Array(value).subarray(headerSize),
          this.reassembleBuffer_pointer
        );

        this.reassembleBuffer_pointer += bytesToCopy;
        this.lastSequenceNumber = sequenceNumber;

        if (this.reassembleBuffer_pointer == this.reassembleBuffer_expectedBytes) {
          const remoteRandom = new Uint8Array(
            this.reassembleBuffer.subarray(0, 16)
          );
          const remotePublicEC = new Uint8Array(
            this.reassembleBuffer.subarray(16, 64)
          );

          const rpub_buf = Module._malloc(ECC_PUB_KEY_SIZE);
          const rpub = Module.HEAPU8.subarray(
            rpub_buf,
            rpub_buf + ECC_PUB_KEY_SIZE
          );
          rpub.set(remotePublicEC);

          const sec_buf = Module._malloc(ECC_PUB_KEY_SIZE);
          const sec = Module.HEAPU8.subarray(
            sec_buf,
            sec_buf + ECC_PUB_KEY_SIZE
          );

          Module._ecdh_shared_secret(this.prv_buf, rpub_buf, sec_buf);

          this.encryptedSequenceNr =
            (sec[0] & 0xff) |
            ((sec[1] & 0xff) << 8) |
            ((sec[2] & 0xff) << 16) |
            ((sec[3] & 0xff) << 24);

          const secretKey = aesjs.utils.hex.toBytes(this.authKey);
          const finalSharedSessionAES = new Uint8Array(16);
          for (let i = 0; i < 16; i++) {
            finalSharedSessionAES[i] = sec[i + 8] ^ secretKey[i];
          }
          this.sharedSessionKey = finalSharedSessionAES;

          const aesCbc1 = new aesjs.ModeOfOperation.cbc(secretKey);
          const out1 = aesCbc1.encrypt(remoteRandom);
          const aesCbc2 = new aesjs.ModeOfOperation.cbc(finalSharedSessionAES);
          const out2 = aesCbc2.encrypt(remoteRandom);

          if (out1.length == 16 && out2.length == 16) {
            const command = new Uint8Array(33);
            command[0] = 0x05;
            command.set(out1, 1);
            command.set(out2, 17);
            console.log("Sending 2nd auth part");
            await this.writeChunkedValue(
              this.chars.chunkedWrite,
              CHUNKED2021_ENDPOINT_AUTH,
              this.getNextHandle(),
              command
            );
          }
        }
        return true;
      }
    });

    const ECC_PUB_KEY_SIZE = 48;
    const ECC_PRV_KEY_SIZE = 24;

    this.pub_buf = Module._malloc(ECC_PUB_KEY_SIZE);
    this.prv_buf = Module._malloc(ECC_PRV_KEY_SIZE);
    this.pub = Module.HEAPU8.subarray(
      this.pub_buf,
      this.pub_buf + ECC_PUB_KEY_SIZE
    );
    this.prv = Module.HEAPU8.subarray(
      this.prv_buf,
      this.prv_buf + ECC_PRV_KEY_SIZE
    );

    crypto.getRandomValues(this.prv);
    Module._ecdh_generate_keys(this.pub_buf, this.prv_buf);

    const auth = this.getInitialAuthCommand(this.pub);
    console.log("Sending first auth");
    await this.writeChunkedValue(
      this.chars.chunkedWrite,
      CHUNKED2021_ENDPOINT_AUTH,
      this.getNextHandle(),
      Uint8Array.from(auth)
    );
  }

  getInitialAuthCommand(publicKey) {
    return [0x04, 0x02, 0x00, 0x02, ...publicKey];
  }

  async writeChunkedValue(char, type, handle, data) {
    let remaining = data.length;
    let count = 0;
    let header_size = 11;
    const mMTU = 23;

    while (remaining > 0) {
      const MAX_CHUNKLENGTH = mMTU - 3 - header_size;
      const copybytes = Math.min(remaining, MAX_CHUNKLENGTH);
      const chunk = new Uint8Array(copybytes + header_size);

      let flags = 0;

      if (count == 0) {
        flags |= 0x01;
        let i = 5;
        chunk[i++] = data.length & 0xff;
        chunk[i++] = (data.length >> 8) & 0xff;
        chunk[i++] = (data.length >> 16) & 0xff;
        chunk[i++] = (data.length >> 24) & 0xff;
        chunk[i++] = type & 0xff;
        chunk[i] = (type >> 8) & 0xff;
      }
      if (remaining <= MAX_CHUNKLENGTH) {
        flags |= 0x06; // last chunk?
      }
      chunk[0] = 0x03;
      chunk[1] = flags;
      chunk[2] = 0;
      chunk[3] = handle;
      chunk[4] = count;

      chunk.set(
        data.slice(
          data.length - remaining,
          data.length - remaining + copybytes
        ),
        header_size
      );

      await char.writeValue(chunk);
      remaining -= copybytes;
      header_size = 5;
      count++;
    }
  }

  async onAuthenticated() {
    console.log("Authentication successful");
    window.dispatchEvent(new CustomEvent("authenticated"));
    //await this.measureHr();
    // ---------------
    this.measureHr();
    this.measureSteps();
    // ---------------
  }

  async measureHr() {
    console.log("Starting heart rate measurement");
    await this.chars.hrControl.writeValue(Uint8Array.from([0x15, 0x02, 0x00]));
    await this.chars.hrControl.writeValue(Uint8Array.from([0x15, 0x01, 0x00]));
    await this.startNotifications(this.chars.hrMeasure, (e) => {
      console.log("Received heart rate value: ", e.target.value);
      const heartRate = e.target.value.getInt16();
      // ---------
      this.heartRate = heartRate;
      // ---------

      window.dispatchEvent(
        new CustomEvent("heartrate", {
          detail: heartRate,
        })
      );
      this.sendPostToFirebase();
    });
    await this.chars.hrControl.writeValue(Uint8Array.from([0x15, 0x01, 0x01]));

    // Start pinging HRM
    this.hrmTimer =
      this.hrmTimer ||
      setInterval(() => {
        console.log("Pinging heart rate monitor");
        this.chars.hrControl.writeValue(Uint8Array.from([0x16]));
      }, 12000);
  }

  // ----------------------
  async measureSteps() {
    console.log("Starting steps measurement");

    await this.startNotifications(this.chars.steps, (e) => {
      console.log("Received steps value: ", e.target.value);
      const data = e.target.value;
      const currentSteps = (data.getInt16(1, true));
      console.log("받아온 걸음 수 : ", currentSteps);

      // 이전 걸음 수와 현재 걸음 수의 차이를 계산합니다.
      const stepsDifference = currentSteps - this.previousSteps;
      this.stepDifference = stepsDifference;
      console.log("걸음 수 차이: ", stepsDifference);

      window.dispatchEvent(
        new CustomEvent("steps", {
            detail: stepsDifference,
        })
      );

      // 현재 걸음 수를 이전 걸음 수로 업데이트합니다.
      this.previousSteps = currentSteps;

      //this.sendPostToFirebase();
    });

    // Start collecting steps data every 10 seconds
    this.stepsTimer =
      this.stepsTimer ||
      setInterval(() => {
        console.log("Collecting steps data");
        // Assuming you need to read the steps data here, update this line accordingly
        this.chars.steps.readValue();
      }, 12000); // Collect steps data every 10 seconds
  }

  async sendPostToFirebase() {
    const url = "https://asia-northeast1-heartrategame.cloudfunctions.net/appleHealth";
    const now = new Date();
    const formattedNow = now.toISOString().slice(0, 19).replace("T", " ");

    const data = {
        data: [
            {
                heartDate: formattedNow,
                heartRate: this.heartRate.toString(),
                steps: this.stepDifference.toString()
            }
        ]
    };

    try {
      const response = await fetch(url, {
          method: "POST",
          mode: 'cors',
          headers: {
              "Content-Type": "application/json"
          },
          body: JSON.stringify(data)
      });

      if (response.ok) {
          const result = await response.text();
          console.log("Result:", result);
      } else {
          console.error("HTTP Error! status: ", response.status);
      }
    } catch (error) {
        console.error("Error:", error);
    }
  }
  // ----------------------

  async startNotifications(char, cb) {
    await char.startNotifications();
    char.addEventListener("characteristicvaluechanged", cb);
  }
}

window.MiBand6 = MiBand6;
