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
    [Serializable]
    public class ProductFaultDialog : LuisDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Fail(new NotImplementedException("ProductFault Dialog is not implemented and is instead being used to show context.Fail"));
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"對不起, 不理解'{result.Query}'這句話的意思. 輸入 'help' 獲取幫助.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
    }
}