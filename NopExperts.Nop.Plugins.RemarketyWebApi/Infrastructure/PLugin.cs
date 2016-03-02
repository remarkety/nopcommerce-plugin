using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Web.Framework.Menu;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class Plugin : BasePlugin, IAdminMenuPlugin
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
    }
}