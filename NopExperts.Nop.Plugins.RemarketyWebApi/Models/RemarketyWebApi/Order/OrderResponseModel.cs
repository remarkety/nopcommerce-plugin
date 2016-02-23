using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Customer;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Order
{
    public class OrderResponseModel
    {
        public OrderResponseModel()
        {
            DiscountCodes = new List<DiscountCodeModel>();
            LineItems = new List<LineItemModel>();
            ShippingLines = new List<ShippingLineModel>();
            TaxLines = new List<TaxLineModel>();
        }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("customer")]
        public CustomerResponseModel Customer { get; set; }

        [JsonProperty("dicsount_codes")]
        public List<DiscountCodeModel> DiscountCodes { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fulfillment_status")]
        public string FulfillmentStatus { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("line_items")]
        public List<LineItemModel> LineItems { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("shipping_lines")]
        public List<ShippingLineModel> ShippingLines { get; set; }

        [JsonProperty("status")]
        public StatusModel Status { get; set; }

        [JsonProperty("subtotal_price")]
        public decimal SubtotalPrice { get; set; }

        [JsonProperty("tax_lines")]
        public List<TaxLineModel> TaxLines { get; set; }

        [JsonProperty("tax_included")]
        public bool TaxesIncluded { get; set; }

        [JsonProperty("test")]
        public bool Test { get; set; }

        [JsonProperty("total_discounts")]
        public decimal TotalDiscounts { get; set; }

        [JsonProperty("total_line_items_price")]
        public decimal TotalLineItemsPrice { get; set; }

        [JsonProperty("total_price")]
        public decimal TotalPrice { get; set; }

        [JsonProperty("total_shipping")]
        public decimal TotalShipping { get; set; }

        [JsonProperty("total_tax")]
        public decimal TotalTax { get; set; }

        [JsonProperty("total_weight")]
        public decimal? TotalWeight { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }


        public class StatusModel
        {
            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}