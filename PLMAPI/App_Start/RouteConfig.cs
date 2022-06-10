using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PLMAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            //routes.MapRoute(
            //    name: "ApiV1",
            //    //url: "v1/{controller}/{action}",
            //    url: "v1/{controller}",
            //    namespaces: new string[] { "PLMAPI.Controllers.v1" }
            //);
            //routes.MapRoute(
            //    name: "ApiV2",
            //    url: "v2/{controller}/{action}",
            //    namespaces: new string[] { "PLMAPI.Controllers.v2" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
