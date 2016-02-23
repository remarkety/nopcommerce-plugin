using System.Collections.Generic;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Cart
{
    public class MultipleCartResponseModel
    {
        public MultipleCartResponseModel()
        {
            Carts = new List<CartResponseModel>();
        }

        public IList<CartResponseModel> Carts { get; set; }
    }
}