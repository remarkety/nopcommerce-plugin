using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models
{
    public class LineItemModel
    {
        public LineItemModel()
        {
            TaxLines = new List<TaxLineModel>();
        }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("variant_id")]
        public string VariantId { get; set; }

        [JsonProperty("variant_title")]
        public string VariantTitle { get; set; }

        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("taxable")]
        public bool Taxable { get; set; }

        [JsonProperty("tax_lines")]
        public List<TaxLineModel> TaxLines { get; set; }
    }
}