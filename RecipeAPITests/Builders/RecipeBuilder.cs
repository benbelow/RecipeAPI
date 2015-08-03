using System.Linq;
using RecipeAPI.Models;

namespace RecipeAPITests.Builders
{
    [Builder]
    public static class RecipeBuilder
    {
        public static Builder<Recipe> New
        {
            get
            {
                return Builder<Recipe>.New
                    .With(r => r.RecipeID, () => Enumerable.Range(1,1000))
                    .With(r => r.Name, "Test Recipe")
                    .With(r => r.Description, "Test Recipe Description")
                    .With(r => r.MealType, "Main")
                    .With(r => r.PreparationTime, 10)
                    .With(r => r.CookTime, 30)
                    .With(r => r.NumberOfServings, 4)
                    .With(r => r.Author, "Test Author");
            }
        }

        public static Builder<Recipe> WithEquipment(this Builder<Recipe> builder, params Equipment[] equipments)
        {
            var recipeId = builder.Build().RecipeID;
            var recipeEquipments = equipments.Select(e => new RecipeEquipment {EquipmentID = e.EquipmentID, RecipeID = recipeId}).ToList();
            return builder.With(r => r.RecipeEquipments, recipeEquipments);
        } 

        public static Builder<Recipe> WithIngredients(this Builder<Recipe> builder, params Ingredient[] ingredients)
        {
            var recipeId = builder.Build().RecipeID;
            var recipeIngredients = ingredients.Select(i => new RecipeIngredient {IngredientID = i.IngredientID, RecipeID = recipeId}).ToList();
            return builder.With(r => r.RecipeIngredients, recipeIngredients);
        } 
    }
}
