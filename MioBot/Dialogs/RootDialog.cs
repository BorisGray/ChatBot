using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;

namespace MioBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string CompareProduct = "比較不同型號產品";
        private const string MapInforUpdate = "圖資更新";
        private const string UpdateOption = "更新（提供選項）";
        private const string ProductFault = "產品故障";
        private const string RepairQuery = "維修查詢";
        private const string MioFAQ = "常見問題咨詢";

        private const string NavigatorOption = "导航";
        private const string cdRecorderOption = "行车记录仪";
        private readonly ResumeAfter<object> MessageReceived;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message == null)
            {
                string msg = "請輸入文字！！";
                await context.PostAsync(msg);
            }

            if (message.Text.ToLower().Contains("help") || message.Text.ToLower().Contains("support") || message.Text.ToLower().Contains("problem"))
            {
                await context.Forward(new SupportDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
            }
            else
            {
                this.ShowOptions(context);
            }
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, 
                this.OnOptionSelected, 
                new List<string>() { CompareProduct,MapInforUpdate, UpdateOption, ProductFault, RepairQuery, MioFAQ }, 
                "## 請選擇服務類型：", 
                "無效選擇！", 
                7);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case CompareProduct:
                        context.Call(new CompareProductDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case MapInforUpdate:
                        context.Call(new MapInforUpdateDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case UpdateOption:
                        context.Call(new UpdateOptionDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case ProductFault:
                        context.Call(new ProductFaultDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case RepairQuery:
                        context.Call(new RepairQueryDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case MioFAQ:
                        context.Call(new QnaFAQDialog(), this.ResumeAfterOptionDialog);
                        break;
                    default:
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
                await this.StartAsync(context);
            }
        }

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            var ticketNumber = await result;

            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
            await this.StartAsync(context);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                //await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                await this.StartAsync(context);
            }
        }
    }
}