using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace RecipeAPI.Helpers
{
    public static class TokenGenerator
    {
        public static string GenerateSecureRandomToken()
        {
            var bytes = new byte[32];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}