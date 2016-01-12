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
    }

    public class StoreCupboardIngredientRepository : Repository<StoreCupboardIngredient>, IStoreCupboardIngredientRepository
    {
        public StoreCupboardIngredientRepository(DbContext context) : base(context) { }
    }
}