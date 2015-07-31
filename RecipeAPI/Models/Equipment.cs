using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeAPI.Models
{
    using System.Data;

    public partial class Equipment
    {
        public Equipment()
        {
            this.RecipeEquipments = new List<RecipeEquipment>();
        }

        public int EquipmentID { get; set; }

        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<RecipeEquipment> RecipeEquipments { get; set; }
    }
}
