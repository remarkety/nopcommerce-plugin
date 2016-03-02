using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Cart
{
    public class MultipleCartResponseModel
    {
        public MultipleCartResponseModel()
        {
            Carts = new List<CartResponseModel>();
        }

        [JsonProperty("carts")]
        public IList<CartResponseModel> Carts { get; set; }
    }
}