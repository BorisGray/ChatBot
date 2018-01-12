using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MioBot.Dialogs
{
    [LuisModel("", "")]

    [Serializable]
    public class MapInforUpdateDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"對不起, 不理解'{result.Query}'這句話的意思. 輸入 'help' 獲取幫助.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("地圖更新")]
        public async Task mapupdate(IDialogContext context, LuisResult result)
        {
            string message = $"您好，很樂意回答您關於地圖更新的問題，您貴姓？";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("打招呼")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            string message = $"您好，很开心可以跟您聊天，请问您贵姓？";
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
            string message = $"{FamilyName}先生（小姐），您好，請問您的機型是哪台";
            await context.PostAsync(message);

            context.Call(new BarCodeImageDialog(), this.ResumeAfterOptionDialog);
        }

        [LuisIntent("识别机器类型")]
        public async Task RecognizeProductType(IDialogContext context, LuisResult result)
        {
            string message = $"可否上传您的机型照片以供识别";
            await context.PostAsync(message);
            context.Call(new ProductImageDialog(), this.ResumeAfterOptionDialog);
        }

        [LuisIntent("登入完成")]
        public async Task LoginFinished(IDialogContext context, LuisResult result)
        {
            string download_url = "http://www.mio.com/cn/products-MiVue-786-overview.htm";
            string message = $"請點擊({download_url})導航欄下載";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("下載完成")]
        public async Task DownloadFinished(IDialogContext context, LuisResult result)
        {
            string message = $"請依步驟XX進行備份和更新";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("更新沒反應")]
        public async Task UpdateNoReponse(IDialogContext context, LuisResult result)
        {
            string message = $"您好，請問您的電腦操作系統是什麼？";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("更新完畢")]
        public async Task UpdateComplete(IDialogContext context, LuisResult result)
        {
            string message = $"恭喜！您已完成圖資更新。";
            await context.PostAsync(message);
            context.Done(true);
        }

        [LuisIntent("電腦操作系統類型")]
        public async Task QueryOS(IDialogContext context, LuisResult result)
        {
            string message = $"無法繼續下一步，需與客服聯繫，請點選對話(400-828-2777)或者郵件(vickymio.wei@mio.com)";
            await context.PostAsync(message);
            context.Done(false);
        }

        [LuisIntent("幫助")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("您好! 請依據提示輸入信息~");

            context.Wait(this.MessageReceived);
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
            //finally
            //{
            //    context.Wait(this.MessageReceivedAsync);
            //}
        }
    }
}