using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Nop.Web.Framework.Components;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Components
{
    [ViewComponent(Name = "RemarketyWebTracking")]
    public class RemarketyWebTrackingComponent : NopViewComponent
    {
        private readonly RemarketyApiSettings _remarketyApiSettings;

        public RemarketyWebTrackingComponent(RemarketyApiSettings remarketyApiSettings)
        {
            _remarketyApiSettings = remarketyApiSettings;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            switch (widgetZone)
            {
                case "productdetails_bottom":
                    {
                        return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetProductDetailsRemarketyWebTracking.cshtml", model: additionalData);

                    }
                case "body_end_html_tag_before":
                    {
                        return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetStoreRemarketyWebTracking.cshtml", model: _remarketyApiSettings.RemarketyStoreId);
                    }
                default:
                    {
                        // unknown widget zone - just return empty result
                        return new ContentViewComponentResult(string.Empty);
                    }
            }
        }
    }
}