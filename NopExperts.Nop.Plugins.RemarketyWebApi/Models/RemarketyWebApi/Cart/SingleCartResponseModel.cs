using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Cart
{
    public class SingleCartResponseModel
    {
        [JsonProperty("cart")]
        public CartResponseModel Cart { get; set; }
    }
}