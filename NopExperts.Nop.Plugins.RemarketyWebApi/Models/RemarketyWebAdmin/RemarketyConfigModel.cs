using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebAdmin
{
    public class RemarketyConfigModel : BaseNopModel
    {
        public RemarketyConfigModel()
        {
            StoreAddressModel = new StoreAddressModel();
            ApiConfigModel = new ApiConfigModel();
        }

        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.RemarketyWebApiKey")]
        public string RemarketyWebApiKey { get; set; }

        public StoreAddressModel StoreAddressModel { get; set; }
        public ApiConfigModel ApiConfigModel { get; set; }
        public DiscountConfigModel DiscountConfigModel { get; set; }
    }

    public class DiscountConfigModel : BaseNopModel
    {
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.DiscountConfig.Enabled")]
        public bool Enabled { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.DiscountConfig.DiscountNamePrefix")]
        public string DiscountNamePrefix { get; set; }
    }

    public class ApiConfigModel : BaseNopModel
    {
        [UIHint("Picture")]
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.StoreLogoPictureId")]
        public int StoreLogoPictureId { get; set; }

        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.TimeZone")]
        public string TimeZone { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.RemarketyStoreId")]
        public string RemarketyStoreId { get; set; }

        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.EmailTrackingEnabled")]
        public bool EmailTrackingEnabled { get; set; }
    }

    public class StoreAddressModel : BaseNopModel
    {
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.Country")]
        public string Country { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.State")]
        public string State { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.City")]
        public string City { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.Address1")]
        public string Address1 { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.Address2")]
        public string Address2 { get; set; }
        [NopResourceDisplayName("NopExperts.RemarketyWebApi.Configure.Zip")]
        public string Zip { get; set; }
    }
}