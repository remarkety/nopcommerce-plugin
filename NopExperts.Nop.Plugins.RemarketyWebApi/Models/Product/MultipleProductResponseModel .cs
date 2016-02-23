using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.Product
{
    public class MultipleProductResponseModel
    {
        public MultipleProductResponseModel()
        {
            Products = new List<ProductResponseModel>();
        }

        [JsonProperty("products")]
        public List<ProductResponseModel> Products { get; set; }
    }
}