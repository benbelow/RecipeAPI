﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Models
{
    public class DetailedEquipment
    {
        public DetailedEquipment(RecipeEquipment recipeEquipment)
        {
            Name = recipeEquipment.Equipment.Name;
        }

        public String Name { get; set; }
    }
}