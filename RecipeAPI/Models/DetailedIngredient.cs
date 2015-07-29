using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class DetailedIngredient
    {
        public DetailedIngredient(RecipeIngredient recipeIngredient)
        {
            Name = recipeIngredient.Ingredient.Name;
            Amount = recipeIngredient.Amount.ToString();
            Units = recipeIngredient.Units;
        }

        public String Name { get; set; }
        public String Amount { get; set; }
        public String Units { get; set; }
    }
}