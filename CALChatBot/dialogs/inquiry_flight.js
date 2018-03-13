const builder = require('botbuilder');
const cognitiveservices = require('botbuilder-cognitiveservices')
// const uuid = require('uuid');

const library = new builder.Library('inquiryFlight');

library.dialog('/', [
    (session) => {
        session.beginDialog('validators:reservationnumber', {
            prompt: '請輸入您的訂位代號:',
            retryPrompt: '您輸入的不是有效的訂位代號.請按照以下格式重新輸入(xyz) xyz-wxyz:',
            maxRetries: 2
        });
    },
    (session, args) => {
        if (args.resumed) {
            session.send('您嘗試輸入訂位代號的次數已達上限,請稍後再次嘗試!');
            session.endDialogWithResult({ resumed: builder.ResumeReason.notCompleted });
            return;
        }

        session.dialogData.ReservationNumber = args.response;
        session.send('您輸入的訂位代號是: ' + args.response);

        builder.Prompts.time(session, '需要和您確認一下您的身份訊息, 請問您機票上的旅客姓名是?', {
            retryPrompt: '您輸入的姓名無效, 請重新輸入:',
            maxRetries: 2
        });
    },
    (session, args) => {
        if (args.resumed) {
            session.send('您輸入的姓名無效. 請稍後再次嘗試!');
            session.endDialogWithResult({ resumed: builder.ResumeReason.notCompleted });
            return;
        }

        session.send('您輸入的姓名是: ' + args.response.entity);

        // var newPassword = uuid.v1();
        // session.send('Thanks! Your new password is _' + newPassword + '_');

        session.endDialogWithResult({ resumed: builder.ResumeReason.completed });
    }
]).cancelAction('cancel', null, { matches: /^cancel/i });

//=========================================================
// Bots Dialogs
//=========================================================

var recognizer = new cognitiveservices.QnAMakerRecognizer({
    knowledgeBaseId: 'set your kbid here',
    subscriptionKey: 'set your subscription key here'});

var basicQnAMakerDialog = new cognitiveservices.QnAMakerDialog({
    recognizers: [recognizer],
    defaultMessage: 'No match! Try changing the query terms!',
    qnaThreshold: 0.3
});

// bot.dialog('/', basicQnAMakerDialog);

// //=========================================================
// // Bots Dialogs
// //=========================================================

// const recognizer = new cognitiveServices.QnAMakerRecognizer({
//     knowledgeBaseId: process.env.QNA_KNOWLEDGE_BASE_ID,
//     subscriptionKey: process.env.QNA_SUBSCRIPTION_KEY,
//     top: 3
//   })
  
//   const brazilianQnaMakerTools = new brazilianTools.BrazilianQnaMakerTools()
//   bot.library(brazilianQnaMakerTools.createLibrary())
  
//   const basicQnaMakerDialog = new cognitiveServices.QnAMakerDialog({
//     recognizers: [recognizer],
//     defaultMessage: 'Not found! Try to change the terms of the question.!',
//     qnaThreshold: 0.5,
//     feedbackLib: brazilianQnaMakerTools
//   })
  
//   basicQnaMakerDialog.respondFromQnAMakerResult = (session, qnaMakerResult) => {
//       const firstAnswer = qnaMakerResult.answers[0].answer
//       const composedAnswer = firstAnswer.split(';')
//       if (composedAnswer.length === 1) {
//       return session.send(firstAnswer)
//       }
//       const [title, description, url, image] = composedAnswer
//       const card = new builder.HeroCard(session)
//           .title(title)
//           .text(description)
//           .images([builder.CardImage.create(session, image.trim())])
//           .buttons([builder.CardAction.openUrl(session, url.trim(), 'Buy now')])
//       const reply = new builder.Message(session).addAttachment(card)
//       session.send(reply)
//   }
  
//   bot.dialog('/', basicQnaMakerDialog)

// module.exports = library;

// Export createLibrary() function
module.exports.createLibrary = function () {
    return library.clone();
};