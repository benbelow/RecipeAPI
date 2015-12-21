using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RecipeAPI.Models;
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
        private IRecipeRepository recipeRepo;

        [SetUp]
        public void SetUp()
        {
            recipeRepo = new RecipeRepository(new TestRecipesContext(TestDb.ConnectionString));
        }

        [Test]
        public void ReturnsRecipes()
        {
            var recipe = RecipeBuilder.New.Build();
            TestDb.Seed(recipe);

            var results = recipeRepo.GetAll().ToList();

            results.First().Name.Should().Be(recipe.Name);
        }

        [Test]
        public void FilterReturnsAllRecipesWhenNoFiltersSpecified()
        {
            const int numberOfRecipes = 10;
            SeedMultipleRecipes(numberOfRecipes);

            var results = recipeRepo.FilterRecipes();

            results.Count().Should().Be(numberOfRecipes);
        }

        [TestCase("Apple Pie", "", true)]                // Matches when search term empty
        [TestCase("Apple Pie", "Apple Pie", true)]       // Matches multiple words
        [TestCase("Apple Pie", "apple Pie", true)]       // Case insensitive
        [TestCase("Apple Pie", "Pie Apple", true)]       // Order does not matter
        [TestCase("Apple Pie", "apple", true)]           // Matches single word
        [TestCase("Apple Pie", "pear tart", false)]      // All search terms must be present 
        [TestCase("Apple Pie", "haddock", false)]        // Does not match absent word
        [TestCase("Apple Pie", "Appl", true)]            // Matches substring of word
        [TestCase("Apple,. ?Pie!", "apple pie", true)]   // Ignores punctuation
        public void CanFilterRecipesByName(string recipeName, string searchTerm, bool expectedMatch)
        {
            var recipe = RecipeBuilder.New.With(r => r.Name, recipeName).Build();
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(name: searchTerm);

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase("Main Course", "Main Course", true)]    // Matches mealtype
        [TestCase("Main Course", "main coursE", true)]    // Case Insensitive
        [TestCase("Main Course", "Mai", false)]           // Matches substring
        [TestCase("Main Course", "", true)]               // Matches when search term empty
        public void CanFilterByMealType(string mealType, string searchTerm, bool expectedMatch)
        {
            var recipe = RecipeBuilder.New.With(r => r.MealType, mealType).Build();
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(mealType: searchTerm);

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase(new []{"one"}, true)]                    // Matches single ingredient
        [TestCase(new []{"on"}, false)]                    // Does not match substring of ingredient
        [TestCase(new []{"ONE"}, true)]                    // Case insensitive
        [TestCase(new []{"two","one"}, true)]              // Order does not matter
        [TestCase(new []{"one", "two", "three"}, true)]    // Matches all ingredients
        [TestCase(new []{"invalid"}, false)]               // Does not match if no ingredients in recipe
        [TestCase(new []{"one", "two", "invalid"}, false)] // Does not match if any ingredient not in recipe
        [TestCase(new string[]{}, true)]                   // Matches empty list 
        public void FilterMustMatchAllIngredientsIngredientsAllList(string[] ingredientsAll, bool expectedMatch)
        {
            var ingredients = CreateIngredientList(new List<string> {"one", "two", "three"});
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(ingredientsAll: ingredientsAll.ToList());

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase(new []{"one"}, true)]                    // Matches single ingredient
        [TestCase(new []{"on"}, false)]                    // Does not match substring of ingredient
        [TestCase(new []{"ONE"}, true)]                    // Case insensitive
        [TestCase(new []{"one", "two", "three"}, true)]    // Matches all ingredients
        [TestCase(new []{"invalid"}, false)]               // Does not match if no ingredients in recipe
        [TestCase(new []{"one", "four", "invalid"}, true)] // Matches if at least one ingredient in recipe
        [TestCase(new string[]{}, true)]                   // Matches empty list 
        public void FilterMustMatchAtLeastOneIngredientInIngredientsAny(string[] ingredientsAny, bool expectedMatch)
        {
            var ingredients = CreateIngredientList(new List<string> {"one", "two", "three"});
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(ingredientsAny: ingredientsAny.ToList());

            results.Any().Should().Be(expectedMatch);
        }

        [Test]
        public void FilterMatchesWhenNoIngredientInIngredientsAnyMatchButAllIngredientsInIngredientsAllMatch()
        {
            var ingredients = CreateIngredientList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(ingredientsAny: new List<string>{"four"}, ingredientsAll: new List<string>{"one","two"});

            results.Any().Should().BeTrue();
        }

        [TestCase(new[] { "one" }, true)]                    // Matches single equipment
        [TestCase(new[] { "on" }, false)]                    // Does not match substring of equipment
        [TestCase(new[] { "ONE" }, true)]                    // Case insensitive
        [TestCase(new[] { "one", "two", "three" }, true)]    // Matches all equipment
        [TestCase(new[] { "invalid" }, false)]               // Does not match if no equipment in recipe
        [TestCase(new[] { "one", "four", "invalid" }, true)] // Matches if at least one equipment in recipe
        [TestCase(new string[] { }, true)]                   // Matches empty list 
        public void FilterMatchesAnyEquipmentInEquipmentList(string[] equipment, bool expectedMatch)
        {
            var recipeEquipment = CreateEquipmentList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithEquipment(recipeEquipment.ToArray()).Build();
            TestDb.Seed(recipeEquipment.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(equipment: equipment.ToList());

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase(0, false)]
        [TestCase(10, false)]
        [TestCase(40, true)]
        [TestCase(50, true)]
        [TestCase(-10, false)]
        public void CanFilterByMaxTotalTime(int maxTotalTime, bool expectedMatch)
        {
            var recipe = RecipeBuilder.New.With(r => r.CookTime, 10).With(r => r.PreparationTime, 30).Build();
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(maxTotalTime: maxTotalTime);

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(4, true)]
        [TestCase(5, false)]
        [TestCase(-10, true)]
        public void CanFilterByMinNumberOfServings(int minNumberOfServings, bool expectedMatch)
        {
            var recipe = RecipeBuilder.New.With(r => r.NumberOfServings, 4).Build();
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterRecipes(minNumberOfServings: minNumberOfServings);

            results.Any().Should().Be(expectedMatch);
        }

        [Test]
        public void CanLimitNumberOfFilterResults()
        {
            SeedMultipleRecipes(20);

            var results = recipeRepo.FilterRecipes(limit: 10);

            results.Count().Should().Be(10);
        }

        [TestCase(new[] { "one", "two" }, false)]                  // Does not match if not all ingredients in recipe
        [TestCase(new[] { "ONE", "TWO", "Three" }, true)]          // Case insensitive
        [TestCase(new[] { "one", "three", "two" }, true)]          // Matches all ingredients
        [TestCase(new[] { "one", "three", "two", "four" }, true)]  // Not all given ingredients must be in recipe
        [TestCase(new string[] { }, false)]                        // Does not match empty list 
        public void FilterWithWhatIHaveOnlyMatchesRecipesWithAllIngredientsInOwnedIngredients(string[] ownedIngredients, bool expectedMatch)
        {
            var ingredients = CreateIngredientList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterWithWhatIHave(ownedIngredients: ownedIngredients.ToList());

            results.Any().Should().Be(expectedMatch);
        }

        [TestCase(new[] { "one", "two" }, false)]                  // Does not match if not all ingredients in recipe
        [TestCase(new[] { "ONE", "TWO", "Three" }, true)]          // Case insensitive
        [TestCase(new[] { "one", "three", "two" }, true)]          // Matches all ingredients
        [TestCase(new[] { "one", "three", "two", "four" }, false)] // All given ingredients must be in recipe
        [TestCase(new string[] { }, false)]                        // Does not match empty list 
        public void FilterWithWhatIHaveOnlyMatchesRecipesWithAllRequiredIngredients(string[] requiredIngredients, bool expectedMatch)
        {
            var ingredients = CreateIngredientList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterWithWhatIHave(requiredIngredients: requiredIngredients.ToList());

            results.Any().Should().Be(expectedMatch);
        }

        [Test]
        public void FilterWithWhatIHaveDoesNotMatchIfNoIngredientsGiven()
        {
            var ingredients = CreateIngredientList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterWithWhatIHave();

            results.Any().Should().BeFalse();
        }

        [Test]
        public void FilterWithWhatIHaveMatchesWhenRequiredIngredientsEmptyIfAllIngredientsAreInOwnedIngredients()
        {
            var ingredients = CreateIngredientList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterWithWhatIHave(ownedIngredients: ingredients.Select(i => i.Name).ToList());

            results.Any().Should().BeTrue();
        }

        [Test]
        public void FilterWithWhatIHaveMatchesWhenOwnedIngredientsEmptyIfAllIngredientsAreInRequiredIngredients()
        {
            var ingredients = CreateIngredientList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithIngredients(ingredients.ToArray()).Build();
            TestDb.Seed(ingredients.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterWithWhatIHave(requiredIngredients: ingredients.Select(i => i.Name).ToList());

            results.Any().Should().BeTrue();
        }

        [TestCase(new[] { "one", "two" }, false)]                  // Does not match if not all equipment in recipe
        [TestCase(new[] { "ONE", "TWO", "Three" }, true)]          // Case insensitive
        [TestCase(new[] { "one", "three", "two" }, true)]          // Matches all equipment
        [TestCase(new[] { "one", "three", "two", "four" }, true)]  // Not all given equipment must be used in recipe
        [TestCase(new string[] { }, true)]                         // Matches empty list 
        public void FilterWithWhatIHaveMatchesWhenAllEquipmentNeededInEquipmentList(string[] equipment, bool expectedMatch)
        {
            var recipeEquipment = CreateEquipmentList(new List<string> { "one", "two", "three" });
            var recipe = RecipeBuilder.New.WithEquipment(recipeEquipment.ToArray()).Build();
            TestDb.Seed(recipeEquipment.ToArray());
            TestDb.Seed(recipe);

            var results = recipeRepo.FilterWithWhatIHave(equipment: equipment.ToList());

            results.Any().Should().Be(expectedMatch);
        }

        private static List<Ingredient> CreateIngredientList(IEnumerable<string> ingredientNames)
        {
            var ingredients = new List<Ingredient>();
            var id = 1;
            foreach (var ingredientName in ingredientNames)
            {
                ingredients.Add(new Ingredient{Name = ingredientName, IngredientID = id});
                id++;
            }
            return ingredients;
        }

        private static List<Equipment> CreateEquipmentList(List<string> equipmentNames)
        {
            var equipment = new List<Equipment>();
            var id = 1;
            foreach (var equipmentName in equipmentNames)
            {
                equipment.Add(new Equipment { Name = equipmentName, EquipmentID = id });
                id++;
            }
            return equipment;
        }

        private static void SeedMultipleRecipes(int numberToSeed)
        {
            var recipes = new List<Recipe>();
            for (var i = 0; i < numberToSeed; i++)
            {
                recipes.Add(RecipeBuilder.New.Build());
            }
            TestDb.Seed(recipes.ToArray());
        }
    }
}