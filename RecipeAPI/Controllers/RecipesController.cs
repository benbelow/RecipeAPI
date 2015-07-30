using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using RecipeAPI.Repositories;
using RecipeAPI.Models;
using RecipeAPI.Helpers.Extensions;

namespace RecipeAPI.Controllers
{
    public class RecipesController : ApiController
    {
        private IRecipeRepository RecipeRepo { get; set; }

        public RecipesController()
        {
            RecipeRepo = new RecipeRepository(new RecipesEntities());
        }

        // GET api/recipes
        /// <summary>
        /// Searches all recipes
        /// </summary>
        /// <param name="name">Recipe name</param>
        /// <param name="mealType">Type of meal</param>
        /// <param name="ingredientsAny">List of ingredients, any of which should be in the recipe</param>
        /// <param name="ingredientsAll">List of ingredients, all of which should be in the recipe</param>
        /// <param name="equipment">List of cooking equipment, any of which should be used in in the recipe</param>
        /// <param name="maxTotalTime">Maximum time to prepare and cook the meal</param>
        /// <param name="minNumberOfServings">Minimum number of servings</param>
        public IEnumerable<DetailedRecipe> Get(string name = "",
                                               string mealType = "",
                                               [FromUri] List<string> ingredientsAny = null,
                                               [FromUri] List<string> ingredientsAll = null,
                                               [FromUri] List<string> equipment = null,
                                               int? maxTotalTime = null,
                                               int? minNumberOfServings = null)
        {
            return RecipeRepo.GetAll().Select(r => new DetailedRecipe(r))
                                      .Where(r => name.Split(' ').All(substring => r.Name.Contains(substring, StringComparison.OrdinalIgnoreCase)))
                                      .Where(r => mealType == "" || r.MealType.Equals(mealType, StringComparison.OrdinalIgnoreCase))
                                      .Where(r => ingredientsAll.All(i => r.Ingredients.Any(ri => ri.Name.Equals(i, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => ingredientsAny.Count == 0 || ingredientsAny.Concat(ingredientsAll).Any(i => r.Ingredients.Any(ri => ri.Name.Equals(i, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => equipment.Count == 0 || equipment.Any(e => r.Equipment.Any(re => re.Name.Equals(e, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => maxTotalTime == null || r.PreparationTime + r.CookTime <= maxTotalTime)
                                      .Where(r => minNumberOfServings == null || r.NumberOfServings >= minNumberOfServings);
        }

    }
}
