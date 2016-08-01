using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Services.Configuration;
using Nop.Web.Controllers;
using NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    public class RemarketyWidgetController : BasePublicController
    {
        private readonly ISettingService _settingService;

        public RemarketyWidgetController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public ActionResult GetStoreRemarketyWebTracking()
        {
            string storeId = _settingService.GetSettingByKey(StringHelper.RemarketyStoreIdSettingKey, String.Empty);
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetStoreRemarketyWebTracking.cshtml", model:storeId);
        }

        public ActionResult GetProductDetailsRemarketyWebTracking(string additionalData)
        {
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetProductDetailsRemarketyWebTracking.cshtml",model: additionalData);
        }
    }
}