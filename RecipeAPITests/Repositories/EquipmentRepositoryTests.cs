using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FakeItEasy;
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
    public class EquipmentRepositoryTests
    {
        private EquipmentRepository repo;

        [SetUp]
        public void SetUp()
        {
            repo = new EquipmentRepository(new TestRecipesContext(TestDb.ConnectionString));
        }

        [Test]
        public void IsImplementationOfBaseRepository()
        {
            repo.Should().BeAssignableTo<Repository<Equipment>>();
        }

        [Test]
        public void CanGetData()
        {
            var equipment = new Equipment {Name = "test"};
            TestDb.Seed(equipment);

            var equipments = repo.GetAll();
            equipments.First().Name.Should().Be(equipment.Name);
        }

        [Test]
        public void CanGetEquipmentByName()
        {
            var equipment = new Equipment { Name = "test" };
            TestDb.Seed(equipment);

            var result = repo.GetEquipmentByName("test");
            result.Should().NotBeNull();
        }

        [Test]
        public void GetEquipmentByNameIsCaseInsensitive()
        {
            var equipment = new Equipment { Name = "TEST" };
            TestDb.Seed(equipment);

            var result = repo.GetEquipmentByName("test");
            result.Should().NotBeNull();
        }
    }
}