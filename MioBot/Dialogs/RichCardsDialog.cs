namespace MioBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class RichCardsDialog : IDialog<object>
    {
        private const string HeroCard = "Hero card";
        private const string SigninCard = "登录";
        private IEnumerable<string> options = new List<string> { HeroCard, SigninCard };

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            PromptDialog.Choice<string>(
                context,
                this.DisplaySelectedCard,
                this.options,
                "请输入对应的编号",
                "Ooops, what you wrote is not a valid option, please try again",
                3,
                PromptStyle.PerLine);
        }

        public async Task DisplaySelectedCard(IDialogContext context, IAwaitable<string> result)
        {
            var selectedCard = await result;
            var message = context.MakeMessage();
            var attachment = GetSelectedCard(selectedCard);
            message.Attachments.Add(attachment);

            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);
        }

        private static Attachment GetSelectedCard(string selectedCard)
        {
            switch (selectedCard)
            {
                case SigninCard:
                    return GetSigninCard();

                default:
                    return GetHeroCard();
            }
        }

        private static Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "BotFramework Hero Card",
                Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.microsoft.com/bot-framework") }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetSigninCard()
        {
            var signinCard = new SigninCard
            {
                Text = "Mio宇达電通官方网站",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, "Sign-in", value: "https://advantage.mio.com/MioAdvantage/login/LoginAction!toLogin.action?com=zh_cn") }
            };

            return signinCard.ToAttachment();
        }

    }
}