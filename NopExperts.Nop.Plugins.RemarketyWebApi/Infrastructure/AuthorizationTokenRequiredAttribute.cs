using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
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

            var token = ConfigurationManager.AppSettings["AuthorizationToken"] ?? "123";

            if (authTokenValue != token)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                filterContext.Response = responseMessage;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}