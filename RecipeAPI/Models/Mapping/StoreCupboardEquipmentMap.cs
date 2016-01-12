using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class StoreCupboardEquipmentMap : EntityTypeConfiguration<StoreCupboardEquipment>
    {
        public StoreCupboardEquipmentMap()
        {
            // Primary Key
            this.HasKey(t => t.StoreCupboardEquipmentID);
            
            // Table & Column Mappings
            this.ToTable("StoreCupboardEquipments");
            this.Property(t => t.StoreCupboardEquipmentID).HasColumnName("StoreCupboardEquipmentID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.EquipmentID).HasColumnName("EquipmentID");

            // Relationships
            this.HasRequired(t => t.Equipment)
                .WithMany(t => t.StoreCupboardEquipments)
                .HasForeignKey(d => d.EquipmentID);
            this.HasRequired(t => t.User)
                .WithMany(t => t.StoreCupboardEquipments)
                .HasForeignKey(d => d.UserID);

        }
    }
}
