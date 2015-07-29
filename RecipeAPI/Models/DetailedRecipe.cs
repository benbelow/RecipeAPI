using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RecipeAPI.Repositories;

namespace RecipeAPI.Models
{
    public class DetailedRecipe
    {
        private InstructionRepository InstructionRepo { get; set; }

        public DetailedRecipe() 
        {
            // TODO: Extract repo initialization and all joins into a service, keep this just for data
            InstructionRepo = new InstructionRepository(new RecipesEntities());
        }

        public DetailedRecipe(Recipe recipe) : this()
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
            Instructions = InstructionRepo.GetInstructionsForRecipe(recipe).Select(i => new DetailedInstruction(i));
        }

        public string Name;
        public string Description;
        public string MealType;
        public string PreparationTime;
        public string CookTime;
        public string NumberOfServings;
        public string Author;
        
        public IEnumerable<DetailedInstruction> Instructions { get; set; }
    }
}