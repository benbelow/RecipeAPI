using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class RecipeIngredient
    {
        public int RecipeIngredientID { get; set; }
        public Nullable<int> RecipeID { get; set; }
        public Nullable<int> IngredientID { get; set; }
        public int Amount { get; set; }
        public string Units { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
