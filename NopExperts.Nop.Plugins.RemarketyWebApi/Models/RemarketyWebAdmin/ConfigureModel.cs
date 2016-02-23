using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebAdmin
{
    public class ConfigureModel : BaseNopModel
    {
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.RemarketyWebApiKey")]
        public string RemarketyWebApiKey { get; set; }
    }
}