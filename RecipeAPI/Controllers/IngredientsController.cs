using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using RecipeAPI.Repositories;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    [Authorize]
    public class IngredientsController : ApiController
    {
        private IIngredientRepository IngredientRepo { get; set; }

        public IngredientsController(IIngredientRepository ingredientRepo)
        {
            IngredientRepo = ingredientRepo;
        }

        // GET api/ingredients
        public IEnumerable<DetailedIngredient> Get()
        {
            return IngredientRepo.GetAll().Select(i => new DetailedIngredient(i));
        }

        [HttpPost]
        public HttpResponseMessage PostIngredient(string name)
        {
            var ingredient = new Ingredient { Name = name };
            IngredientRepo.Add(ingredient);
            IngredientRepo.SaveContext();

            var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
