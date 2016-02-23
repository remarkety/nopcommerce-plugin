using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Controllers;
using Nop.Core;
using Nop.Services.Common;
using NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebAdmin;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    public class RemarketyWebAdminController : BaseAdminController
    {
        private readonly IStoreContext _storeContext;

        public RemarketyWebAdminController(IStoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public ActionResult Configure()
        {
            var model = new ConfigureModel
            {
                RemarketyWebApiKey =
                    _storeContext.CurrentStore.GetAttribute<Guid>(StringHelper.RemarketyApiKey).ToString()
            };


            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWebAdmin/Configure.cshtml", model);
        }
    }
}