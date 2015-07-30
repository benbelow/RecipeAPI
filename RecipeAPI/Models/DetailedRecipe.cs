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
            PreparationTime = recipe.PreparationTime;
            CookTime = recipe.CookTime;
            NumberOfServings = recipe.NumberOfServings;
            Author = recipe.Author;

            Instructions = recipe.Instructions.Select(i => new DetailedInstruction(i));
            Ingredients = recipe.RecipeIngredients.Select(i => new DetailedRecipeIngredient(i));
            Equipment = recipe.RecipeEquipments.Select(e => new DetailedEquipment(e));
        }

        public string Name;
        public string Description;
        public string MealType;
        public int PreparationTime;
        public int CookTime;
        public int NumberOfServings;
        public string Author;

        public IEnumerable<DetailedInstruction> Instructions { get; set; }

        public IEnumerable<DetailedRecipeIngredient> Ingredients { get; set; }

        public IEnumerable<DetailedEquipment> Equipment { get; set; }
    }
}