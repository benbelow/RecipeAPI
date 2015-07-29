using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RecipeAPI.Repositories;
using RecipeAPI.Models;

namespace RecipeAPI.Models
{
    public class DetailedRecipe
    {

        public DetailedRecipe(Recipe recipe) {
            Name = recipe.Name;
            Description = recipe.Description; ;
            MealType = recipe.MealType;
            PreparationTime = recipe.PreparationTime.ToString();
            CookTime = recipe.CookTime.ToString();
            NumberOfServings = recipe.NumberOfServings.ToString();
            Author = recipe.Author;

            Instructions = recipe.Instructions.Select(i => new DetailedInstruction(i));
            Ingredients = recipe.RecipeIngredients.Select(i => new DetailedIngredient(i));
            Equipment = recipe.RecipeEquipments.Select(e => new DetailedEquipment(e));
        }

        public string Name;
        public string Description;
        public string MealType;
        public string PreparationTime;
        public string CookTime;
        public string NumberOfServings;
        public string Author;

        public IEnumerable<DetailedInstruction> Instructions { get; set; }

        public IEnumerable<DetailedIngredient> Ingredients { get; set; }

        public IEnumerable<DetailedEquipment> Equipment { get; set; }
    }
}