using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class RecipeEquipmentMap : EntityTypeConfiguration<RecipeEquipment>
    {
        public RecipeEquipmentMap()
        {
            // Primary Key
            this.HasKey(t => t.RecipeEquipmentID);

            // Properties
            // Table & Column Mappings
            this.ToTable("RecipeEquipments");
            this.Property(t => t.RecipeEquipmentID).HasColumnName("RecipeEquipmentID");
            this.Property(t => t.RecipeID).HasColumnName("RecipeID");
            this.Property(t => t.EquipmentID).HasColumnName("EquipmentID");

            // Relationships
            this.HasRequired(t => t.Equipment)
                .WithMany(t => t.RecipeEquipments)
                .HasForeignKey(d => d.EquipmentID);
            this.HasRequired(t => t.Recipe)
                .WithMany(t => t.RecipeEquipments)
                .HasForeignKey(d => d.RecipeID);

        }
    }
}
