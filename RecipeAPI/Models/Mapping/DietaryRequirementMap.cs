using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class DietaryRequirementMap : EntityTypeConfiguration<DietaryRequirement>
    {
        public DietaryRequirementMap()
        {
            // Primary Key
            this.HasKey(t => t.DietaryRequirementID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("DietaryRequirements");
            this.Property(t => t.DietaryRequirementID).HasColumnName("DietaryRequirementID");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
