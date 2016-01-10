using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Authentication
{
    public class GoogleVerificationResponse
    {
        public string aud { get; set; }
        public string iss { get; set; }
        public string email { get; set; }
        public int exp { get; set; }
        public string sub { get; set; }
    }
}