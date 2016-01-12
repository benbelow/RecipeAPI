using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class StoreCupboardIngredientMap : EntityTypeConfiguration<StoreCupboardIngredient>
    {
        public StoreCupboardIngredientMap()
        {
            // Primary Key
            this.HasKey(t => t.StoreCupboardIngredientID);
            
            // Table & Column Mappings
            this.ToTable("StoreCupboardIngredients");
            this.Property(t => t.StoreCupboardIngredientID).HasColumnName("StoreCupboardIngredientID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.IngredientID).HasColumnName("IngredientID");

            // Relationships
            this.HasRequired(t => t.Ingredient)
                .WithMany(t => t.StoreCupboardIngredients)
                .HasForeignKey(d => d.IngredientID);
            this.HasRequired(t => t.User)
                .WithMany(t => t.StoreCupboardIngredients)
                .HasForeignKey(d => d.UserID);

        }
    }
}
