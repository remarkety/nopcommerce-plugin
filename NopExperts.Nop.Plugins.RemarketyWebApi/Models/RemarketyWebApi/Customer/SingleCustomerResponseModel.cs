using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Customer
{
    public class SingleCustomerResponseModel
    {
        [JsonProperty("customer")]
        public CustomerResponseModel Customer { get; set; }
    }
}