using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        Ingredient GetIngredientByName(string name);
    }

    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(DbContext context) : base(context) { }

        public Ingredient GetIngredientByName(string name)
        {
            return GetAll().SingleOrDefault(i => i.Name == name);
        }
    }
}