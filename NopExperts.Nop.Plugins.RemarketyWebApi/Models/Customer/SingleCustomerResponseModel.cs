using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.Customer
{
    public class SingleCustomerResponseModel
    {
        [JsonProperty("customer")]
        public CustomerResponseModel Customer { get; set; }
    }
}