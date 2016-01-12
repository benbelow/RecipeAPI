using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models
{
    public partial class StoreCupboardIngredient
    {
        public int StoreCupboardIngredientID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int IngredientID { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public virtual User User { get; set; }
    }
}
