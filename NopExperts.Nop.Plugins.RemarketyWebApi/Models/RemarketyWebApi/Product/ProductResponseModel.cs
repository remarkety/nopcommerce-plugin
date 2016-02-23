using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Product
{
    public class ProductResponseModel
    {
        public ProductResponseModel()
        {
            ProductCategories = new List<ProductCategoryModel>();
            Images = new List<ImageModel>();
            Options = new List<OptionModel>();
            Variants = new List<ProductVariantModel>();
        }

        [JsonProperty(PropertyName = "body_html")]
        public string BodyHtml { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "published_at")]
        public DateTime? PublishedAt { get; set; }

        [JsonProperty(PropertyName = "product_exists")]
        public bool ProductExists { get; set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "vendor")]
        public string Vendor { get; set; }



        [JsonProperty(PropertyName = "categories")]
        public IList<ProductCategoryModel> ProductCategories { get; set; }

        [JsonProperty(PropertyName = "image")]
        public ImageModel Image { get; set; }

        [JsonProperty(PropertyName = "images")]
        public IList<ImageModel> Images { get; set; }

        [JsonProperty(PropertyName = "options")]
        public IList<OptionModel> Options { get; set; }

        [JsonProperty(PropertyName = "variants")]
        public IList<ProductVariantModel> Variants { get; set; }



        public class ProductCategoryModel
        {
            [JsonProperty(PropertyName = "code")]
            public int Code { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        public class ImageModel
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "product_id")]
            public int ProductId { get; set; }

            [JsonProperty(PropertyName = "created_at")]
            public DateTime? CreatedAt { get; set; }

            [JsonProperty(PropertyName = "updated_at")]
            public DateTime? UpdatedAt { get; set; }

            [JsonProperty(PropertyName = "src")]
            public string Src { get; set; }

            [JsonProperty(PropertyName = "variant_ids")]
            public int[] VariantIds { get; set; }
        }

        public class OptionModel
        {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "values")]
            public string[] Values { get; set; }
        }

        public class ProductVariantModel
        {
            [JsonProperty(PropertyName = "barcode")]
            public string Barcode { get; set; }

            [JsonProperty(PropertyName = "compare_at_price")]
            public string CompareAtPrice { get; set; }

            [JsonProperty(PropertyName = "currency")]
            public string Currency { get; set; }

            [JsonProperty(PropertyName = "created_at")]
            public DateTime? CreatedAt { get; set; }

            [JsonProperty(PropertyName = "fulfillment_service")]
            public string FullfilmentService { get; set; }

            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "image")]
            public string Image { get; set; }

            [JsonProperty(PropertyName = "inventory_quantity")]
            public int InventoryQuantity { get; set; }

            [JsonProperty(PropertyName = "price")]
            public decimal Price { get; set; }

            [JsonProperty(PropertyName = "product_id")]
            public int ProductId { get; set; }

            [JsonProperty(PropertyName = "sku")]
            public string Sku { get; set; }

            [JsonProperty(PropertyName = "taxable")]
            public bool Taxable { get; set; }

            [JsonProperty(PropertyName = "title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "option1")]
            public string Option1 { get; set; }

            [JsonProperty(PropertyName = "option2")]
            public string Option2 { get; set; }

            [JsonProperty(PropertyName = "option3")]
            public string Option3 { get; set; }

            [JsonProperty(PropertyName = "updated_at")]
            public DateTime? UpdatedAt { get; set; }

            [JsonProperty(PropertyName = "requires_shipping")]
            public bool RequiresShipping { get; set; }

            [JsonProperty(PropertyName = "weight")]
            public decimal Weight { get; set; }

            [JsonProperty(PropertyName = "weight_unit")]
            public string WeightUnit { get; set; }
        }
    }
}