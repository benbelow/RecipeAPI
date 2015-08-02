using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using RecipeAPI.Controllers;
using RecipeAPI.Models;
using RecipeAPI.Repositories;

namespace RecipeAPITests.Controllers
{
    using NUnit.Framework;

    [TestFixture]
    public class EquipmentControllerTests
    {
        private EquipmentController controller;
        private IEquipmentRepository fakeEquipmentRepo;
        private List<Equipment> equipments = new List<Equipment>();

        [SetUp]
        public void Setup()
        {
            fakeEquipmentRepo = A.Fake<IEquipmentRepository>();
            A.CallTo(() => fakeEquipmentRepo.GetAll()).Returns(equipments);
            controller = new EquipmentController(fakeEquipmentRepo);
        }

        [Test]
        public void TemporaryTest()
        {
            var name = "name";
            var equipment = A.Dummy<Equipment>();
            equipment.Name = name;
            equipments.Add(equipment);

            var response = controller.Get();

            response.FirstOrDefault().Name.Should().Be(name);

        }

    }
}