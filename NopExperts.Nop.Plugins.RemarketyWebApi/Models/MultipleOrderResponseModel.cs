using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class MultipleOrderResponseModel
    {
        [JsonProperty("orders")]
        public List<OrderResponseModel> Orders { get; set; }
    }
}