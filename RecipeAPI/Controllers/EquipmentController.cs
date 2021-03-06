﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RecipeAPI.Repositories;
using RecipeAPI.Models;

namespace RecipeAPI.Controllers
{
    public class EquipmentController : ApiController
    {
        private IEquipmentRepository EquipmentRepo { get; set; }

        public EquipmentController(IEquipmentRepository equipmentRepo)
        {
            EquipmentRepo = equipmentRepo;
        }

        // GET api/ingredients
        public IEnumerable<DetailedEquipment> Get()
        {
            return EquipmentRepo.GetAll().Select(e => new DetailedEquipment(e));
        }

        [HttpPost]
        public HttpResponseMessage PostEquipment(string name)
        {
            var equipment = new Equipment { Name = name };
            EquipmentRepo.Add(equipment);
            EquipmentRepo.SaveContext();

            var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
