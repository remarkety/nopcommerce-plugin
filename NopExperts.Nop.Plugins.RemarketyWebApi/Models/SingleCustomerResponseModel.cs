using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class SingleCustomerResponseModel
    {
        [JsonProperty("customer")]
        public CustomerResponseModel Customer { get; set; }
    }
}