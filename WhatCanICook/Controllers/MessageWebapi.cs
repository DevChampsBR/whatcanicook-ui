using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WhatCanICook
{
    public class MessageWebapi
    {
        public List<DtoResponse.Recipe> recipes;
        public int step = 0;

        public async Task Post([FromBody]Activity activity)
        {
            switch (step)
            {
                case 0:
                    await GetRecipes(activity);
                    break;
                case 1:
                    await SelectRecipe(activity);
                    break;
                default:
                    break;
            }
        }

        public async Task SelectRecipe(Activity activity)
        {
            var recipe = recipes.FirstOrDefault(x => x.name.Contains(activity.Text));
            var msg = "";
            if (recipe == null)
            {
                msg = "receita inválida";
            }
            else
            {
                msg = $"Modo de preparo: \r\n{string.Join(" - \r\n", recipe.directions.Select(x => x).ToList())}";
            }

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            await connector.Conversations.ReplyToActivityAsync(activity.CreateReply(msg));
        }

        public async Task GetRecipes(Activity activity)
        {
            var ingredients = activity.Text.Split(',').ToList();
            var ingredientsStr = new StringBuilder();

            for (int i = 0; i < ingredients.Count; i++)
            {
                ingredientsStr.Append($"Ingredients[{i}]={ingredients[i]}");
            };

            var url = $"http://whatcanicook-service.azurewebsites.net/api/recipe?{ingredientsStr.ToString()}";

            DtoResponse dtoResponse = null;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                dtoResponse = JsonConvert.DeserializeObject<DtoResponse>(json);
            }

            Activity reply;
            if (dtoResponse.success)
            {
                recipes = dtoResponse.recipes.ToList();
                reply = activity.CreateReply($"Qual receita você quer fazer? \r\n{string.Join(" - \r\n", dtoResponse.recipes.Select(x => x.name).ToList())}");
                step = 1;
            }
            else
            {
                var msg = string.Empty;
                if (dtoResponse.errors.Any())
                {
                    msg = $"{string.Join(" - \r\n", dtoResponse.errors.Select(x => x.message).ToList())}; ";
                }
                if (dtoResponse.invalidIngredients.Any())
                {
                    msg = $"{msg}{string.Join(" - \r\n", dtoResponse.invalidIngredients.Select(x => x).ToList())}";
                }
                reply = activity.CreateReply(msg);

            }

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            await connector.Conversations.ReplyToActivityAsync(reply);
        }
    }


    public class DtoResponse
    {
        public Recipe[] recipes { get; set; }
        public string[] invalidIngredients { get; set; }
        public Error[] errors { get; set; }
        public bool success { get; set; }

        public class Recipe
        {
            public string name { get; set; }
            public string image { get; set; }
            public string video { get; set; }
            public string[] directions { get; set; }
            public IngredientForRecipe[] ingredients { get; set; }
        }

        public class IngredientForRecipe
        {
            public Ingredient ingredient { get; set; }
            public Unit unit { get; set; }
            public decimal quantity { get; set; }
        }

        public class Ingredient
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Unit
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public class Error
        {
            public string message { get; set; }
        }
    }

}