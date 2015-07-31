using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class IngredientDietaryRequirement
    {
        public int IngredientDietaryRequirementID { get; set; }
        public Nullable<int> IngredientID { get; set; }
        public Nullable<int> DietaryRequirementID { get; set; }
        public virtual DietaryRequirement DietaryRequirement { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
