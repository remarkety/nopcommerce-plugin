using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Store
{
    public class StoreSettingsResponseModel
    {
        public StoreSettingsResponseModel()
        {
            ContactInfo = new ContactInfoModel();
            Address = new AddressModel();
            OrderStatuses = new List<OrderStatusModel>();
        }

        [JsonProperty(PropertyName = "store_front_url")]
        public string StoreFrontUrl { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "logo_url")]
        public string LogoUrl { get; set; }
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; set; }


        [JsonProperty(PropertyName = "contact_info")]
        public ContactInfoModel ContactInfo { get; set; }
        [JsonProperty(PropertyName = "address")]
        public AddressModel Address { get; set; }
        [JsonProperty(PropertyName = "order_statuses")]
        public IList<OrderStatusModel> OrderStatuses { get; set; }

        public class ContactInfoModel
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "email")]
            public string Email { get; set; }
            [JsonProperty(PropertyName = "phone")]
            public string Phone { get; set; }
        }

        public class AddressModel
        {
            [JsonProperty(PropertyName = "country")]
            public string Country { get; set; }
            [JsonProperty(PropertyName = "state")]
            public string State { get; set; }
            [JsonProperty(PropertyName = "city")]
            public string City { get; set; }
            [JsonProperty(PropertyName = "address_1")]
            public string Address1 { get; set; }
            [JsonProperty(PropertyName = "address_2")]
            public string Address2 { get; set; }
            [JsonProperty(PropertyName = "zip")]
            public string Zip { get; set; }
        }

        public class OrderStatusModel
        {
            [JsonProperty(PropertyName = "code")]
            public string Code { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

        }
    }

}