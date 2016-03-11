using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Order
{
    public class SingleOrderResponseModel
    {
        [JsonProperty("order")]
        public OrderResponseModel Order { get; set; }
    }
}