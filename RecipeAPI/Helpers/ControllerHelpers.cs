using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace RecipeAPI.Helpers
{
    public static class ControllerHelpers
    {
        public static string StringProperty(this Dictionary<string, object> properties, string propertyName)
        {
            if (!properties.ContainsKey(propertyName))
            {
                return null;
            }
            var ret = properties[propertyName];
            if (ret == null || ret is string)
            {
                return ret as string;
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }


        public static string RequiredStringProperty(this Dictionary<string, object> properties, string propertyName)
        {
            if (properties.ContainsKey(propertyName))
            {
                return properties.StringProperty(propertyName);
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
    }
}