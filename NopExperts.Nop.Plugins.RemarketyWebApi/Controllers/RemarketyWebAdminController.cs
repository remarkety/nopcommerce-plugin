using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Nop.Admin.Controllers;
using Nop.Admin.Extensions;
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
                RemarketyWebApiKey = _storeContext.CurrentStore.GetAttribute<Guid>(StringHelper.RemarketyApiKey).ToString(),

                StoreAddressModel = new StoreAddressModel
                {
                    Address1 = _remarketyStoreAddressSettings.Address1,
                    Address2 = _remarketyStoreAddressSettings.Address2,
                    City = _remarketyStoreAddressSettings.City,
                    Country = _remarketyStoreAddressSettings.Country,
                    State = _remarketyStoreAddressSettings.State,
                    Zip = _remarketyStoreAddressSettings.Zip
                },
                ApiConfigModel = new ApiConfigModel
                {
                    EmailTrackingEnabled = _remarketyApiSettings.EmailTrackingEnabled,
                    RemarketyStoreId = _remarketyApiSettings.RemarketyStoreId,
                   
                },
                DiscountConfigModel =new DiscountConfigModel
                {
                    DiscountNamePrefix = _remarketyDiscountsSettings.DiscountNamePrefix,
                    Enabled = _remarketyDiscountsSettings.Enabled
                }
            };

            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWebAdmin/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(RemarketyConfigModel model)
        {
            _remarketyApiSettings.EmailTrackingEnabled = model.ApiConfigModel.EmailTrackingEnabled;
            _remarketyApiSettings.RemarketyStoreId = model.ApiConfigModel.RemarketyStoreId;
            
            _remarketyStoreAddressSettings.Address1 = model.StoreAddressModel.Address1;
            _remarketyStoreAddressSettings.Address2 = model.StoreAddressModel.Address2;
            _remarketyStoreAddressSettings.City = model.StoreAddressModel.City;
            _remarketyStoreAddressSettings.Country = model.StoreAddressModel.Country;
            _remarketyStoreAddressSettings.State = model.StoreAddressModel.State;
            _remarketyStoreAddressSettings.Zip = model.StoreAddressModel.Zip;

            _remarketyDiscountsSettings.Enabled = model.DiscountConfigModel.Enabled;
            _remarketyDiscountsSettings.DiscountNamePrefix = model.DiscountConfigModel.DiscountNamePrefix;
            
            _settingService.SaveSetting(_remarketyApiSettings);
            _settingService.SaveSetting(_remarketyStoreAddressSettings);
            _settingService.SaveSetting(_remarketyDiscountsSettings);

            return RedirectToAction("Configure");
        }
    }
}