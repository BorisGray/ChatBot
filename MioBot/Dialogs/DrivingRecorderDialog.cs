using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MioBot.Dialogs
{
    [Serializable]
    public class DrivingRecorderDialog : IDialog<object>
    {
        private const string dr786Option = "MiVue™ 786 ";
        private const string dr772Option = "MiVue™ 772 ";
        private const string drA30Option = "MiVue™ A30 ";

        private IEnumerable<string> options = new List<string> { dr786Option, dr772Option, drA30Option };

        public async Task StartAsync(IDialogContext context)
        {
            //context.Fail(new NotImplementedException(" DrivingRecorder Dialog is not implemented and is instead being used to show context.Fail"));

            context.Wait(MessageReceivedAsync);
            //return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            PromptDialog.Choice<string>(
                context,
                this.DisplaySelectedCard,
                this.options,
                @"請選擇機型：",
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
                case dr786Option:
                    return GetDr786Infor();
                case dr772Option:
                    return GetDr772Infor();
                case drA30Option:
                    return GetDrA30Infor();

                default:
                    return GetDr786Infor();
            }
        }

        private static Attachment GetDr786Infor()
        {
            var animationCard = new AnimationCard
            {
                Title = @"#MiVue™ 786  
                    WIFI实时 - 事件影片自动备份
                    采用索尼的低照度影像传感器
                    2.7电容式触控屏幕 
                    内建GPS",
                Subtitle = "2017 MiVue™ 7系列行車記錄器",
                Image = new ThumbnailUrl
                {
                    Url = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://www.mio.com.cn/images/product_overview_MiVue786_front_big_cn.gif"
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Learn More",
                        Type = ActionTypes.OpenUrl,
                        Value = "http://www.mio.com.cn/products-MiVue-786-overview.htm"
                    }
                 }
            };

            return animationCard.ToAttachment();
        }

        private static Attachment GetDr772Infor()
        {
            var animationCard = new AnimationCard
            {
                Title = @"#MiVue™ 772  
                   WIFI实时-事件影片自动备份 
                    采用索尼的低照度影像传感器 
                    2.7电容式触控屏幕 
                    独家二合一监控模式",
                Subtitle = "2017 MiVue™ 7系列行車記錄器",
                Image = new ThumbnailUrl
                {
                    Url = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://www.mio.com.cn/images/product_overview_MiVue772_front_big_cn.gif"
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Learn More",
                        Type = ActionTypes.OpenUrl,
                        Value = "http://www.mio.com.cn/products-MiVue-772-overview.htm"
                    }
                 }
            };

            return animationCard.ToAttachment();
        }

        private static Attachment GetDrA30Infor()
        {
            var animationCard = new AnimationCard
            {
                Title = @"MiVue™ A30  
                    采用索尼的影像传感器 
                    真实1080P/30fps高画质 
                    F1.8大光圈 
                    7段可调EV值",
                Subtitle = "2017 MiVue™ 7系列行車記錄器",
                Image = new ThumbnailUrl
                {
                    Url = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png"
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://www.mio.com.cn/images/product_overview_MiVueA30_front_big_cn.gif"
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Learn More",
                        Type = ActionTypes.OpenUrl,
                        Value = "http://www.mio.com.cn/products-MiVue-A30-overview.htm"
                    }
                 }
            };

            return animationCard.ToAttachment();
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { dr786Option, dr772Option, drA30Option}, " 請選擇機型：", "無效選擇！", 4);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case dr786Option:
                        //context.Call(new NavigatorDialog(), this.ResumeAfterOptionDialog);
                        // download
                        break;

                    case dr772Option:
                        //context.Call(new DrivingRecorderDialog(), this.ResumeAfterOptionDialog);\
                        // download
                        break;

                    case drA30Option:
                        //context.Call(new DrivingRecorderDialog(), this.ResumeAfterOptionDialog);
                        // download
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                //context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                //context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}