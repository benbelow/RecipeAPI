using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string ImageSource { get; set; }

        [Required]
        public virtual ICollection<Instruction> Instructions { get; set; }
        [Required]
        public virtual ICollection<RecipeEquipment> RecipeEquipments { get; set; }
        [Required]
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
