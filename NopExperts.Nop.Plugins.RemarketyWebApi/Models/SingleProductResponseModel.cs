using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class SingleProductResponseModel
    {
        public SingleProductResponseModel()
        {
            Product = new ProductResponseModel();
        }

        [JsonProperty("product")]
        public ProductResponseModel Product { get; set; }
    }
}