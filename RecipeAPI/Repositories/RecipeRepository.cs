using System;
using System.Collections.Generic;
using System.Linq;
using RecipeAPI.Models;
using System.Data.Entity;

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

        IEnumerable<Recipe> FilterWithWhatIHave(List<string> ownedIngredients = null,
                                                List<string> requiredIngredients = null,
                                                List<string> equipment = null);

        Recipe GetRecipeById(int id);
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
                .Where(r => nameSearchTerms.All(substring => substring.Length == 0 || r.Name.Contains(substring)))
                .Where(r => mealType.Length == 0 || r.MealType == mealType)
                .Where(r => ingredientsAll.All(i =>r.RecipeIngredients.Any(ri => ri.Ingredient.Name == i)))
                .Where(r => ingredientsAny.Count == 0 || totalIngredients.Any(i =>r.RecipeIngredients.Any(ri => ri.Ingredient.Name == i)))
                .Where(r => equipment.Count == 0 || equipment.Any(e =>r.RecipeEquipments.Any(re => re.Equipment.Name == e)))
                .Where(r => maxTotalTime == null || r.PreparationTime + r.CookTime <= maxTotalTime)
                .Where(r => minNumberOfServings == null || r.NumberOfServings >= minNumberOfServings)
                .OrderBy(r => r.RecipeID)
                .Skip(offset)
                .Take(limit);
        }

        public IEnumerable<Recipe> FilterWithWhatIHave(List<string> ownedIngredients, List<string> requiredIngredients, List<string> equipment)
        {
            ownedIngredients = ownedIngredients ?? new List<string>();
            requiredIngredients = requiredIngredients ?? new List<string>();
            equipment = equipment ?? new List<string>();
            var totalIngredients = ownedIngredients.Concat(requiredIngredients).ToList();

            return Entities
                .Where(r => r.RecipeIngredients.All(ri => totalIngredients.Contains(ri.Ingredient.Name)))
                .Where(r => requiredIngredients.All(i => r.RecipeIngredients.Any(ri => i == ri.Ingredient.Name)))
                .Where(r => equipment.Count == 0 || r.RecipeEquipments.All(re => equipment.Any(e => e == re.Equipment.Name)));
        
        }

        public Recipe GetRecipeById(int id)
        {
            return Entities.Where(r => r.RecipeID == id).SingleOrDefault();
        }
    }
}