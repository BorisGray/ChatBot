using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace MioBot.Dialogs
{
    [LuisModel("", "")]

    [Serializable]
    public class UpdateOptionDialog : LuisDialog<object>
    {
        private const string NavigatorOption = "導航儀";
        private const string DrivingRecorderOption = "行車記錄器";

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"對不起, 不理解'{result.Query}'這句話的意思. 輸入 'help' 獲取幫助.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("提供選項的更新")]
        public async Task AskingForHelp(IDialogContext context, LuisResult result)
        {
            string message = $"您好，很樂意回答您關於更新的問題，您貴姓？";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("回復稱呼")]
        public async Task AskingName(IDialogContext context, LuisResult result)
        {
            string FamilyName = string.Empty;
            EntityRecommendation title;
            if (result.TryFindEntity("family_name", out title))
            {
                FamilyName = title.Entity;
            }
            string message = $"{FamilyName}先生（小姐），您好，請選擇您的機型：";
            this.ShowOptions(context, message);
        }

        private void ShowOptions(IDialogContext context, string message)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { NavigatorOption, DrivingRecorderOption }, message, "Not a valid option", 3);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case NavigatorOption:
                        context.Call(new NavigatorDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case DrivingRecorderOption:
                        context.Call(new DrivingRecorderDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
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