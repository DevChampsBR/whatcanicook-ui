﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;

namespace WhatCanICook
{
    public enum CookBotState { Initial, Ingredients, Webapi, Intro };

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        static CookBotState _state = CookBotState.Intro;
        static public void SetInternalState(CookBotState state)
        {
            _state = state;
        }

        static MessageIntro messageIntro = new MessageIntro();
        static MessageWebapi messageWebApi = new MessageWebapi();

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            if (activity.Type == ActivityTypes.Message)
            {
                if (activity.Text.Equals("reset", StringComparison.CurrentCultureIgnoreCase))
                {
                    MessagesController.SetInternalState(CookBotState.Intro);
                    messageIntro = new MessageIntro();
                    messageWebApi = new MessageWebapi();
                }
                else
                {
                    switch (_state)
                    {
                        case CookBotState.Initial:
                            await MessageInitial.Post(activity);
                            break;
                        case CookBotState.Ingredients:
                            await MessageIngredientes.Post(activity);
                            break;
                        case CookBotState.Webapi:
                            await messageWebApi.Post(activity);
                            break;
                        case CookBotState.Intro:
                            await messageIntro.Post(activity);
                            break;
                    }
                }
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