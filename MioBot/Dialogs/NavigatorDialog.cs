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
    public class NavigatorDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Fail(new NotImplementedException("Navigator Dialog is not implemented and is instead being used to show context.Fail"));
        }
    }
}