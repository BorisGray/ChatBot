using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
//using Qna_Rich_Cards.AnswerFormats;
using Newtonsoft.Json.Linq;

namespace MioBot.Dialogs
{
    [Serializable]
    public class QnaFAQDialog : QnAMakerDialog
    {
        
        public QnaFAQDialog() : base(new QnAMakerService(new QnAMakerAttribute
            (ConfigurationManager.AppSettings["8c06dd6f8f414bd5ada5cfa19a42b260"],
            ConfigurationManager.AppSettings["d0da51f5-cfb6-4de3-b2ba-020cdbd3e1bb"],
            "Sorry, I couldn't find an answer for that", 0.5)))
        { }

        //add cards to the response
        //    protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        //    {
        //        // answer is a string
        //        var answer = result.Answers.First().Answer;

        //        //    Activity reply = ((Activity)context.Activity).CreateReply();
        //        //    string[] qnaAnswerData = answer.Split(';');
        //        //    string title = qnaAnswerData[0];
        //        //    string description = qnaAnswerData[1];
        //        //    string url = qnaAnswerData[2];
        //        //    string imageURL = qnaAnswerData[3];
        //        //    HeroCard card = new HeroCard
        //        //    {
        //        //        Title = title,
        //        //        Subtitle = description,
        //        //    };
        //        //    card.Buttons = new List<CardAction>
        //        //{
        //        //    new CardAction(ActionTypes.OpenUrl, "Learn More", value: url)
        //        //};
        //        //    card.Images = new List<CardImage>
        //        //{
        //        //    new CardImage( url = imageURL)
        //        //};
        //        //    reply.Attachments.Add(card.ToAttachment());


        //        JsonQnaAnswer qnaAnswer = new JsonQnaAnswer();
        //        Activity reply = ((Activity)context.Activity).CreateReply();
        //        var response = JObject.Parse(answer);
        //        qnaAnswer.title = response.Value<string>("title");
        //        qnaAnswer.desc = response.Value<string>("desc");
        //        qnaAnswer.url = response.Value<string>("url");
        //        ThumbnailCard card = new ThumbnailCard()
        //        {
        //            Title = qnaAnswer.title,
        //            Subtitle = qnaAnswer.desc,
        //            Buttons = new List<CardAction>
        //    {
        //        new CardAction(ActionTypes.OpenUrl, "Click Here", value: qnaAnswer.url)
        //    }
        //        };
        //        reply.Attachments.Add(card.ToAttachment());
        //        await context.PostAsync(reply);
        //    }

    }

}