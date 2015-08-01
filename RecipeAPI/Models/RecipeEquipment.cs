using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models
{
    public partial class RecipeEquipment
    {
        public int RecipeEquipmentID { get; set; }

        [Required]
        public Nullable<int> RecipeID { get; set; }

        [Required]
        public Nullable<int> EquipmentID { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
