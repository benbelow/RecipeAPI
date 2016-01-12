using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RecipeAPI.Models;
using RecipeAPI.Repositories;
using RecipeAPITests.TestHelpers.Attributes;
using RecipeAPITests.TestHelpers.Database;

namespace RecipeAPITests.Repositories
{
    [TestFixture]
    [RequiresDatabase]
    public class IngredientRepositoryTests
    {
        private IngredientRepository repo;

        [SetUp]
        public void SetUp()
        {
            repo = new IngredientRepository(new TestRecipesContext(TestDb.ConnectionString));
        }

        [Test]
        public void IsImplementationOfBaseRepository()
        {
            repo.Should().BeAssignableTo<Repository<Ingredient>>();
        }

        [Test]
        public void CanGetData()
        {
            var ingredient = new Ingredient { Name = "test" };
            TestDb.Seed(ingredient);

            var ingredients = repo.GetAll();
            ingredients.First().Name.Should().Be(ingredient.Name);
        }

        [Test]
        public void CanGetIngredientByName()
        {
            var ingredient = new Ingredient { Name = "test" };
            TestDb.Seed(ingredient);

            var result = repo.GetIngredientByName("test");
            result.Should().NotBeNull();
        }

        [Test]
        public void GetIngredientByNameIsCaseInsensitive()
        {
            var ingredient = new Ingredient { Name = "TEST" };
            TestDb.Seed(ingredient);

            var result = repo.GetIngredientByName("test");
            result.Should().NotBeNull();
        } 
    }
}