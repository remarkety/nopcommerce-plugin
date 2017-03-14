using Nop.Core.Configuration;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Settings
{
    public class RemarketyApiSettings : ISettings
    {
        public string RemarketyStoreId { get; set; }

        public bool EmailTrackingEnabled { get; set; }
    }
}