using System.Collections.Generic;
using System.Web.Mvc;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Filters
{
    public class FilterProvider : IFilterProvider
    {

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var isAdminArea = controllerContext.RequestContext.RouteData.DataTokens.ContainsKey("area") &&
                              (string)controllerContext.RequestContext.RouteData.DataTokens["area"] == "Admin"
                              ;

            if (!(isAdminArea || controllerContext.IsChildAction))
            {
                return new[]
                    {
                        new Filter(new EmailTrackingFilterAttribute(), FilterScope.Action, null)
                    };
            }
            
            return new Filter[] { };
        }
    }
}