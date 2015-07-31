using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class RecipeEquipment
    {
        public int RecipeEquipmentID { get; set; }
        public Nullable<int> RecipeID { get; set; }
        public Nullable<int> EquipmentID { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
