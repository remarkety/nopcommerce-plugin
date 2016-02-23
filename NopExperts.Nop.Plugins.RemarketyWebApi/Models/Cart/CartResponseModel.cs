using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.Customer;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.Cart
{
    public class CartResponseModel
    {
        public CartResponseModel()
        {
            DiscountCodes = new List<DiscountCodeModel>();
        }

        [JsonProperty("abandoned_checkout_url")]
        public string AbandonedCheckoutUrl { get; set; }

        [JsonProperty("accepts_marketing")]
        public bool AcceptsMarketing { get; set; }

        [JsonProperty("cart_token")]
        public Guid CartToken { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("customer")]
        public CustomerResponseModel Customer { get; set; }

        [JsonProperty("discount_codes")]
        public List<DiscountCodeModel> DiscountCodes { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fulfillment_status")]
        public string FulfillmentStatus { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("line_items")]
        public List<LineItemModel> LineItems { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("shipping_lines")]
        public List<ShippingLineModel> ShippingLines { get; set; }

        [JsonProperty("subtotal_price")]
        public decimal SubtotalPrice { get; set; }

        [JsonProperty("tax_lines")]
        public List<TaxLineModel> TaxLines { get; set; }

        [JsonProperty("taxes_included")]
        public bool TaxesIncluded { get; set; }

        [JsonProperty("total_discounts")]
        public decimal? TotalDiscounts { get; set; }

        [JsonProperty("total_line_items_price")]
        public decimal? TotalLineItemsPrice { get; set; }

        [JsonProperty("total_price")]
        public decimal TotalPrice { get; set; }

        [JsonProperty("total_shipping")]
        public decimal? TotalShipping { get; set; }

        [JsonProperty("total_tax")]
        public decimal? TotalTax { get; set; }

        [JsonProperty("total_weight")]
        public decimal TotalWeight { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }


        [JsonProperty("billing_address")]
        public AddressModel BillingAddress { get; set; }

        [JsonProperty("shipping_address")]
        public AddressModel ShippingAddress { get; set; }
    }
}