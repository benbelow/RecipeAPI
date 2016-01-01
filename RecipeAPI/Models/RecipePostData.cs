using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class RecipePostData
    {
        public string name;
        public string description;
        public string mealType;
        public int prepTime;
        public int cookTime;
        public int numberOfServings;
        public string author;
        public string imageSource;
        public List<DetailedInstruction> Instructions;
        public List<DetailedRecipeIngredient> Ingredients;
        public List<DetailedEquipment> Equipment;
    }
}