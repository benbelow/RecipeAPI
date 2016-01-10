using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace RecipeAPI.Authentication
{
    class UserPrincipal : IPrincipal
    {
        private readonly UserIdentity identity;

        public UserPrincipal(UserIdentity identity)
        {
            this.identity = identity;
        }

        public int UserId
        {
            get { return identity.UserID; }
        }

        public UserIdentity Identity
        {
            get { return identity; }
        }

        IIdentity IPrincipal.Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}
