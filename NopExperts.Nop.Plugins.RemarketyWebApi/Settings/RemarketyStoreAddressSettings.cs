using Newtonsoft.Json;
using Nop.Core.Configuration;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Settings
{
    public class RemarketyStoreAddressSettings : ISettings
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zip { get; set; }
    }
}