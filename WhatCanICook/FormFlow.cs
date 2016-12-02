using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace WhatCanICook
{
    public enum IngredientsOptions
    {
        Agua, Farinha, Leite, Acucar
    };

    [Serializable]
    public class IngredientsList
    {
        
        [Prompt("Selecione os ingredientes que você possui: {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
        public IngredientsOptions? Ingredients;
        
        [Prompt("Digite os ingredientes que você possui: ")]
        public string TesteIngredientes;

        public static IForm<IngredientsList> BuildForm()
        {
            OnCompletionAsyncDelegate<IngredientsList> processRequest = async (context, state) =>
            {                
                //Chamar api:

                await context.PostAsync(string.Concat($@"Com os ingredientes : ", state.TesteIngredientes, " podemos fazer: "));
            };


            return new FormBuilder<IngredientsList>()
                //.Field(nameof(Ingredients),
                //    validate: async
                // )
                .Message("Oi. Mamãe não deixou nada pronto pra você comer? Não tem problema, eu te ajudo!")
                .Field(nameof(TesteIngredientes))
                .Confirm("Vamos buscar o que fazer com estes ingredientes: {TesteIngredientes}?")
                .OnCompletion(processRequest)
                .Build();
                
        }
        
    }

}