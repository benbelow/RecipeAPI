using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeAPI.Models;
using System.Data.Entity;
using RecipeAPI.Helpers.Extensions;

namespace RecipeAPI.Repositories
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        IEnumerable<Recipe> FilterRecipes(string name = "",
                            string mealType = "",
                            List<string> ingredientsAny = null,
                            List<string> ingredientsAll = null,
                            List<string> equipment = null,
                            int? maxTotalTime = null,
                            int? minNumberOfServings = null,
                            int limit = 100,
                            int offset = 0);
    }

    public class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext context) : base(context) {}
        
        public IEnumerable<Recipe> FilterRecipes(string name, string mealType, List<string> ingredientsAny, List<string> ingredientsAll,
                                                 List<string> equipment, int? maxTotalTime, int? minNumberOfServings, int limit, int offset)
        {
            ingredientsAll = ingredientsAll ?? new List<string>();
            ingredientsAny = ingredientsAny ?? new List<string>();
            equipment = equipment?? new List<string>();

            var totalIngredients = ingredientsAny.Concat(ingredientsAll).ToList();

            var nameSearchTerms = name.Split(' ');

            return Entities
                .Where(r => nameSearchTerms.All(substring => substring == "" || r.Name.Contains(substring)))
                .Where(r => mealType == "" || r.MealType.Equals(mealType, StringComparison.OrdinalIgnoreCase))
                .Where(r =>ingredientsAll.All(i =>r.RecipeIngredients.Any(ri => ri.Ingredient.Name.Equals(i, StringComparison.OrdinalIgnoreCase))))
                .Where(r =>ingredientsAny.Count == 0 ||totalIngredients.Any(i =>r.RecipeIngredients.Any(ri => ri.Ingredient.Name.Equals(i, StringComparison.OrdinalIgnoreCase))))
                .Where(r =>equipment.Count == 0 ||equipment.Any(e =>r.RecipeEquipments.Any(re => re.Equipment.Name.Equals(e, StringComparison.OrdinalIgnoreCase))))
                .Where(r => maxTotalTime == null || r.PreparationTime + r.CookTime <= maxTotalTime)
                .Where(r => minNumberOfServings == null || r.NumberOfServings >= minNumberOfServings)
                .OrderBy(r => r.RecipeID)
                .Skip(offset)
                .Take(limit);
        }
    }
}