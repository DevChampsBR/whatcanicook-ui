using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using System.Web.Http;

namespace WhatCanICook
{

    public class MessageIntro
    {
        public static int status = 0;

        public static async Task Post([FromBody]Activity activity)
        {
            using (var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl)))
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    if (status == 0)
                    {
                        status = 1;
                        var reply = activity.CreateReply($"Bom dia, Filho.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                    else if(status == 1)
                    {
                        var reply = activity.CreateReply($"Mamãe tá cansada demais. Hoje você vai cozinhar! Vai ver o que tem disponível no armário e geladeira.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }
            }
        }
    }
}