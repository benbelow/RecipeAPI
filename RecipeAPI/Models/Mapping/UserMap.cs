using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RecipeAPI.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserID);


            this.Property(t => t.Email)
                .HasMaxLength(450);

            // Table & Column Mappings
            this.ToTable("Users");
        }
    }
}
