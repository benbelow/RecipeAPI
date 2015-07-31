using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class IngredientDietaryRequirementMap : EntityTypeConfiguration<IngredientDietaryRequirement>
    {
        public IngredientDietaryRequirementMap()
        {
            // Primary Key
            this.HasKey(t => t.IngredientDietaryRequirementID);

            // Properties
            // Table & Column Mappings
            this.ToTable("IngredientDietaryRequirements");
            this.Property(t => t.IngredientDietaryRequirementID).HasColumnName("IngredientDietaryRequirementID");
            this.Property(t => t.IngredientID).HasColumnName("IngredientID");
            this.Property(t => t.DietaryRequirementID).HasColumnName("DietaryRequirementID");

            // Relationships
            this.HasOptional(t => t.DietaryRequirement)
                .WithMany(t => t.IngredientDietaryRequirements)
                .HasForeignKey(d => d.DietaryRequirementID);
            this.HasOptional(t => t.Ingredient)
                .WithMany(t => t.IngredientDietaryRequirements)
                .HasForeignKey(d => d.IngredientID);

        }
    }
}
