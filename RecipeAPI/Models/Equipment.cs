using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    public partial class Equipment
    {
        public Equipment()
        {
            this.RecipeEquipments = new List<RecipeEquipment>();
        }

        public int EquipmentID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RecipeEquipment> RecipeEquipments { get; set; }
    }
}
