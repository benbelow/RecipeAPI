using System.Linq;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        Ingredient GetIngredientByName(string name);
        Ingredient GetOrCreateIngredient(string name);
    }

    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(DbContext context) : base(context) { }

        public Ingredient GetIngredientByName(string name)
        {
            return Entities.SingleOrDefault(i => i.Name == name);
        }

        public Ingredient GetOrCreateIngredient(string name)
        {
            var ingredient = GetIngredientByName(name);
            if (ingredient != null) {
                return ingredient;
            }

            ingredient = new Ingredient
            {
                Name = name
            };

            Add(ingredient);
            SaveContext();

            return ingredient;
        }
    }
}