using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RecipeAPI.Repositories;
using RecipeAPITests.Builders;
using RecipeAPITests.TestHelpers.Attributes;
using RecipeAPITests.TestHelpers.Database;

namespace RecipeAPITests.Repositories
{
    [TestFixture]
    [RequiresDatabase]
    public class RecipeRepositoryTests
    {
        private IRecipeRepository repo;

        [SetUp]
        public void SetUp()
        {
            repo = new RecipeRepository(new TestRecipesContext(TestDb.ConnectionString));
        }

        [Test]
        public void ReturnsRecipes()
        {
            var recipe = RecipeBuilder.New.Build();
            TestDb.Seed(recipe);

            var results = repo.GetAll().ToList();

            results.First().Name.Should().Be(recipe.Name);
        }

        [TestCase("Apple Pie", "", true)]               // Matches when search term empty
        [TestCase("Apple Pie", "Apple Pie", true)]      // Matches multiple words
        [TestCase("Apple Pie", "apple Pie", true)]      // Case insensitive
        [TestCase("Apple Pie", "Pie Apple", true)]      // Order does not matter
        [TestCase("Apple Pie", "apple", true)]          // Matches single word
        [TestCase("Apple Pie", "pear tart", false)]     // All search terms must be present 
        [TestCase("Apple Pie", "haddock", false)]       // Does not match absent word
        [TestCase("Apple,. ?Pie!", "apple pie", true)]  // Ignores punctuation
        public void CanFilterRecipesByName(string recipeName, string searchTerm, bool expectedMatch)
        {
            var recipe = RecipeBuilder.New.With(r => r.Name, recipeName).Build();
            TestDb.Seed(recipe);

            var results = repo.FilterRecipes(name: searchTerm);

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase("Main Course", "Main Course", true)]
        [TestCase("Main Course", "main coursE", true)]
        [TestCase("Main Course", "Main", false)]
        [TestCase("Main Course", "", true)]
        public void CanFilterByMealType(string mealType, string searchTerm, bool expectedMatch)
        {
            var recipe = RecipeBuilder.New.With(r => r.MealType, mealType).Build();
            TestDb.Seed(recipe);


            var all = repo.GetAll();

            var results = repo.FilterRecipes(mealType: searchTerm);

            results.Any().Should().Be(expectedMatch);
        }
    }
}