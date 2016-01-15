using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Repositories
{
    public static class RecipeFilters
    {
        public static IQueryable<Recipe> FilterByName(this IQueryable<Recipe> recipes, string name)
        {
            var nameSearchTerms = name.Split(' ');
            return recipes.Where(r => nameSearchTerms.All(substring => substring.Length == 0 || r.Name.Contains(substring)));
        }

        public static IQueryable<Recipe> FilterByMealType(this IQueryable<Recipe> recipes, string mealType)
        {
            return recipes.Where(r => mealType.Length == 0 || r.MealType == mealType);
        }

        public static IQueryable<Recipe> FilterByIngredients(this IQueryable<Recipe> recipes, List<string> ingredients)
        {
            return recipes.Where(r => ingredients.All(i => r.RecipeIngredients.Any(ri => ri.Ingredient.Name == i)));
        }

        public static IQueryable<Recipe> FilterByEquipment(this IQueryable<Recipe> recipes, List<string> equipment)
        {
            return recipes.Where(r => equipment.All(i => r.RecipeEquipments.Any(re => re.Equipment.Name == i)));
        }

        public static IQueryable<Recipe> FilterByMaxTime(this IQueryable<Recipe> recipes, int? maxTotalTime)
        {
            return recipes.Where(r => r.PreparationTime + r.CookTime <= maxTotalTime);
        }

        public static IQueryable<Recipe> FilterByMinServings(this IQueryable<Recipe> recipes, int? minNumberOfServings)
        {
            return recipes.Where(r => r.NumberOfServings >= minNumberOfServings);
        }

        public static IQueryable<Recipe> RestrictByIngredients(this IQueryable<Recipe> recipes, List<string> ingredients)
        {
            return recipes.Where(r => r.RecipeIngredients.All(ri => ingredients.Contains(ri.Ingredient.Name)));
        }

        public static IQueryable<Recipe> RestrictByEquipment(this IQueryable<Recipe> recipes, List<string> equipment)
        {
            return recipes.Where(r => r.RecipeEquipments.All(re => equipment.Contains(re.Equipment.Name)));
        }
    }
}