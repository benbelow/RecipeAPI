using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace RecipeAPI.Authentication
{
    public class UserIdentity : IIdentity
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public UserIdentity(User user)
        {
            UserID = user.UserID;
            Email = user.Email;
        }

        public string AuthenticationType
        {
            get { return "token"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

    }
}