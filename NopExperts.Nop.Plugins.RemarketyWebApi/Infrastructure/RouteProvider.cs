using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority => 0;

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            //routes.MapHttpRoute(
            //     name: "RemarketyWebApi",
            //     routeTemplate: "RemarketyWebApi/{action}",
            //     defaults: new { controller = "RemarketyWebApi" }
            //);

            //GlobalConfiguration.Configuration.EnsureInitialized();
            //GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

            //    routes.MapRoute("WidgetControllerRoutes",
            //        "PopupWidget/{action}",
            //        new { controller = "PopupWidget", action = "RenderWidgetView" },
            //        new[] { "NopExperts.Nop.Plugins.PopUpNewsletterPlugin.Controllers" }
            //    );

            //    var route = routes.MapRoute("PopupAdminControllerRoutes",
            //        "Admin/PopupNewsletterPlugin/{action}",
            //        new { controller = "PopupAdmin", action = "Configure" },
            //        new[] { "NopExperts.Nop.Plugins.PopUpNewsletterPlugin.Controllers" }
            //    );

            //    route.DataTokens.Add("area", "admin");

            //    routes.Remove(route);
            //    routes.Insert(0, route);

            GlobalConfiguration.Configure(x => x.MapHttpAttributeRoutes());
        }
    }
}