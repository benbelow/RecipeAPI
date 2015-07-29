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
    public class RecipesController : ApiController
    {
        private IRecipeRepository RecipeRepo { get; set; }

        public RecipesController()
        {
            RecipeRepo = new RecipeRepository(new RecipesEntities());
        }

        // GET api/recipes
        public IEnumerable<DetailedRecipe> Get()
        {
            return RecipeRepo.GetAll().Select(r => new DetailedRecipe(r));
        }
    }
}
