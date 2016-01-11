using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using RecipeAPI.Repositories;
using RecipeAPI.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RecipeAPI.Authentication;
using System.IO;
using RestSharp;
using RecipeAPI.Helpers;

namespace RecipeAPI.Controllers
{
    [Authorize]
    public class AuthenticationController : ApiController
    {
        private IUserRepository UserRepo { get; set; }

        public AuthenticationController(IUserRepository userRepo)
        {
            UserRepo = userRepo;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/auth/googleLogin")]
        public HttpResponseMessage GoogleTokenSignIn(string googleIdToken)
        {
            var baseRequestUrl = "/oauth2/v3/tokeninfo?id_token=";
            var requestUrl = baseRequestUrl + googleIdToken;

            RestClient client = new RestClient("https://www.googleapis.com");
            RestRequest request = new RestRequest("/oauth2/v3/tokeninfo");
            request.AddParameter("id_token", googleIdToken);

            var response = client.Execute<GoogleVerificationResponse>(request);
            GoogleVerificationResponse googleResponse = response.Data;

            if (!GoogleTokenValid(googleResponse))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var user = UserRepo.GetUserByEmail(googleResponse.email);
            if (user == null)
            {
                user = new User()
                {
                    Email = googleResponse.email,
                    AccessToken = TokenGenerator.GenerateSecureRandomToken(),
                    AccessTokenExpiry = DateTime.UtcNow.AddDays(1),
                    // Generating the refresh token in this way isn't currently much more secure than 
                    // having a long lasting access token, but it's easily extensible
                    // at a time when I care more about security
                    RefreshToken = TokenGenerator.GenerateSecureRandomToken()
                };
                UserRepo.Add(user);
                UserRepo.SaveContext();
            }

            return Request.CreateResponse(HttpStatusCode.OK, new SignInResponse(user));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/auth/refresh")]
        public HttpResponseMessage Refresh(Dictionary<string, object> refreshProperties)
        {
            var refresh_token = refreshProperties.RequiredStringProperty("RefreshToken");
            var user = UserRepo.GetUserByRefreshToken(refresh_token);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            user.AccessToken = TokenGenerator.GenerateSecureRandomToken();
            user.AccessTokenExpiry = DateTime.UtcNow.AddDays(1);
            UserRepo.SaveContext();

            return Request.CreateResponse(HttpStatusCode.OK, new AccessTokenResponse(user));
        }

        private bool GoogleTokenValid(GoogleVerificationResponse googleResponse)
        {
            return googleResponse.iss == "https://accounts.google.com"
                && UnixTimeConverter.FromUnixTime(googleResponse.exp) > DateTime.UtcNow
                && googleResponse.aud.Contains("238308584955-f5megnjst1pq4a6s0n6iq4o2dm2biv47.apps.googleusercontent.com");
        }

    }
}
