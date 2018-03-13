// START OF CODE==============================================
try {
    require('dotenv-extended').load({ path: './.env' });
} catch (error) {
    console.log('.env file could not found!');
}

const moment = require('moment')
const restify = require('restify'); //this is loading the library installed earlier
const builder = require('botbuilder'); //this is loading the library installed earlier
const botbuilder_azure = require("botbuilder-azure");
// const luisModelUrl = process.env.LUIS_MODEL_URL;
const luisModelUrl = 'https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/0e61f9bb-7e6c-4a4f-9c96-9deeb1003f65?subscription-key=72a2350f67b944dfbc5f9b342fa846ac&verbose=true&timezoneOffset=0&q=';


global.luisModelUrl = luisModelUrl;
console.log('luisModelUrl = %s', luisModelUrl);

var inMemoryStorage = new builder.MemoryBotStorage();
//============================================================
// Setting up server, connector, and bot
//============================================================

// Setup Restify Server
const server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
    //When testing on a local machine, 3978 indicates the port to test on
    console.log('%s listening to %s', server.name, server.url);
});

// Create chat bot
// MICROSOFT_APP_ID and MICROSOFT_APP_PASSWORD are created when you register a bot at https://dev.botframework.com/bots 
// Details in the tutorial link in README
const connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});

// Create chat bot
// const connector = new botbuilder_azure.BotServiceConnector({
//     appId: process.env['MicrosoftAppId'],
//     appPassword: process.env['MicrosoftAppPassword'],
//     stateEndpoint: process.env['BotStateEndpoint'],
//     openIdMetadata: process.env['BotOpenIdMetadata']
// });


// const BookFlightOption = '預訂航班';
// const SearchFlightOption = '查詢航班';
// const ChangeFlightOption = '變更航班';
// const InquiryFlightOption = '詢問問題';

// const bot = new builder.UniversalBot(connector, [
//     (session) => {
//         builder.Prompts.choice(session,
//             '請按照您所需的服務點選下面的選項:  ',
//             [BookFlightOption, SearchFlightOption, ChangeFlightOption, InquiryFlightOption],
//             { listStyle: builder.ListStyle.button });
//     },
//     (session, result) => {
//         if (result.response) {
//             switch (result.response.entity) {
//                 case BookFlightOption:
//                     // session.send('This functionality is not yet implemented! Try resetting your password.');
//                     // session.reset();
//                     session.beginDialog('bookFlight:/');
//                     break;
//                 case SearchFlightOption:
//                     session.beginDialog('searchFlight:/');
//                     break;
//                 case ChangeFlightOption:
//                     session.beginDialog('changeFlight:/');
//                     break;
//                 case InquiryFlightOption:
//                     session.beginDialog('inquiryFlight:/');
//                     break;
//             }
//         } else {
//             session.send(`I am sorry but I didn't understand that. I need you to select one of the options below`);
//         }
//     },
//     (session, result) => {
//         if (result.resume) {
//             session.send('You identity was not verified and your password cannot be reset');
//             session.reset();
//         }
//     }
// ]);

// Welcome Dialog
var MainOptions = {
    BookFlightOption: '預訂航班',
    SearchFlightOption: '查詢航班',
    ChangeFlightOption: '變更航班',
    InquiryFlightOption: '詢問問題'
};

// Cache of localized regex to match selection from main options
var LocalizedRegexCache = {};
function localizedRegex(session, localeKeys) {
    var locale = session.preferredLocale();
    var cacheKey = locale + ":" + localeKeys.join('|');
    if (LocalizedRegexCache.hasOwnProperty(cacheKey)) {
        return LocalizedRegexCache[cacheKey];
    }

    var localizedStrings = localeKeys.map(function (key) { return session.localizer.gettext(locale, key); });
    var regex = new RegExp('^(' + localizedStrings.join('|') + ')', 'i');
    LocalizedRegexCache[cacheKey] = regex;
    return regex;
}

const bot = new builder.UniversalBot(connector, function (session) {

    if (localizedRegex(session, [MainOptions.BookFlightOption]).test(session.message.text)) { 
        return session.beginDialog('bookFlight:/');
    }

    var welcomeCard = new builder.HeroCard(session)
        .title('欢迎进入    中华航空自动客服系统')
        .subtitle('***V1.0***')
        .images([
            new builder.CardImage(session)
                .url('file:///root/workspace/dp-ai-chatbot/CALChatBot/assetes/CALLogo.jpg')
                .alt('中华航空自动客服系统')
        ])
        .buttons([
            builder.CardAction.imBack(session, session.gettext(MainOptions.BookFlightOption), MainOptions.BookFlightOption),
            builder.CardAction.imBack(session, session.gettext(MainOptions.SearchFlightOption), MainOptions.SearchFlightOption),
            builder.CardAction.imBack(session, session.gettext(MainOptions.ChangeFlightOption), MainOptions.ChangeFlightOption),
            builder.CardAction.imBack(session, session.gettext(MainOptions.InquiryFlightOption), MainOptions.InquiryFlightOption)
        ]);

    session.send(new builder.Message(session)
        .addAttachment(welcomeCard));
});

// Send welcome when conversation with bot is started, by initiating the root dialog
bot.on('conversationUpdate', function (message) {
    if (message.membersAdded) {
        message.membersAdded.forEach(function (identity) {
            if (identity.id === message.address.bot.id) {
                bot.beginDialog(message.address, '/');
            }
        });
    }
});

// const bot = new builder.UniversalBot(connector)
bot.set('storage', inMemoryStorage)
// Set default locale
bot.set('localizerSettings', {
    botLocalePath: './customLocale',
    defaultLocale: 'cht'
});

global.bot = bot;

//Sub-Dialogs
// bot.library(require('./dialogs/book_flight'));
// bot.library(require('./dialogs/search_flight'));
// bot.library(require('./dialogs/change_flight'));
// bot.library(require('./dialogs/inquiry_flight'));

bot.library(require('./dialogs/book_flight').createLibrary());
bot.library(require('./dialogs/change_flight').createLibrary());
bot.library(require('./dialogs/inquiry_flight').createLibrary());
bot.library(require('./dialogs/search_flight').createLibrary());

//Validators
bot.library(require('./validators'));
// Trigger secondary dialogs when 'settings' or 'support' is called
bot.use({
    botbuilder: function (session, next) {
        var text = session.message.text;

        // var settingsRegex = localizedRegex(session, ['main_options_settings']);
        // var supportRegex = localizedRegex(session, ['main_options_talk_to_support', 'help']);

        // if (settingsRegex.test(text)) {
        //     // interrupt and trigger 'settings' dialog 
        //     return session.beginDialog('settings:/');
        // } else if (supportRegex.test(text)) {
        //     // interrupt and trigger 'help' dialog
        //     return session.beginDialog('help:/');
        // }

        // continue normal flow
        next();
    }
});

server.post('/api/messages', connector.listen());

function Salute(){
    const split_afternoon = 12
    const split_evening = 17
    const currentHour = parseFloat(moment().utc().format('HH'))
    if(currentHour >= split_afternoon && currentHour <= split_evening){
        return 'Good night'
    }
    else if (currentHour >= split_evening) {
        return 'Good afternoon'
    }
    return 'Good morning'
}

const  recognizer = new builder.LuisRecognizer(luisModelUrl);
const  intents = new builder.IntentDialog({ recognizers: [recognizer] });

intents.matches('None', (session, args, next) => {
    session.send('**( ͡° ͜ʖ ͡°)** - Desculpe, mas não entendi o que você quis dizer.\n\nLembre-se que sou um bot e meu conhecimento é limitado.')
})

intents.matches('help', (session, args, next) => {
    const message = 'Do not forget that I am an * *Bot * * and my chat BOT is limited. Look, oh, what I can do: \ n '+
                    '* **to talk like people**\n' +
                    '* **describe images**\n' +
                    '* **recognize emotions**\n' +
                    '* **to classify objects**\n' +
                    '* ** translate texts**\n' +
                    '* **recommend products by item**\n' +
                    '* **recommend products for a particular profile**'
                    session.send(message)
})

// intents.matches('book.start_booking', 
//     // session.send('**(¬_¬)** - 請輸入出發地:')
//     [
//     function (session) {
//         session.send('歡迎預定中華航空航班!');
//         builder.Prompts.text(session, '請輸入出發地:');
//     },
//     function (session, results, next) {
//         session.dialogData.departure = results.response;
//         session.send('Looking for city: %s', results.response);
//         next();
// },
// ]
// )




// bot.dialog('/', intents)

/*
var bot = new builder.UniversalBot(connector
    ,  [
    function (session) {
        // console.log('++++++++++++++++++++++++++var bot = new builder.UniversalBot(connector....  function (session)++++++++++++++++++++++');
        session.send("greeting");
        session.send("instructions");
        session.beginDialog('localePickerDialog');
    },
    function (session) {
        builder.Prompts.text(session, "text_prompt");
    },
    function (session, results) {
        session.send("input_response", results.response);
        builder.Prompts.number(session, "number_prompt");
    },
    function (session, results) {
        session.send("input_response", results.response);
        builder.Prompts.choice(session, "listStyle_prompt", "auto|inline|list|button|none");
    },
    function (session, results) {
        // You can use the localizer manually to load a localized list of options.
        var style = builder.ListStyle[results.response.entity];
        var options = session.localizer.gettext(session.preferredLocale(), "choice_options");
        builder.Prompts.choice(session, "choice_prompt", options, { listStyle: style });
    },
    function (session, results) {
        session.send("choice_response", results.response.entity);
        builder.Prompts.confirm(session, "confirm_prompt");
    },
    function (session, results) {
        // You can use the localizer manually to load prompts from another namespace.
        var choice = results.response ? 'confirm_yes' : 'confirm_no';
        session.send("choice_response", session.localizer.gettext(session.preferredLocale(), choice, 'BotBuilder'));
        builder.Prompts.time(session, "time_prompt");
    },
    function (session, results) {
        session.send("time_response", JSON.stringify(results.response));
        session.endDialog("demo_finished");
    }]
).set('storage', inMemoryStorage); // Register in memory storage;
var recognizer = new builder.LuisRecognizer(luisModelUrl);
// var intentDialog = bot.dialog('/', new builder.IntentDialog({ recognizers: [recognizer] })
//     .onDefault(DefaultReplyHandler));


// Configure bots default locale and locale folder path.
bot.set('localizerSettings', {
    botLocalePath: "./customLocale", 
    defaultLocale: "en" 
});

function DefaultReplyHandler(session) {
    session.endDialog(
        'Sorry, I did not understand "%s".',
        session.message.text);
}

// Add locale picker dialog 
bot.dialog('localePickerDialog', [
    function (session) {
     
        // console.log('---------------bot.dialog(\'localePickerDialog\'  function (session)-----------------');
        // Prompt the user to select their preferred locale
        builder.Prompts.choice(session, "locale_prompt", 'English|Simplified Chinese|Traditional Chinese');
    },
    function (session, results) {
        // console.log('bot.dialog(\'----------------localePickerDialog\'  function (session, results)----------------');
        // Update preferred locale
        var locale;
        switch (results.response.entity) {
            case 'English':
                locale = 'en';
                break;
            case 'Simplified Chinese':
                locale = 'ch';
                break;
            case 'Traditional Chinese':
                locale = 'cht';
                break;
        }
        session.preferredLocale(locale, function (err) {
            if (!err) {
                // Locale files loaded
                session.endDialog('locale_updated');
            } else {
                // Problem loading the selected locale
                session.error(err);
            }
        });
    }
]);

//============================================================
// Defining how bot carries on the conversation with the user
//============================================================

// bot.dialog('/', function (session) {
//     session.send("Hello World");
// });

*/

// END OF CODE================================================
