using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WhatCanICook
{
    public class MessageIntro
    {
        public static async Task Post([FromBody]Activity activity)
        {
            await Task.Delay(0);
        }
    }
}