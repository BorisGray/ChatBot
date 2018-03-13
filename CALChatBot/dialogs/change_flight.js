var builder = require('botbuilder');
// const uuid = require('uuid');

const library = new builder.Library('changeFlight');

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

// module.exports = library;
// Export createLibrary() function
module.exports.createLibrary = function () {
    return library.clone();
};