using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;
using Nop.Services.Logging;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Filters
{
    public class EmailTrackingFilterAttribute : ActionFilterAttribute
    {
        private const string EMAIL_TRACK_KEY = "em";

        private readonly RemarketyApiSettings _remarketyApiSettings;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        public EmailTrackingFilterAttribute()
        {
            _remarketyApiSettings = EngineContext.Current.Resolve<RemarketyApiSettings>();
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
            _customerService = EngineContext.Current.Resolve<ICustomerService>();
            _logger = EngineContext.Current.Resolve<ILogger>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var isAdminArea = filterContext.RouteData.Values.ContainsKey("area") &&
                                  (string)filterContext.RouteData.Values["area"] == "Admin";
            if (isAdminArea)
                return;

            _logger.Information("Email filter hit");

            // if email tracking not enabled => do nothing
            if (!_remarketyApiSettings.EmailTrackingEnabled)
                return;

            var emailBase64Encoded = filterContext.HttpContext.Request.Query[EMAIL_TRACK_KEY];

            // if we haven't passed this parameter => just walk by.
            if (string.IsNullOrEmpty(emailBase64Encoded))
                return;

            _logger.Information($"Email extracted: {emailBase64Encoded}");

            var customer = _workContext.CurrentCustomer;

            // we need to update only guests with no email
            if (customer.IsRegistered() && !string.IsNullOrEmpty(customer.Email))
                return;

            var email = Encoding.UTF8.GetString(Convert.FromBase64String(emailBase64Encoded));

            _logger.Information($"Email decoded: {email}");

            customer.Email = email;

            _customerService.UpdateCustomer(customer);
        }
    }
}