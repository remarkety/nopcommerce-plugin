using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class CountResponseModel
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}