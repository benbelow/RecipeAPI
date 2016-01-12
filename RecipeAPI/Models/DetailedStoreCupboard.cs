using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class DetailedStoreCupboard
    {
        public DetailedStoreCupboard() { }

        public DetailedStoreCupboard(List<DetailedIngredient> ingredients, List<DetailedEquipment> equipments)
        {
            this.ingredients = ingredients;
            this.equipments = equipments;
        }

        public List<DetailedIngredient> ingredients { get; set; }
        public List<DetailedEquipment> equipments { get; set; }
    }
}