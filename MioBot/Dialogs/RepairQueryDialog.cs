// Added by guobingxue

namespace MioBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Diagnostics;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Newtonsoft.Json;
    using System.Net;
    using System.Threading;

    [LuisModel("", "")]
    [Serializable]
    public class RepairQueryDialog : LuisDialog<object>
    {
        private const string PhoneRegexPattern = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
        private const string SNRegexPattern = null;
        private const string RepairSheetNoRegexPattern = null;

        [LuisIntent("")]  //luis Intent : the element of the Luis App
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"對不起, 不理解'{result.Query}'這句話的意思. 輸入 'help' 獲取幫助.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("維修進度查詢")]
        public async Task repairprogress(IDialogContext context, LuisResult result)
        {
            string message = $"您好，很樂意回答您關於維修的問題，您貴姓？";
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
            string message = $"{FamilyName}先生（小姐），您好，請協助提供報修人電話或機器序列號或維修單號。";
            //await context.PostAsync(message);
            //context.Wait(MessageReceived);

            var promptPhoneDialog = new PromptStringRegex(
              message,
              PhoneRegexPattern,
              "輸入的數據不是電話號碼. 請按照以下格式輸入 (xyz) xyz-wxyz:",
              "您嘗試輸入電話號碼的次數超過規定的次數. 請稍後嘗試.",
              attempts: 3);

            context.Call(promptPhoneDialog, this.ResumeAfterPhoneSnNumberEntered);
        }

        //this part should be replaced by the bar code recognition
        [LuisIntent("二維碼識別")]
        public async Task BarCodeRecog(IDialogContext context, LuisResult result)
        {
            string SerialCode = string.Empty;
            EntityRecommendation title;
            if (result.TryFindEntity("序列號", out title))
            {
                SerialCode = title.Entity;
            }
            string message = $"您的機器序列號是{SerialCode}，目前正在維修，如需進一步查詢請選擇電話(88888888)或者郵件(66666@163.com)";

            await context.PostAsync(message);
        
            context.Wait(MessageReceived);
        }

        private async Task ResumeAfterPhoneSnNumberEntered(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var QueryNo = await result;

                /*await*/ HttpApiAsyncTask.QueryRepairProgressAsync(QueryNo);
                Trace.WriteLine(string.Format("Enter phone number is: {0}", QueryNo));

                string message = "目前正在查詢，若需進一步查詢，請點選對話(400-828-2777)或者郵件(vickymio.wei@mio.com)";
                await context.PostAsync(message);
                context.Done(false);

            }
            catch (Exception ex)
            {
                //await context.PostAsync($"Failed with message: {ex.Message}");
            }
            //finally
            //{
            //    context.Wait(this.MessageReceivedAsync);
            //}
        }
    }

}