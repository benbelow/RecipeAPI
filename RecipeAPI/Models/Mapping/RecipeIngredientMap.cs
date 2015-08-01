using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class RecipeIngredientMap : EntityTypeConfiguration<RecipeIngredient>
    {
        public RecipeIngredientMap()
        {
            // Primary Key
            this.HasKey(t => t.RecipeIngredientID);

            // Properties
            this.Property(t => t.Units)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("RecipeIngredients");
            this.Property(t => t.RecipeIngredientID).HasColumnName("RecipeIngredientID");
            this.Property(t => t.RecipeID).HasColumnName("RecipeID");
            this.Property(t => t.IngredientID).HasColumnName("IngredientID");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Units).HasColumnName("Units");

            // Relationships
            this.HasRequired(t => t.Ingredient)
                .WithMany(t => t.RecipeIngredients)
                .HasForeignKey(d => d.IngredientID);
            this.HasRequired(t => t.Recipe)
                .WithMany(t => t.RecipeIngredients)
                .HasForeignKey(d => d.RecipeID);

        }
    }
}
