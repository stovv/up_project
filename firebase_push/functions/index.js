const functions = require('firebase-functions');

const admin = require('firebase-admin');
admin.initializeApp(functions.config().firebase);

exports.sendPowerNotification = functions.database.ref("/Группы/").onUpdate((event) => {
  console.log('Power event triggered');
    const payload = {
      notification: {
          title: 'UP-notify',
          body: `Study time is changes`,
          sound: "default"
      }
    };

    const options = {
      priority: "high",
      timeToLive: 60 * 60 * 24 //24 hours
    };

  return admin.messaging().sendToTopic("FireNotify", payload, options);
});

  
