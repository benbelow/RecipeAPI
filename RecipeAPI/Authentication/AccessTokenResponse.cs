using RecipeAPI.Helpers;
using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Authentication
{
    public class AccessTokenResponse
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public double AccessTokenExpiry { get; set; }

        public AccessTokenResponse(User user)
        {
            Email = user.Email;
            AccessToken = user.AccessToken;
            AccessTokenExpiry = UnixTimeConverter.ToUnixTime((DateTime) user.AccessTokenExpiry);
        }
    }
}