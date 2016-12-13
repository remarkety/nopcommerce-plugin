using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Discounts
{
    public class PlaceNewDiscountResponseModel
    {
        [JsonProperty("is_success")]
        public bool IsSuccess { get; set; }
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}