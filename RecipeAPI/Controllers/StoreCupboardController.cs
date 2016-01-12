using RecipeAPI.Authentication;
using RecipeAPI.Helpers.Comparers;
using RecipeAPI.Models;
using RecipeAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RecipeAPI.Controllers
{
    public class StoreCupboardController : ApiController
    {
        public StoreCupboardController(
            IStoreCupboardIngredientRepository storeCupboardIngredientRepo, 
            IStoreCupboardEquipmentRepository storeCupboardEquipmentRepo,
            IUserRepository userRepository,
            IIngredientRepository ingredientRepository,
            IEquipmentRepository equipmentRepository
            )
        {
            StoreCupboardIngredientRepo = storeCupboardIngredientRepo;
            UserRepository = userRepository;
            IngredientRepository = ingredientRepository;
            EquipmentRepository = equipmentRepository;
        }

        private IStoreCupboardIngredientRepository StoreCupboardIngredientRepo { get; set; }

        private IStoreCupboardEquipmentRepository StoreCupboardEquipmentRepo { get; set; }

        private IUserRepository UserRepository { get; set; }

        private IIngredientRepository IngredientRepository { get; set; }

        private IEquipmentRepository EquipmentRepository { get; set; }

        [HttpGet]
        public HttpResponseMessage GetStoreCupboard()
        {
            var principal = RequestContext.Principal;
            if (!(principal is UserPrincipal))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var userId = ((UserPrincipal)principal).UserId;

            var user = UserRepository.GetUserById(userId);

            var ingredients = user.StoreCupboardIngredients.Select(i => new DetailedIngredient(i.Ingredient)).ToList();
            var equipments = user.StoreCupboardEquipments.Select(i => new DetailedEquipment(i.Equipment)).ToList();

            var storeCupboard = new DetailedStoreCupboard(ingredients, equipments);

            return Request.CreateResponse(HttpStatusCode.OK, storeCupboard);
        }

        [HttpPost]
        [Route("api/StoreCupboard/ingredients")]
        public HttpResponseMessage PostStoreCupboardIngredients([FromBody] List<string> ingredientNames)
        {
            var principal = RequestContext.Principal;
            if (!(principal is UserPrincipal))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var userId = ((UserPrincipal)principal).UserId;
            var user = UserRepository.GetUserById(userId);

            var ingredientIds = ingredientNames.Select(n => IngredientRepository.GetOrCreateIngredient(n).IngredientID);

            var newSCIs = ingredientIds.Select(i => new StoreCupboardIngredient { IngredientID = i, UserID = userId }).ToList();
            var oldSCIs = user.StoreCupboardIngredients;

            foreach (var sci in oldSCIs.Except(newSCIs, new StoreCupboardIngredientComparer()).ToList())
            {
                StoreCupboardIngredientRepo.Remove(sci);
            }

            foreach (var sci in newSCIs.Except(oldSCIs, new StoreCupboardIngredientComparer()))
            {
                user.StoreCupboardIngredients.Add(sci);
            }

            StoreCupboardIngredientRepo.SaveContext();

            return GetStoreCupboard();
        }

        [HttpPost]
        [Route("api/StoreCupboard/equipments")]
        public HttpResponseMessage PostStoreCupboardEquipments([FromBody] List<string> equipmentNames)
        {
            var principal = RequestContext.Principal;
            if (!(principal is UserPrincipal))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var userId = ((UserPrincipal)principal).UserId;
            var user = UserRepository.GetUserById(userId);

            var equipmentIds = equipmentNames.Select(n => EquipmentRepository.GetOrCreateEquipment(n).EquipmentID);

            var newSCEs = equipmentIds.Select(i => new StoreCupboardEquipment { EquipmentID = i, UserID = userId }).ToList();
            var oldSCEs = user.StoreCupboardEquipments;

            foreach (var sce in oldSCEs.Except(newSCEs, new StoreCupboardEquipmentComparer()).ToList())
            {
                StoreCupboardEquipmentRepo.Remove(sce);
            }

            foreach (var sce in newSCEs.Except(oldSCEs, new StoreCupboardEquipmentComparer()))
            {
                user.StoreCupboardEquipments.Add(sce);
            }

            StoreCupboardIngredientRepo.SaveContext();

            return GetStoreCupboard();
        }
    }
}
