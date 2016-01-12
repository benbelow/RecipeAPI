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
            IUserRepository userRepository,
            IIngredientRepository ingredientRepository
            )
        {
            StoreCupboardIngredientRepo = storeCupboardIngredientRepo;
            UserRepository = userRepository;
            IngredientRepository = ingredientRepository;
        }

        private IStoreCupboardIngredientRepository StoreCupboardIngredientRepo { get; set; }

        private IUserRepository UserRepository { get; set; }

        private IIngredientRepository IngredientRepository { get; set; }

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
            var equipments = new List<DetailedEquipment>();

            var storeCupboard = new DetailedStoreCupboard(ingredients, equipments);

            return Request.CreateResponse(HttpStatusCode.OK, storeCupboard);
        }

        [HttpPost]
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
    }
}
