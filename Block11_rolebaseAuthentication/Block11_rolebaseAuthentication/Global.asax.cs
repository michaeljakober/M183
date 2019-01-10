using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Block11_rolebaseAuthentication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string[] roles;
        public static Dictionary<string, object> userRoles;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            roles = new string[] { "Administrator", "User" };
            userRoles = new Dictionary<string, object>();
            userRoles.Add("admin", "Administrator");
            userRoles.Add("user", "User");
        }
    }
}
