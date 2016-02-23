using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class SingleCartResponseModel
    {
        [JsonProperty("cart")]
        public CartResponseModel Cart { get; set; }
    }
}