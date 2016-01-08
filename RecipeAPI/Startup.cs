using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace RecipeAPI
{
    using RecipeAPI.App_Start;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            SecurityConfig.Configure(app);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(config);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}