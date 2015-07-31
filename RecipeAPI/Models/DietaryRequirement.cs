using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class DietaryRequirement
    {
        public DietaryRequirement()
        {
            this.IngredientDietaryRequirements = new List<IngredientDietaryRequirement>();
        }

        public int DietaryRequirementID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<IngredientDietaryRequirement> IngredientDietaryRequirements { get; set; }
    }
}
