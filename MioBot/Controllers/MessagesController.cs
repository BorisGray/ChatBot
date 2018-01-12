using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;

namespace MioBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    string strCurrentURL =
                        this.Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");

                    // Create a reply message
                    Activity replyToConversation = message.CreateReply();
                    replyToConversation.Recipient = message.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();
                    // AttachmentLayout options are list or carousel
                    replyToConversation.AttachmentLayout = "carousel";

                    #region Card One
                    // Full URL to the image
                    string strOpeningCard =
                        String.Format(@"{0}/{1}",
                        strCurrentURL,
                        "Data/logo.gif");

                    // Create a CardImage and add our image
                    List<CardImage> cardImages1 = new List<CardImage>();
                    cardImages1.Add(new CardImage(url: strOpeningCard));

                    // Create a CardAction to make the HeroCard clickable
                    // Note this does not work in some Skype clients
                    CardAction btnMioBotWebsite = new CardAction()
                    {
                        Type = "openUrl",
                        Title = "MioBot",
                        Value = "http://www.mio.com.cn/"
                    };

                    // Finally create the Hero Card
                    // adding the image and the CardAction
                    HeroCard plCard1 = new HeroCard()
                    {
                        Title = @"Mio宇达电通-行车记录仪及GPS导航仪领导品牌",
                        Subtitle = @"歡迎進入MioBot人機對話世界!",
                        Images = cardImages1,
                        Tap = btnMioBotWebsite
                    };

                    // Create an Attachment by calling the
                    // ToAttachment() method of the Hero Card
                    Attachment plAttachment1 = plCard1.ToAttachment();

                    // Add the Attachment to the reply message
                    replyToConversation.Attachments.Add(plAttachment1);
                    #endregion

                    // Create a ConnectorClient and use it to send the reply message
                    var connector =
                        new ConnectorClient(new Uri(message.ServiceUrl));
                    var reply =
                        connector.Conversations.SendToConversationAsync(replyToConversation);

                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}