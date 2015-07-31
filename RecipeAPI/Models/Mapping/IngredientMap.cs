using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class IngredientMap : EntityTypeConfiguration<Ingredient>
    {
        public IngredientMap()
        {
            // Primary Key
            this.HasKey(t => t.IngredientID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(60);

            // Table & Column Mappings
            this.ToTable("Ingredients");
            this.Property(t => t.IngredientID).HasColumnName("IngredientID");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
