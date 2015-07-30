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
            var totalIngredients = ingredientsAny.Concat(ingredientsAll).ToList();

            return RecipeRepo.GetAll().Select(r => new DetailedRecipe(r))
                                      .Where(r => name.Split(' ').All(substring => r.Name.Contains(substring, StringComparison.OrdinalIgnoreCase)))
                                      .Where(r => mealType == "" || r.MealType.Equals(mealType, StringComparison.OrdinalIgnoreCase))
                                      .Where(r => ingredientsAll.All(i => r.Ingredients.Any(ri => ri.Name.Equals(i, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => ingredientsAny.Count == 0 || totalIngredients.Any(i => r.Ingredients.Any(ri => ri.Name.Equals(i, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => equipment.Count == 0 || equipment.Any(e => r.Equipment.Any(re => re.Name.Equals(e, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => maxTotalTime == null || r.PreparationTime + r.CookTime <= maxTotalTime)
                                      .Where(r => minNumberOfServings == null || r.NumberOfServings >= minNumberOfServings);
        }

        /// <summary>
        /// Searches recipes that only contain ingredients and recipes you specify
        /// </summary>
        /// <param name="ownedIngredients">Ingredients you have but do not require to be in the dish</param>
        /// <param name="requiredIngredients">Ingredients you have which the dish must contain</param>
        /// <param name="equipment">Equipment you have. Leave blank to not filter by equipment</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<DetailedRecipe> GetWithWhatIHave([FromUri] List<string> ownedIngredients = null,
                                                            [FromUri] List<string> requiredIngredients = null,
                                                            [FromUri] List<string> equipment = null)
        {
            var totalIngredients = ownedIngredients.Concat(requiredIngredients).ToList();

            return RecipeRepo.GetAll().Select(r => new DetailedRecipe(r))
                                      .Where(r => r.Ingredients.All(ri => totalIngredients.Contains(ri.Name, StringComparison.OrdinalIgnoreCase)))
                                      .Where(r => requiredIngredients.All(i => r.Ingredients.Any(ri => i.Equals(ri.Name, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => equipment.Count == 0 || r.Equipment.All(re => equipment.Any(e => e.Equals(re.Name, StringComparison.OrdinalIgnoreCase))));
        }


    }
}
