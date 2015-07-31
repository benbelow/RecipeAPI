using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            this.IngredientDietaryRequirements = new List<IngredientDietaryRequirement>();
            this.RecipeIngredients = new List<RecipeIngredient>();
        }

        public int IngredientID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<IngredientDietaryRequirement> IngredientDietaryRequirements { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
