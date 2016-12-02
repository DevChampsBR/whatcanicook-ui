using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace WhatCanICook
{
    public class MessageIngredientes
    {
        internal static IDialog<IngredientsList> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(IngredientsList.BuildForm));
        }

        public static async Task Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, MakeRootDialog);
            }
        }
    }
}