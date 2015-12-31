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

        public RecipesController(IRecipeRepository recipeRepo, IRecipeIngredientRepository recipeIngredientRepo, IInstructionRepository instructionRepo, IIngredientRepository ingredientRepo, IEquipmentRepository equipmentRepo, IRecipeEquipmentRepository recipeEquipmentRepo)
        {
            RecipeRepo = recipeRepo;
            RecipeIngredientRepo = recipeIngredientRepo;
            InstructionRepo = instructionRepo;
            IngredientRepo = ingredientRepo;
            EquipmentRepo = equipmentRepo;
            RecipeEquipmentRepo = recipeEquipmentRepo;
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
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        [Route("api/Recipes")]
        public IEnumerable<DetailedRecipe> GetRecipes(string name = "",
                                               string mealType = "",
                                               [FromUri] List<string> ingredientsAny = null,
                                               [FromUri] List<string> ingredientsAll = null,
                                               [FromUri] List<string> equipment = null,
                                               int? maxTotalTime = null,
                                               int? minNumberOfServings = null,
                                               int limit = 100,
                                               int offset = 0)
        {
            return RecipeRepo.FilterRecipes(name, mealType, ingredientsAny, ingredientsAll, equipment, maxTotalTime,minNumberOfServings,limit,offset)
                             .Select(r => new DetailedRecipe(r));
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
            return RecipeRepo.FilterWithWhatIHave(ownedIngredients, requiredIngredients, equipment)
                             .Select(r => new DetailedRecipe(r));

        }

        public HttpResponseMessage PostRecipe(string name,
                                              string description,
                                              string mealType,
                                              int prepTime,
                                              int cookTime,
                                              int numberOfServings,
                                              string author,
                                              [FromBody] RecipePostData postData,
                                              string imageSource = null)
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
                Author = author,
                ImageSource = imageSource
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
                    Units = detailedRecipeIngredient.Units,
                    Description = detailedRecipeIngredient.Description
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
