using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Helpers
{
    public static class UnixTimeConverter
    {
        public static DateTime FromUnixTime(int unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}