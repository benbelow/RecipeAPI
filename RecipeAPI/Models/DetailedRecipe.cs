using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace RecipeAPI.Models
{
    public class DetailedRecipe
    {
        public DetailedRecipe() { }

        public DetailedRecipe(Recipe recipe)
        {
            PopulateFromRecipe(recipe);
        }

        private void PopulateFromRecipe(Recipe recipe)
        {
            Name = recipe.Name;
            Description = recipe.Description;
            MealType = recipe.MealType;
            PreparationTime = recipe.PreparationTime.ToString();
            CookTime = recipe.CookTime.ToString();
            NumberOfServings = recipe.NumberOfServings.ToString();
            Author = recipe.Author;
            Instructions = new List<Instruction>();
        }

        public string Name;
        public string Description;
        public string MealType;
        public string PreparationTime;
        public string CookTime;
        public string NumberOfServings;
        public string Author;
        
        public ICollection<Instruction> Instructions { get; set; }
    }
}