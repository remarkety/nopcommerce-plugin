using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class AddressModel
    {
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        [JsonProperty("province_code")]
        public string ProvinceCode { get; set; }
        [JsonProperty("zip")]
        public string Zip { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}