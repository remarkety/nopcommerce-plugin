using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Filters
{
    public class AuthorizationTokenRequiredAttribute : ActionFilterAttribute
    {
        const string AUTH_TOKEN_KEY = "remarkety_api_key";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var queryParameters = filterContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

            string authTokenValue = string.Empty;

            if (queryParameters.ContainsKey(AUTH_TOKEN_KEY))
            {
                authTokenValue = queryParameters[AUTH_TOKEN_KEY];
            }

            var storeContext = EngineContext.Current.Resolve<IStoreContext>();
            var token = storeContext.CurrentStore.GetAttribute<string>(StringHelper.RemarketyApiKey);

            if (authTokenValue != token)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                filterContext.Response = responseMessage;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}