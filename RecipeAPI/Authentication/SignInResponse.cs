using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Authentication
{
    public class SignInResponse : AccessTokenResponse
    {
        public string RefreshToken { get; set; }

        public SignInResponse(User user) : base(user)
        {
            RefreshToken = user.RefreshToken;
        }
    }
}