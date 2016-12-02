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
using Microsoft.Bot.Builder.FormFlow;

namespace WhatCanICook
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        internal static IDialog<IngredientsList> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(IngredientsList.BuildForm));
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //StateClient stateClient = activity.GetStateClient();

                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                /*
                ChannelAccount botAccount = new ChannelAccount("1234", "WhatCanICook");
                ChannelAccount userAccount = new ChannelAccount("teste", "Thiago");

                var conversationId = await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);

                IMessageActivity message = Activity.CreateMessageActivity();
                message.From = botAccount;
                message.Recipient = userAccount;
                message.Conversation = new ConversationAccount(id: conversationId.Id);
                message.Text = "Olá!";
                message.Locale = "pt-BR";

                await connector.Conversations.SendToConversationAsync((Activity)message);
                */

                // calculate something for us to return
                //int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //await connector.Conversations.ReplyToActivityAsync(reply);

                //Forms flow
                await Conversation.SendAsync(activity, MakeRootDialog);               
                
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
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
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