using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json.Converters;
using Nop.Web.Framework.Mvc.Routes;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority => 0;

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
            
            var route = routes.MapRoute("RemarketyWebAdmin",
                "Admin/RemarketyWebAdmin/{action}",
                new { controller = "RemarketyWebAdmin" },
                new[] { "NopExperts.Nop.Plugins.RemarketyWebApi.Controllers" }
            );

            route.DataTokens.Add("area", "admin");

            routes.Remove(route);
            routes.Insert(0, route);

            routes.MapRoute("RemarketyWidget",
               "RemarketyWidget/{action}",
               new { controller = "RemarketyWidget" },
               new[] { "NopExperts.Nop.Plugins.RemarketyWebApi.Controllers" }
           );
        }
    }
}