using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using RecipeAPI.Models.Mapping;

namespace RecipeAPI.Models
{
    public partial class RecipesContext : DbContext
    {
        static RecipesContext()
        {
            Database.SetInitializer<RecipesContext>(null);
        }

        public RecipesContext()
            : base("Name=RecipesContext")
        {
        }

        public RecipesContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<DietaryRequirement> DietaryRequirements { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<IngredientDietaryRequirement> IngredientDietaryRequirements { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<RecipeEquipment> RecipeEquipments { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DietaryRequirementMap());
            modelBuilder.Configurations.Add(new EquipmentMap());
            modelBuilder.Configurations.Add(new IngredientDietaryRequirementMap());
            modelBuilder.Configurations.Add(new IngredientMap());
            modelBuilder.Configurations.Add(new InstructionMap());
            modelBuilder.Configurations.Add(new RecipeEquipmentMap());
            modelBuilder.Configurations.Add(new RecipeIngredientMap());
            modelBuilder.Configurations.Add(new RecipeMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
