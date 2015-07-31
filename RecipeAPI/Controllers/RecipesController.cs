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
        private IInstructionRepository InstructionRepo { get; set; }
        private IIngredientRepository IngredientRepo { get; set; }
        private IRecipeIngredientRepository RecipeIngredientRepo { get; set; }
        private IEquipmentRepository EquipmentRepo { get; set; }
        private IRecipeEquipmentRepository RecipeEquipmentRepo{ get; set; }

        public RecipesController()
        {
            RecipeRepo = new RecipeRepository(new RecipesContext());
            InstructionRepo = new InstructionRepository(new RecipesContext());
            IngredientRepo = new IngredientRepository(new RecipesContext());
            RecipeIngredientRepo = new RecipeIngredientRepository(new RecipesContext());
            EquipmentRepo = new EquipmentRepository(new RecipesContext());
            RecipeEquipmentRepo = new RecipeEquipmentRepository(new RecipesContext());
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
        [Route("api/Recipes")]
        public IEnumerable<DetailedRecipe> GetRecipes(string name = "",
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
        [ActionName("With")]
        public IEnumerable<DetailedRecipe> WithWhatIHave([FromUri] List<string> ownedIngredients = null,
                                                            [FromUri] List<string> requiredIngredients = null,
                                                            [FromUri] List<string> equipment = null)
        {
            var totalIngredients = ownedIngredients.Concat(requiredIngredients).ToList();

            return RecipeRepo.GetAll().Select(r => new DetailedRecipe(r))
                                      .Where(r => r.Ingredients.All(ri => totalIngredients.Contains(ri.Name, StringComparison.OrdinalIgnoreCase)))
                                      .Where(r => requiredIngredients.All(i => r.Ingredients.Any(ri => i.Equals(ri.Name, StringComparison.OrdinalIgnoreCase))))
                                      .Where(r => equipment.Count == 0 || r.Equipment.All(re => equipment.Any(e => e.Equals(re.Name, StringComparison.OrdinalIgnoreCase))));
        }

        public HttpResponseMessage PostRecipe(string name,
                                              string description,
                                              string mealType,
                                              int prepTime,
                                              int cookTime,
                                              int numberOfServings,
                                              string author,
                                              [FromBody] RecipePostData postData)
        {
            var instructions = postData.Instructions;
            var ingredients = postData.Ingredients;
            var equipment = postData.Equipment;

            var recipe = new Recipe
            {
                Name = name,
                Description = description,
                MealType = mealType,
                PreparationTime = prepTime,
                CookTime = cookTime,
                NumberOfServings = numberOfServings,
                Author = author
            };

            RecipeRepo.Add(recipe);
            RecipeRepo.SaveContext();

            foreach (var detailedInstruction in instructions)
            {
                var instruction = new Instruction
                {
                    StepNumber = detailedInstruction.StepNumber,
                    StepDescription = detailedInstruction.StepDescription,
                    RecipeID = recipe.RecipeID
                };
                InstructionRepo.Add(instruction);
            }

            foreach (var detailedRecipeIngredient in ingredients)
            {
                var ingredient = IngredientRepo.GetIngredientByName(detailedRecipeIngredient.Name);

                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = detailedRecipeIngredient.Name
                    };

                    IngredientRepo.Add(ingredient);
                    IngredientRepo.SaveContext();
                }

                var recipeIngredient = new RecipeIngredient
                {
                    RecipeID = recipe.RecipeID,
                    IngredientID = ingredient.IngredientID,
                    Amount = detailedRecipeIngredient.Amount,
                    Units = detailedRecipeIngredient.Units
                };

                RecipeIngredientRepo.Add(recipeIngredient);
            }

            foreach (var detailedEquipment in equipment)
            {
                var singleEquipment = EquipmentRepo.GetEquipmentByName(detailedEquipment.Name);

                if (singleEquipment == null)
                {
                    singleEquipment = new Equipment
                    {
                        Name = detailedEquipment.Name,
                    };

                    EquipmentRepo.Add(singleEquipment);
                    EquipmentRepo.SaveContext();
                }

                var recipeEquipment = new RecipeEquipment
                {
                    RecipeID = recipe.RecipeID,
                    EquipmentID = singleEquipment.EquipmentID
                };

                RecipeEquipmentRepo.Add(recipeEquipment);
            }

            InstructionRepo.SaveContext();
            RecipeIngredientRepo.SaveContext();
            RecipeEquipmentRepo.SaveContext();

            var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }


    }
}
