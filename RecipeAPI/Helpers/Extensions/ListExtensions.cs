using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI.Helpers.Extensions
{
    public static class ListExtensions
    {
        public static bool Contains(this List<string> source, string toCheck, StringComparison comp)
        {
            return source.Any(x => x.Equals(toCheck, comp));
        }
    }
}