using Nop.Core.Configuration;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Settings
{
    public class RemarketyDiscountsSettings : ISettings
    {
        public bool Enabled { get; set; }
        public string DiscountNamePrefix { get; set; }
    }
}