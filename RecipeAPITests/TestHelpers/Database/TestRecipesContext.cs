using System.Data.Entity;
using RecipeAPI.Models;

namespace RecipeAPITests.TestHelpers.Database
{
    public class TestRecipesContext : RecipesContext
    {
        public TestRecipesContext() { }

        public TestRecipesContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}
