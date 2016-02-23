using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class ShippingLineModel
    {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}