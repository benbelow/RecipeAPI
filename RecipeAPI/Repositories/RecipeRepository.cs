using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
    }

    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext context) : base(context) {}
    }
}