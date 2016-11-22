using System;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Filters
{
    public class EmailTrackingFilterAttribute : ActionFilterAttribute
    {
        private const string EMAIL_TRACK_KEY = "em";

        private readonly RemarketyApiSettings _remarketyApiSettings;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;

        public EmailTrackingFilterAttribute()
        {
            _remarketyApiSettings = EngineContext.Current.Resolve<RemarketyApiSettings>();
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
            _customerService = EngineContext.Current.Resolve<ICustomerService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // if email tracking not enabled => do nothing
            if (!_remarketyApiSettings.EmailTrackingEnabled)
                return;
            
            var emailBase64Encoded = filterContext.HttpContext.Request.QueryString.Get(EMAIL_TRACK_KEY);
            
            // if we haven't passed this parameter => just walk by.
            if (string.IsNullOrEmpty(emailBase64Encoded))
                return;

            var customer = _workContext.CurrentCustomer;

            // we need to update only guests with no email
            if (customer.IsRegistered() && !string.IsNullOrEmpty(customer.Email))
                return;

            var email = Encoding.UTF8.GetString(Convert.FromBase64String(emailBase64Encoded));

            customer.Email = email;

            _customerService.UpdateCustomer(customer);
        }
    }
}