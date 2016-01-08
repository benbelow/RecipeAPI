using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.App_Start
{
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Google;
    using Owin;

    public class SecurityConfig
	{
        public static void Configure(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ExternalCookie
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure google authentication
            var options = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "238308584955-f5megnjst1pq4a6s0n6iq4o2dm2biv47.apps.googleusercontent.com",
                ClientSecret = "cYYJYwiKqufCIp5IVwfLI9GI"
            };

            app.UseGoogleAuthentication(options);
        }
    }
}