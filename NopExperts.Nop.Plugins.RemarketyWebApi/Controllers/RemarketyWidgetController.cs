using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Services.Configuration;
using Nop.Web.Controllers;
using NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    public class RemarketyWidgetController : BasePublicController
    {
        private readonly ISettingService _settingService;
        private readonly RemarketyApiSettings _remarketyApiSettings;

        public RemarketyWidgetController(ISettingService settingService, RemarketyApiSettings remarketyApiSettings)
        {
            _settingService = settingService;
            _remarketyApiSettings = remarketyApiSettings;
        }

        public ActionResult GetStoreRemarketyWebTracking()
        {
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetStoreRemarketyWebTracking.cshtml", model: _remarketyApiSettings.RemarketyStoreId);
        }

        public ActionResult GetProductDetailsRemarketyWebTracking(string additionalData)
        {
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetProductDetailsRemarketyWebTracking.cshtml",model: additionalData);
        }
    }
}