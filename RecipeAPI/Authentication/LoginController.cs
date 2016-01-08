using System.Web.Mvc;

namespace RecipeAPI.Authentication
{
    using System.Web.Http;

    public class LoginController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ExternalLogin()
        {
            return new ChallengeResult("Google", "/swagger", this.Request);
        }
    }
}