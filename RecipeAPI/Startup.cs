using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeAPI
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

//            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(config);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}