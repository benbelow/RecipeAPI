using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class DetailedRecipeIngredient
    {
        public DetailedRecipeIngredient() { }

        public DetailedRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            Name = recipeIngredient.Ingredient.Name;
            Amount = recipeIngredient.Amount;
            Units = recipeIngredient.Units;
            Description = recipeIngredient.Description;
        }

        public String Name { get; set; }
        public double Amount { get; set; }
        public String Units { get; set; }
        public String Description { get; set; }
    }
}