using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class DetailedIngredient
    {
        public DetailedIngredient(Ingredient ingredient)
        {
            Name = ingredient.Name;
        }

        public String Name { get; set; }
    }
}