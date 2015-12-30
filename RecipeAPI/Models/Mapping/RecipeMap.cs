using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class RecipeMap : EntityTypeConfiguration<Recipe>
    {
        public RecipeMap()
        {
            // Primary Key
            this.HasKey(t => t.RecipeID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.MealType)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Author)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Recipes");
            this.Property(t => t.RecipeID).HasColumnName("RecipeID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.MealType).HasColumnName("MealType");
            this.Property(t => t.PreparationTime).HasColumnName("PreparationTime");
            this.Property(t => t.CookTime).HasColumnName("CookTime");
            this.Property(t => t.NumberOfServings).HasColumnName("NumberOfServings");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.ImageSource).IsOptional();
        }
    }
}
