using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using RecipeAPI.Repositories;
using RecipeAPI.Models;

namespace RecipeAPI.Authentication
{
    public class AccessTokenMessageHandler : DelegatingHandler
    {
        private IUserRepository userRepo;

        public AccessTokenMessageHandler()
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorization = request.Headers.Where(h => h.Key == "Authorization")
                .Select(h => h.Value.SingleOrDefault()).SingleOrDefault();

            if (authorization == null)
            {
                var queryParams = request.RequestUri.ParseQueryString();

                if (queryParams.AllKeys.Contains("Authorization"))
                {
                    authorization = queryParams.Get("Authorization");
                }
            }

            if (authorization != null) { 

                if (authorization.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
                {
                    authorization = authorization.Substring("Bearer ".Length);
                }

                SetPrincipalFromAuthorizationHeader(authorization);

            }

 	        return await base.SendAsync(request, cancellationToken);
        }

        public void SetPrincipalFromAuthorizationHeader(string authorization)
        {
            SetUserPrincipal(authorization);
        }

        public void SetUserPrincipal(string authorization)
        {
            User user;

            using (var context = new RecipesContext()){
                var userRepo = new UserRepository(context);
                user = userRepo.GetUserByAccessToken(authorization);
            }

            UserIdentity identity = null;

            if (user != null && user.AccessTokenExpiry > DateTime.UtcNow)
            {
                identity = new UserIdentity(user);
            }

            if (identity != null)
            {
                SetPrincipal(new UserPrincipal(identity));
            }
        }

        public void SetPrincipal(IPrincipal principal)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            Thread.CurrentPrincipal = principal;
        }
    }
}