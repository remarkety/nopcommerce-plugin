using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Filters
{
    public class AuthorizationTokenRequiredAttribute : ActionFilterAttribute
    {
        const string AUTH_TOKEN_KEY = "remarkety_api_key";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryParameters = context.HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value);
            
            string authTokenValue = string.Empty;

            if (queryParameters.ContainsKey(AUTH_TOKEN_KEY))
            {
                authTokenValue = queryParameters[AUTH_TOKEN_KEY];
            }

            var storeContext = EngineContext.Current.Resolve<IStoreContext>();
            var token = storeContext.CurrentStore.GetAttribute<string>(StringHelper.RemarketyApiKey);

            if (authTokenValue != token)
            {
               // var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}