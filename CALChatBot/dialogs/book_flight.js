var builder = require('botbuilder');
// const uuid = require('uuid');

const library = new builder.Library('bookFlight');
// Create LUIS recognizer that points at our model 
console.log('--------------------------luisModelUrl = %s-------------------', luisModelUrl);
const recognizer = new builder.LuisRecognizer(luisModelUrl);
const intents = new builder.IntentDialog({ recognizers: [recognizer] });
//bot.dialog('/', dialog);

function DefaultReplyHandler(session) {
    session.endDialog(
        'Sorry, I did not understand "%s".',
        session.message.text);
}

intents.matches('None', (session, args, next) => {
    session.send('**( ͡° ͜ʖ ͡°)** - Sorry, but I do not understand what you mean.\n\nRemember that I am a BOT and my knowledge is limited.')
})

intents.matches('help', (session, args, next) => {
    const message = 'Do not forget that I am an * *Bot * * and my chat BOT is limited. Look, oh, what I can do: \ n ' +
        '* **to talk like people**\n' +
        '* **describe images**\n' +
        '* **recognize emotions**\n' +
        '* **to classify objects**\n' +
        '* ** translate texts**\n' +
        '* **recommend products by item**\n' +
        '* **recommend products for a particular profile**'
    session.send(message)
})
intents.matches('book.start_booking',
    // session.send('**(¬_¬)** - 請輸入出發地:')
    [
        function (session) {
            session.send('歡迎預定中華航空航班!');
            builder.Prompts.text(session, '請輸入出發地:');
        },
        function (session, results, next) {
            session.dialogData.departure = results.response;
            session.send('Looking for city: %s', results.response);
            next();
        },
    ]
)

library.dialog('/', [
    function (session) {
        session.send("**欢迎进入预订航班模块**.");
        session.beginDialog('askDepartureAndDestination');
    },

    function (session, results) {
        // Store the destination name on the userData session attribute
        // session.userData.destination = results.response;
        session.beginDialog('askOneWayOrRoundTrip');
    },

    function (session, results) {
        // if (results.response.entity === OnewayOrRoundtripLabels.RoundTrip) {
        //     session.userData.IsRoundtrip = true;
        // }
        // else {
        //     session.userData.IsRoundtrip = false;
        // };
        session.beginDialog('askDepartureDate_v2');
    },

    function (session, results) {
        session.beginDialog('askPassengersNumber');
    }

    ///////////////////////-----------v1--------------///////////////////////////////////////////////////////////
    // function (session) {
    //     // session.send("Welcome to the CALBot book flight module.");
    //     session.beginDialog('askDeparture');
    // },
    // function (session, results) {
    //     // Store the departure name on the userData session attribute
    //     session.userData.departure = results.response;
    //     session.beginDialog('askDestination');
    // },
    // // function (session, args) {
    // //     session.beginDialog('/askOneWayOrRoundTrip');
    // // },

    // function (session, results) {
    //     // Store the destination name on the userData session attribute
    //     // session.userData.destination = results.response;
    //     session.beginDialog('askOneWayOrRoundTrip');
    // },

    // function (session, results) {
    //     if (results.response.entity === OnewayOrRoundtripLabels.RoundTrip) {
    //         session.userData.IsRoundtrip = true;
    //     }
    //     else {
    //         session.userData.IsRoundtrip = false;
    //     };
    //     session.beginDialog('askDepartureDate');    
    // },

    // // function (session, args) {
    // //     // prompt for search option
    // //     builder.Prompts.choice(
    // //         session,
    // //         '請問您是選擇單程還是雙程?',
    // //         [OnewayOrRoundtripLabels.OneWay, OnewayOrRoundtripLabels.RoundTrip],
    // //         { listStyle: builder.ListStyle.button });
    // // },

    // // function (session, result) {
    // //     if (result.response) {
    // //         // continue on proper dialog
    // //         session.userData.OnewayOrRoundtrip = result.response.entity;
    // //         // session.endDialog();
    // //         // session.beginDialog('/askDepartureDate');    
    // //         switch (result.response.entity) {
    // //             case OnewayOrRoundtripLabels.OneWay:
    // //                 // [function (session) {
    // //                 //     builder.Prompts.text(session, '請問您哪一天出發(MM/dd/yyyy)?')
    // //                 // },
    // //                 //     function (session, results) {
    // //                 //         //     // Store the destination name on the userData session attribute
    // //                 //         session.userData.departureDate = results.response;
    // //                 //         //     // session.endDialog();
    // //                 //     }]

    // //                 session.beginDialog('/askDepartureDate');

    // //                 break;
    // //             case OnewayOrRoundtripLabels.RoundTrip:
    // //                 // session.endDialog();
    // //                 // session.beginDialog('/askReturnDate');
    // //                 [session =>
    // //                     builder.Prompts.time(session, '請問您哪一天出發(MM/dd/yyyy)?'),
    // //                 (session, results) => {
    // //                     // Store the destination name on the userData session attribute
    // //                     session.userData.departureDate = builder.EntityRecognizer.resolveTime([results.response]);
    // //                     // session.endDialog();
    // //                 },
    // //                 session =>
    // //                     builder.Prompets.time(session, '請問您哪一天返程(MM/dd/yyyy)?'),
    // //                 (session, results) => {
    // //                     // Store the arrival date on the userData session attribute
    // //                     session.userData.returnDate = session.beginDialog('hotels-search', value)builder.EntityRecognizer.resolveTime([results.response]);
    // //                     session.endDialog();
    // //                 }]
    // //                 break;
    // //         }
    // //     } else {
    // //         session.send(`I am sorry but I didn't understand that. I need you to select one of the options below`);
    // //     }
    // // },

    // function (session, results) {
    //     session.beginDialog('askPassengersNumber');
    // },

    // function (session, args) {
    //     if (!session.userData.greeting) {
    //         session.send(session.gettext('provide_departure'));
    //         session.userData.greeting = true;
    //     } else if (!session.userData.name) {
    //         getName(session);
    //     } else if (!session.userData.email) {
    //         getEmail(session);
    //     } else if (!session.userData.password) {
    //         getPassword(session);
    //     } else {
    //         session.userData = null;
    //     }
    //     session.endDialog();
    // }
]
    // [
    // function (session) {
    //     // Ask for delivery address using 'address' library
    //     session.beginDialog('address:/',
    //         {
    //             promptMessage: session.gettext('provide_departure', session.message.user.name || session.gettext('default_user_name'))
    //         });
    // },
    // function (session, args) {
    //     // Retrieve address, continue to shop
    //     session.dialogData.recipientAddress = args.address;
    //     session.beginDialog('product-selection:/');
    // },
    // function (session, args) {
    //     // Retrieve selection, continue to delivery date
    //     session.dialogData.selection = args.selection;
    //     session.beginDialog('delivery:date');
    // },
    // function (session, args) {
    //     // Retrieve deliveryDate, continue to details
    //     session.dialogData.deliveryDate = args.deliveryDate;
    //     session.send('confirm_choice', session.dialogData.selection.name, session.dialogData.deliveryDate.toLocaleDateString());
    //     session.beginDialog('details:/');
    // },
    // function (session, args) {
    //     // Retrieve details, continue to billing address
    //     session.dialogData.details = args.details;
    //     session.beginDialog('address:billing');
    // },
    // function (session, args, next) {
    //     // Retrieve billing address
    //     session.dialogData.billingAddress = args.billingAddress;
    //     next();
    // },
    // function (session, args) {
    //     // Continue to checkout
    //     var order = {
    //         selection: session.dialogData.selection,
    //         delivery: {
    //             date: session.dialogData.deliveryDate,
    //             address: session.dialogData.recipientAddress
    //         },
    //         details: session.dialogData.details,
    //         billingAddress: session.dialogData.billingAddress
    //     };

    //     console.log('order', order);
    //     session.beginDialog('checkout:/', { order: order });
    // }
    // ]
)

library.dialog('askDepartureAndDestination', [
    function (session) {

        if (session.message && session.message.value) {
            // A Card's Submit Action obj was received
            // processSubmitAction(session, session.message.value);
            session.userData.departure = session.message.value.departure;
            session.userData.destination = session.message.value.destination;
            // session.replaceDialog('askOneWayOrRoundTrip');
            session.endDialog();
            return;
        }

        var card = {
            'contentType': 'application/vnd.microsoft.card.adaptive',
            'content':
                {
                    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                    "type": "AdaptiveCard",
                    "version": "1.0",
                    "body": [
                        {
                            "type": "TextBlock",
                            "text": "请输入**出发地**和**目的地**",
                            "size": "large",
                            "weight": "bolder"
                        },
                        {
                            'type': 'TextBlock',
                            'text': '出发地:'
                        },
                        {
                            'type': 'Input.Text',
                            'id': 'departure',
                            'speak': '<s>请输入出发地</s>',
                            'placeholder': '武汉, 中华人民共和国',
                            'style': 'text'
                        },

                        {
                            'type': 'TextBlock',
                            'text': '目的地:'
                        },
                        {
                            'type': 'Input.Text',
                            'id': 'destination',
                            'speak': '<s>请输入目的地</s>',
                            'placeholder': '台北, 中华民国台湾省',
                            'style': 'text'
                        },
                    ],

                    "actions": [
                        {
                            "type": "Action.Submit",
                            "title": "确认"
                        }
                    ]
                }
        };

        var msg = new builder.Message(session)
            .addAttachment(card);
        session.send(msg);
    }
]
)

function processSubmitAction(session, value) {

    // session.userData.departure = results.response;
    // session.userData.destination = results.response;
}


// askDeparture dialog
library.dialog('askDeparture', [
    session =>
        builder.Prompts.text(session, '好的, 接下來我將協助您預定所需要的航班, 請問您打算從哪裏出發呢?'),
    (session, results) => {
        session.endDialogWithResult(results);
    }
]);

// askDestination dialog
library.dialog('askDestination', [
    session =>
        builder.Prompts.text(session, '請問您要去哪個城市?'),
    (session, results) => {
        session.endDialogWithResult(results);
    }
]);


var OnewayOrRoundtripLabels = {
    OneWay: '單程',
    RoundTrip: '雙程'
};

library.dialog('askOneWayOrRoundTrip', [
    function (session) {
        builder.Prompts.choice(
            session,
            '**請問您是選擇單程還是雙程?**',
            [OnewayOrRoundtripLabels.OneWay, OnewayOrRoundtripLabels.RoundTrip],
            { listStyle: builder.ListStyle.button });
    },
    function (session, results) {
        if (results.response) {
            if (results.response.entity === OnewayOrRoundtripLabels.RoundTrip)
                session.userData.IsRoundtrip = true;
            else
                session.userData.IsRoundtrip = false;

            // session =>
            //     builder.Prompts.time(session, '請問您哪一天出發(MM/dd/yyyy)?'),
            // (session, results) => {
            //     session.userData.departureDate = builder.EntityRecognizer.resolveTime([results.response]);
            // };
            // session.beginDialog('/askDepartureDate');
            // session.endDialogWithResult(results);
            session.endDialog();
        }
    }
]);


library.dialog('askDepartureDate_v2', [
    function (session) {

        if (session.message && session.message.value) {

            // if (session.message.value.type == 'departure_date') {
            session.userData.departureDate = session.message.value.departure_date;

            if (session.userData.IsRoundtrip === true)
                session.replaceDialog('askReturnDate_v2');
            else
                session.endDialog();
            return;
            // }

        }

        var card = {
            'contentType': 'application/vnd.microsoft.card.adaptive',
            'content':
                {
                    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                    "type": "AdaptiveCard",
                    "version": "1.0",
                    "body": [
                        {
                            "type": "TextBlock",
                            "text": "请输入**出发日期**",
                            "size": "large",
                            "weight": "bolder"
                        },
                        {
                            'type': 'Input.Date',
                            'id': 'departure_date',
                            "title": "出发日期",
                        }
                    ],

                    "actions": [
                        {
                            "type": "Action.Submit",
                            "title": "确认"
                        }
                    ]
                }
        };

        var msg = new builder.Message(session)
            .addAttachment(card);
        session.send(msg);
    }
]
)


library.dialog('askReturnDate_v2', [
    function (session) {

        if (session.message && session.message.value.return_date) {
            session.userData.returnDate = session.message.value.return_date;
            session.endDialog();
            return;
        }

        var card = {
            'contentType': 'application/vnd.microsoft.card.adaptive',
            'content':
                {
                    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                    "type": "AdaptiveCard",
                    "version": "1.0",
                    "body": [
                        {
                            "type": "TextBlock",
                            "text": "请输入**返程日期**",
                            "size": "large",
                            "weight": "bolder"
                        },
                        {
                            'type': 'Input.Date',
                            'id': 'return_date',
                            "title": "返程日期",
                        }
                    ],

                    "actions": [
                        {
                            "type": "Action.Submit",
                            "title": "确认"
                        }
                    ]
                }
        };

        var msg = new builder.Message(session)
            .addAttachment(card);
        session.send(msg);
    }
]
)
// askDepartureDate dialog
library.dialog('askDepartureDate', [
    session =>
        builder.Prompts.time(session, '請問您哪一天出發(MM/dd/yyyy)?'),
    (session, results) => {
        session.userData.departureDate = builder.EntityRecognizer.resolveTime([results.response]);
        // session.userData.departureDate = results.response;
        // session.endDialogWithResult(results);

        if (session.userData.IsRoundtrip === true)
            // Dialog stack: askReturnDate dialog will replace askDepartureDate
            session.replaceDialog('askReturnDate');
        else
            session.endDialogWithResult(results);
    },
]);

// askReturnDate dialog
library.dialog('askReturnDate', [
    session =>
        builder.Prompts.time(session, '請問您哪一天返程(MM/dd/yyyy)?'),
    (session, results) => {
        // Store the arrival date on the userData session attribute
        session.userData.returnDate = builder.EntityRecognizer.resolveTime([results.response]);
        session.endDialogWithResult(results);
    },
]);

// library.dialog('askPassengersNumber', [
//     session =>
//         builder.Prompts.number(session, '請問一共有多少名乘客?'),
//     (session, results) => {
//         // Store the arrival date on the userData session attribute
//         session.userData.passengersNumber = results.response;
//         session.endDialogWithResult(results);
//     }
// ]);

library.dialog('askPassengersNumber', [
    function (session) {

        if (session.message && session.message.value) {

            // if (session.message.value.type == 'departure_date') {
            // session.userData.departureDate = session.message.value.departure_date;

            // if (session.userData.IsRoundtrip === true)
            //     session.replaceDialog('askReturnDate_v2');
            // else
            //     session.endDialog();
            // return;
            // // }

        }

        var card = {
            'contentType': 'application/vnd.microsoft.card.adaptive',
            'content':
                {
                    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
                    "type": "AdaptiveCard",
                    "version": "1.0",
                    "body": [
                        {
                            "type": "TextBlock",
                            "text": "请输入**乘客数**",
                            "size": "large",
                            "weight": "bolder"
                        },
                        {
                            'type': 'Input.Number',
                            'id': 'adults_number',
                            'min': 1,
                            'max': 9,
                            'speak': '<s>成人数</s>'
                        },
                        {
                            'type': 'Input.Number',
                            'id': 'children_number',
                            'min': 1,
                            'max': 9,
                            'speak': '<s>儿童数</s>'
                        },
                        {
                            'type': 'Input.Number',
                            'id': 'infants_number',
                            'min': 1,
                            'max': 9,
                            'speak': '<s>婴儿数</s>'
                        }
                    ],

                    "actions": [
                        {
                            "type": "Action.Submit",
                            "title": "确认"
                        }
                    ]
                }
        };

        var msg = new builder.Message(session)
            .addAttachment(card);
        session.send(msg);
    }
]
)
// library.dialog('/', function (session, args) {
//     if (!session.userData.greeting) {
//         session.send("Hello. What is your name?");
//         session.userData.greeting = true;
//     } else if (!session.userData.name) {
//         getName(session);
//     } else if (!session.userData.email) {
//         getEmail(session);
//     } else if (!session.userData.password) {
//         getPassword(session);
//     } else {
//         session.userData = null;
//     }
//     session.endDialog();
// });


// library.dialog('/', [
//     (session) => {
//         session.send('歡迎預定中華航空航班!');
//         builder.Prompts.text(session, '請輸入出發地:');
//     },

//     (session) => {
//         session.beginDialog('validators:reservationnumbOnewayOrRoundtriper', {
//             prompt: '請輸入您的訂位代號:',
//             retryPrompt: '您輸入的不是有效的訂位代號.請按照以下格式重新輸入(xyz) xyz-wxyz:',
//             maxRetries: 2
//         });
//     },
//     (session, args) => {
//         if (args.resumed) {
//             session.send('您嘗試輸入訂位代號的次數已達上限,請稍後再次嘗試!');
//             session.endDialogWithResult({ resumed: builder.ResumeReason.notCompleted });
//             return;
//         }

//         session.dialogData.ReservationNumber = args.response;
//         session.send('您輸入的訂位代號是: ' + args.response);

//         builder.Prompts.time(session, '需要和您確認一下您的身份訊息, 請問您機票上的旅客姓名是?', {
//             retryPrompt: '您輸入的姓名無效, 請重新輸入:',
//             maxRetries: 2
//         });
//     },
//     (session, args) => {
//         if (args.resumed) {
//             session.send('您輸入的姓名無效. 請稍後再次嘗試!');
//             session.endDialogWithResult({ resumed: builder.ResumeReason.notCompleted });
//             return;
//         }

//         session.send('您輸入的姓名是: ' + args.response.entity);

//         // var newPassword = uuid.v1();
//         // session.send('Thanks! Your new password is _' + newPassword + '_');

//         session.endDialogWithResult({ resumed: builder.ResumeReason.completed });
//     }
// ]).cancelAction('cancel', null, { matches: /^cancel/i });

// module.exports = library;


// library.dialog('/', [
//     function (session) {
//         // Ask for delivery address using 'address' library
//         session.beginDialog('address:/',
//             {
//                 promptMessage: session.gettext('provide_delivery_address', session.message.user.name || session.gettext('default_user_name'))
//             });
//     },
//     function (session, args) {
//         // Retrieve address, continue to shop
//         session.dialogData.recipientAddress = args.address;
//         session.beginDialog('product-selection:/');
//     },
//     function (session, args) {
//         // Retrieve selection, continue to delivery date
//         session.dialogData.selection = args.selection;
//         session.beginDialog('delivery:date');
//     },
//     function (session, args) {
//         // Retrieve deliveryDate, continue to details
//         session.dialogData.deliveryDate = args.deliveryDate;
//         session.send('confirm_choice', session.dialogData.selection.name, session.dialogData.deliveryDate.toLocaleDateString());
//         session.beginDialog('details:/');
//     },
//     function (session, args) {
//         // Retrieve details, continue to billing address
//         session.dialogData.details = args.details;
//         session.beginDialog('address:billing');
//     },
//     function (session, args, next) {
//         // Retrieve billing address
//         session.dialogData.billingAddress = args.billingAddress;
//         next();
//     },
//     function (session, args) {
//         // Continue to checkout
//         var order = {
//             selection: session.dialogData.selection,
//             delivery: {
//                 date: session.dialogData.deliveryDate,
//                 address: session.dialogData.recipientAddress
//             },
//             details: session.dialogData.details,
//             billingAddress: session.dialogData.billingAddress
//         };

//         console.log('order', order);
//         session.beginDialog('checkout:/', { order: order });
//     }
// ]);



// Export createLibrary() function
module.exports.createLibrary = function () {
    return library.clone();
};