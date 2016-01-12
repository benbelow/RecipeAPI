using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IStoreCupboardIngredientRepository : IRepository<StoreCupboardIngredient>
    {
        IEnumerable<Ingredient> GetIngredientsForUser(int userId);
    }

    public class StoreCupboardIngredientRepository : Repository<StoreCupboardIngredient>, IStoreCupboardIngredientRepository
    {
        public StoreCupboardIngredientRepository(DbContext context) : base(context) { }

        public IEnumerable<Ingredient> GetIngredientsForUser(int userId)
        {
            return Entities.Where(i => i.UserID == userId).Select(i => i.Ingredient);
        }
    }
}