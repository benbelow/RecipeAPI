using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Authentication
{
    public class SignInResponse
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpiry { get; set; }
        public string RefreshToken { get; set; }

        public SignInResponse(User user)
        {
            Email = user.Email;
            AccessToken = user.AccessToken;
            AccessTokenExpiry = user.AccessTokenExpiry;
            RefreshToken = user.RefreshToken;
        }
    }
}