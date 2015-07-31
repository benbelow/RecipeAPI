using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class RecipePostData
    {
        public List<DetailedInstruction> Instructions;
        public List<DetailedRecipeIngredient> Ingredients;
        public List<DetailedEquipment> Equipment;
    }
}