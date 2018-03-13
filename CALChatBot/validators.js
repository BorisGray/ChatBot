var builder = require('botbuilder');

const ReservationNumberRegex = new RegExp(/^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$/);

const library = new builder.Library('validators');

library.dialog('reservationnumber',
    builder.DialogAction.validatedPrompt(builder.PromptType.text, (response) =>
        ReservationNumberRegex.test(response)));

module.exports = library;
module.exports.ReservationNumberRegex = ReservationNumberRegex;