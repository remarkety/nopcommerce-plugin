using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private RemarketyDiscountsSettings _remarketyDiscountsSettings;

        public RemarketyWebAdminController(IStoreContext storeContext, 
            RemarketyApiSettings remarketyApiSettings, 
            RemarketyStoreAddressSettings remarketyStoreAddressSettings,
            ISettingService settingService, RemarketyDiscountsSettings remarketyDiscountsSettings)
        {
            _storeContext = storeContext;
            _remarketyApiSettings = remarketyApiSettings;
            _remarketyStoreAddressSettings = remarketyStoreAddressSettings;
            _settingService = settingService;
            _remarketyDiscountsSettings = remarketyDiscountsSettings;
        }

        public ActionResult Configure()
        {
            var model = new RemarketyConfigModel
            {
                RemarketyWebApiKey =
                    _storeContext.CurrentStore.GetAttribute<Guid>(StringHelper.RemarketyApiKey).ToString(),

                StoreAddressModel = Mapper.Map<StoreAddressModel>(_remarketyStoreAddressSettings),
                ApiConfigModel = Mapper.Map<ApiConfigModel>(_remarketyApiSettings),
                DiscountConfigModel = Mapper.Map<DiscountConfigModel>(_remarketyDiscountsSettings),
                PluginVersion = StringHelper.GetPluginVersion()
            };
            
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWebAdmin/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(RemarketyConfigModel model)
        {
            _remarketyApiSettings = Mapper.Map<RemarketyApiSettings>(model.ApiConfigModel);
            _remarketyStoreAddressSettings = Mapper.Map<RemarketyStoreAddressSettings>(model.StoreAddressModel);
            _remarketyDiscountsSettings = Mapper.Map<RemarketyDiscountsSettings>(model.DiscountConfigModel);

            _settingService.SaveSetting(_remarketyApiSettings);
            _settingService.SaveSetting(_remarketyStoreAddressSettings);
            _settingService.SaveSetting(_remarketyDiscountsSettings);

            return RedirectToAction("Configure");
        }
    }
}