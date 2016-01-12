using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.Models
{
    public partial class StoreCupboardEquipment
    {
        public int StoreCupboardEquipmentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int EquipmentID { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual User User { get; set; }
    }
}
