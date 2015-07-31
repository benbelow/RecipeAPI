using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class InstructionMap : EntityTypeConfiguration<Instruction>
    {
        public InstructionMap()
        {
            // Primary Key
            this.HasKey(t => t.InstructionID);

            // Properties
            this.Property(t => t.StepDescription)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Instructions");
            this.Property(t => t.InstructionID).HasColumnName("InstructionID");
            this.Property(t => t.RecipeID).HasColumnName("RecipeID");
            this.Property(t => t.StepNumber).HasColumnName("StepNumber");
            this.Property(t => t.StepDescription).HasColumnName("StepDescription");

            // Relationships
            this.HasOptional(t => t.Recipe)
                .WithMany(t => t.Instructions)
                .HasForeignKey(d => d.RecipeID);

        }
    }
}
