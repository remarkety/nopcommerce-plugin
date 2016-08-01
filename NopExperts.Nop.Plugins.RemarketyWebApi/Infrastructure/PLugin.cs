using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Web.Framework.Menu;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class Plugin : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;

        public Plugin(IStoreContext storeContext, IGenericAttributeService genericAttributeService)
        {
            _storeContext = storeContext;
            _genericAttributeService = genericAttributeService;
        }

        public override void Install()
        {
            var currentStoreApiKey = Guid.NewGuid();

            _genericAttributeService.SaveAttribute(_storeContext.CurrentStore, StringHelper.RemarketyApiKey, currentStoreApiKey);

            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.RemarketyWebApiKey", "Remarkety WebApi Key");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.RemarketyWebApiKey.Hint", "Remarkety WebApi Key");

            base.Install();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var configNode = new SiteMapNode
            {
                Title = "RemarketyWebApi plugin",
                Visible = true,
                ActionName = "Configure",
                ControllerName = "RemarketyWebAdmin",
            };


            var node = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins") ?? rootNode;


            var nopExpertsNode = node.ChildNodes.FirstOrDefault(x => x.SystemName == "NopExperts");

            if (nopExpertsNode == null)
            {
                nopExpertsNode = new SiteMapNode
                {
                    Visible = true,
                    Title = "NopExperts",
                    SystemName = "NopExperts"
                };

                node.ChildNodes.Add(nopExpertsNode);
            }

            nopExpertsNode.ChildNodes.Add(configNode);
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string> { "productdetails_bottom", "body_end_html_tag_before" };
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = String.Empty;
            controllerName = String.Empty;
            routeValues = new RouteValueDictionary { { "Namespaces", "NopExperts.Nop.Plugins.RemarketyWebApi.Controllers" }, { "area", "Admin" } };
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            routeValues = new RouteValueDictionary {
                { "Namespaces", "NopExperts.Nop.Plugins.RemarketyWebApi.Controllers" },
                { "area", null } ,
                {"widgetZone", widgetZone}
            };

            actionName = String.Empty;
            controllerName = "RemarketyWidget";

            switch (widgetZone)
            {
                case "productdetails_bottom":
                    {
                        actionName = "GetProductDetailsRemarketyWebTracking";
                        break;
                    }
                case "body_end_html_tag_before":
                    {
                        actionName = "GetStoreRemarketyWebTracking";
                        break;
                    }
            }

        }
    }
}