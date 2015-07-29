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
    public class IngredientsController : ApiController
    {
        private IIngredientRepository IngredientRepo { get; set; }

        public IngredientsController()
        {
            IngredientRepo = new IngredientRepository(new RecipesEntities());
        }

        // GET api/ingredients
        public IEnumerable<DetailedIngredient> Get()
        {
            return IngredientRepo.GetAll().Select(i => new DetailedIngredient(i));
        }
    }
}
