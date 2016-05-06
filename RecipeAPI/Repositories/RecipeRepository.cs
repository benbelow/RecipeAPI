using System;
using System.Collections.Generic;
using System.Linq;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Recipe GetRecipeById(int id);
        void DeleteRecipe(Recipe recipe);
        void DeleteRecipe(int id);
    }

    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext context) : base(context) {}
      
        public Recipe GetRecipeById(int id)
        {
            return Entities.Where(r => r.RecipeID == id).SingleOrDefault();
        }

        public void DeleteRecipe(Recipe recipe)
        {
            Entities.Remove(recipe);
            SaveContext();
        }

        public void DeleteRecipe(int id)
        {
            Recipe recipe = new Recipe() { RecipeID = id };
            DeleteRecipe(recipe);
        }

    }
}