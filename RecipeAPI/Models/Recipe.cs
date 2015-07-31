using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            this.Instructions = new List<Instruction>();
            this.RecipeEquipments = new List<RecipeEquipment>();
            this.RecipeIngredients = new List<RecipeIngredient>();
        }

        public int RecipeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MealType { get; set; }
        public int PreparationTime { get; set; }
        public int CookTime { get; set; }
        public int NumberOfServings { get; set; }
        public string Author { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<RecipeEquipment> RecipeEquipments { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
