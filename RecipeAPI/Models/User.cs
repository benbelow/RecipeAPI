using System;
using System.Collections.Generic;

namespace RecipeAPI.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class User
    {
        public User()
        {
        }

        public int UserID { get; set; }

        [Index(IsUnique = true)]
        public string Email { get; set; }

        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpiry { get; set; }
        public string RefreshToken { get; set; }
    }
}
