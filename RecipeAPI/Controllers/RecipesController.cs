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
using JsonPatch;
using RecipeAPI.Authentication;

namespace RecipeAPI.Controllers
{
    public class RecipesController : ApiController
    {
        private IRecipeRepository RecipeRepo { get; set; }
        private IInstructionRepository InstructionRepo { get; set; }
        private IIngredientRepository IngredientRepo { get; set; }
        private IRecipeIngredientRepository RecipeIngredientRepo { get; set; }
        private IEquipmentRepository EquipmentRepo { get; set; }
        private IRecipeEquipmentRepository RecipeEquipmentRepo { get; set; }
        private IUserRepository UserRepo { get; set; }

        public RecipesController(IRecipeRepository recipeRepo, IRecipeIngredientRepository recipeIngredientRepo, IInstructionRepository instructionRepo, IIngredientRepository ingredientRepo, IEquipmentRepository equipmentRepo, IRecipeEquipmentRepository recipeEquipmentRepo, IUserRepository userRepo)
        {
            RecipeRepo = recipeRepo;
            RecipeIngredientRepo = recipeIngredientRepo;
            InstructionRepo = instructionRepo;
            IngredientRepo = ingredientRepo;
            EquipmentRepo = equipmentRepo;
            RecipeEquipmentRepo = recipeEquipmentRepo;
            UserRepo = userRepo;
        }

        // GET api/recipes
        /// <summary>
        /// Searches all recipes
        /// </summary>
        /// <param name="name">Recipe name</param>
        /// <param name="mealType">Type of meal</param>
        /// <param name="ingredients">List of ingredients, all of which should be in the recipe</param>
        /// <param name="equipment">List of cooking equipment, all of which should be used in in the recipe</param>
        /// <param name="restrictIngredients">Whether to restrict ingredients to just those in the store cupboard of the current user</param>
        /// <param name="restrictEquipment">Whether to restrict equipment to just that in the store cupboard of the current user</param>
        /// <param name="maxTotalTime">Maximum time to prepare and cook the meal</param>
        /// <param name="minNumberOfServings">Minimum number of servings</param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        [Route("api/Recipes")]
        public HttpResponseMessage GetRecipes(string name = null,
                                               string mealType = null,
                                               [FromUri] List<string> ingredients = null,
                                               [FromUri] List<string> equipment = null,
                                               bool restrictIngredients = false,
                                               bool restrictEquipment = false,
                                               int? maxTotalTime = null,
                                               int? minNumberOfServings = null,
                                               int limit = 100,
                                               int offset = 0)
        {

            var recipes = RecipeRepo.GetAllQuery();

            if (name != null)
            {
                recipes = recipes.FilterByName(name);
            }

            if (mealType != null)
            {
                recipes = recipes.FilterByMealType(mealType);
            }

            if (ingredients != null)
            {
                recipes = recipes.FilterByIngredients(ingredients);
            }

            if (equipment != null)
            {
                recipes = recipes.FilterByEquipment(equipment);
            }

            if (maxTotalTime != null)
            {
                recipes = recipes.FilterByMaxTime(maxTotalTime);
            }

            if (minNumberOfServings != null)
            {
                recipes = recipes.FilterByMinServings(minNumberOfServings);
            }

            if (restrictIngredients || restrictEquipment)
            {
                var principal = RequestContext.Principal;
                if (!(principal is UserPrincipal))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                var userId = ((UserPrincipal)principal).UserId;

                var user = UserRepo.GetUserById(userId);

                if (restrictIngredients)
                {
                    recipes = recipes.RestrictByIngredients(user.StoreCupboardIngredients.Select(i => i.Ingredient.Name).ToList());
                }

                if (restrictEquipment)
                {
                    recipes = recipes.RestrictByEquipment(user.StoreCupboardEquipments.Select(e => e.Equipment.Name).ToList());
                }

            }

            var detailedRecipes = recipes.OrderBy(r => r.RecipeID)
                          .Skip(offset)
                          .Take(limit)
                          .ToList()
                          .Select(r => new DetailedRecipe(r));

            return Request.CreateResponse(HttpStatusCode.OK, detailedRecipes);
        }

        public HttpResponseMessage GetRecipeByID(int id)
        {
            HttpResponseMessage response;
            var recipe = RecipeRepo.GetRecipeById(id);
            if (recipe == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, new DetailedRecipe(recipe));
            }
            return response;
        }

        [HttpPost]
        [Route("api/Recipes")]
        public HttpResponseMessage PostRecipe([FromBody] RecipePostData postData)
        {
            var instructions = postData.Instructions;
            var ingredients = postData.Ingredients;
            var equipment = postData.Equipment;

            var recipe = new Recipe
            {
                Name = postData.name,
                Description = postData.description,
                MealType = postData.mealType,
                PreparationTime = postData.prepTime,
                CookTime = postData.cookTime,
                NumberOfServings = postData.numberOfServings,
                Author = postData.author,
                ImageSource = postData.imageSource
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

            var response = Request.CreateResponse(HttpStatusCode.Created, new DetailedRecipe(recipe));
            return response;
        }

        [HttpPatch]
        public HttpResponseMessage PatchRecipe(int id, [FromBody] JsonPatchDocument<Recipe> recipePatchDocument)
        {
            var recipe = RecipeRepo.GetRecipeById(id);
            if (recipe == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            recipePatchDocument.ApplyUpdatesTo(recipe);
            RecipeRepo.SaveContext();

            return Request.CreateResponse(HttpStatusCode.OK, new DetailedRecipe(recipe));
        }


        public HttpResponseMessage DeleteRecipe(int id)
        {
            var recipe = RecipeRepo.GetRecipeById(id);
            if (recipe == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            RecipeRepo.DeleteRecipe(recipe);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
