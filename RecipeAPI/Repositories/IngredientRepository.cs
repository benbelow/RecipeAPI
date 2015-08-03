using System.Linq;
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
            return Entities.SingleOrDefault(i => i.Name == name);
        }
    }
}