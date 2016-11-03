using Nop.Core.Configuration;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Settings
{
    public class RemarketyApiSettings : ISettings
    {
        public string TimeZone { get; set; }

        public string RemarketyStoreId { get; set; }
        public int StoreLogoPictureId { get; set; }
    }
}