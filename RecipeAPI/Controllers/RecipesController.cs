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
        public IEnumerable<DetailedRecipe> Get(string name = "", [FromUri] List<string> ingredients = null)
        {
            return RecipeRepo.GetAll().Select(r => new DetailedRecipe(r))
                                      .Where(r => name.Split(' ').All(substring => r.Name.Contains(substring, StringComparison.OrdinalIgnoreCase)))
                                      .Where(r => ingredients.All(i => r.Ingredients.Any(ri => ri.Name.Equals(i, StringComparison.OrdinalIgnoreCase))));
        }

    }
}
