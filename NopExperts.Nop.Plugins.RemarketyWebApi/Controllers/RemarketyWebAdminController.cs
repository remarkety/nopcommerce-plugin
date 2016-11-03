using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Nop.Admin.Controllers;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Services.Common;
using Nop.Services.Configuration;
using NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebAdmin;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    public class RemarketyWebAdminController : BaseAdminController
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;

        private RemarketyApiSettings _remarketyApiSettings;
        private RemarketyStoreAddressSettings _remarketyStoreAddressSettings;

        public RemarketyWebAdminController(IStoreContext storeContext, 
            RemarketyApiSettings remarketyApiSettings, 
            RemarketyStoreAddressSettings remarketyStoreAddressSettings,
            ISettingService settingService)
        {
            _storeContext = storeContext;
            _remarketyApiSettings = remarketyApiSettings;
            _remarketyStoreAddressSettings = remarketyStoreAddressSettings;
            _settingService = settingService;
        }

        public ActionResult Configure()
        {
            var model = new RemarketyConfigModel
            {
                RemarketyWebApiKey =
                    _storeContext.CurrentStore.GetAttribute<Guid>(StringHelper.RemarketyApiKey).ToString(),
                StoreAddressModel = Mapper.Map<StoreAddressModel>(_remarketyStoreAddressSettings),
                ApiConfigModel = Mapper.Map<ApiConfigModel>(_remarketyApiSettings)
            };
            
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWebAdmin/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(RemarketyConfigModel model)
        {
            _remarketyApiSettings = Mapper.Map<RemarketyApiSettings>(model.ApiConfigModel);
            _remarketyStoreAddressSettings = Mapper.Map<RemarketyStoreAddressSettings>(model.StoreAddressModel);

            _settingService.SaveSetting(_remarketyApiSettings);
            _settingService.SaveSetting(_remarketyStoreAddressSettings);

            return RedirectToAction("Configure");
        }
    }
}