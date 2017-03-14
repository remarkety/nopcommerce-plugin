using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Web.Framework.Menu;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class Plugin : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;

        public Plugin(IStoreContext storeContext, IGenericAttributeService genericAttributeService, IWorkContext workContext)
        {
            _storeContext = storeContext;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
        }

        public override void Install()
        {
            var currentStoreApiKey = Guid.NewGuid();

            _genericAttributeService.SaveAttribute(_storeContext.CurrentStore, StringHelper.RemarketyApiKey, currentStoreApiKey);

            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.ApiConfig", "Api configuration");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.StoreAddress", "Store address");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig", "Discount Api");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.ApiInfo", "Information");

            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.RemarketyWebApiKey", "Remarkety WebApi Key");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.RemarketyWebApiKey.Hint", "Remarkety WebApi Key");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.StoreLogoPictureId", "Store logo");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.StoreLogoPictureId.Hint", "Store logo");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.TimeZone", "Time zone name (e.g. Africa/Abidjan)");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.TimeZone.Hint", "Time zone name (e.g. Africa/Abidjan)");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.EmailTrackingEnabled", "Website tracking enabled");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.EmailTrackingEnabled.Hint", "Website tracking enabled");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.RemarketyStoreId", "Remarkety Store ID");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.RemarketyStoreId.Hint", "Remarkety Store ID");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Country", "Country");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Country.Hint", "Country");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.State", "State");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.State.Hint", "State");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.City", "City");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.City.Hint", "City");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Address1", "Address1");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Address1.Hint", "Address1");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Address2", "Address2");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Address2.Hint", "Address2");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Zip", "Zip");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.Zip.Hint", "Zip");

            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig.Enabled", "Enabled");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig.Enabled.Hint", "Enable discount creation via API");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig.DiscountTemplateId", "Discount template id");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig.DiscountTemplateId.Hint", "Discount template id (existing base discount)");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig.DiscountNamePrefix", "Generated discount name prefix");
            this.AddOrUpdatePluginLocaleResource("NopExperts.RemarketyWebApi.Configure.DiscountConfig.DiscountNamePrefix.Hint", "Generated discount name prefix");

            base.Install();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            if (!_workContext.CurrentCustomer.IsAdmin())
                return;

            var configNode = new SiteMapNode
            {
                Title = "RemarketyWebApi plugin",
                Visible = true,
                ActionName = "Configure",
                ControllerName = "RemarketyWebAdmin",
                SystemName = "RemarketyWebApiConfig"
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