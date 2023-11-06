import * as functions from 'firebase-functions';
import * as admin from 'firebase-admin';
import * as cors from 'cors';

interface Sample {
  heartDate: string;
  heartRate: number;
  stepDate: string;
  steps: number;
}

interface Body {
  data: Sample[];
}

admin.initializeApp();

const db = admin.firestore();
const corsHandler = cors({origin: true});

/**
 * @function
 * @param {functions.https.Request} request
 * @param {functions.Response<any>} response
 * @returns {void}
 */
export const appleHealth = functions
  .region('asia-northeast1')
  .https.onRequest(async (request, response) => {
    corsHandler(request, response, async () => {
      const {data}: Body = request.body;
      console.log(JSON.stringify(data));
      const batch = db.batch();
      const collection = db.collection('apple-health');

      data.forEach((sample) => {
        batch.set(collection.doc(), sample, {});
        console.log(JSON.stringify(sample));
      });

      await batch.commit();
      response.send('ok');
    });
  });

// // Start writing functions
// // https://firebase.google.com/docs/functions/typescript
//
// export const helloWorld = functions.https.onRequest((request, response) => {
//   functions.logger.info("Hello logs!", {structuredData: true});
//   response.send("Hello from Firebase!");
// });

