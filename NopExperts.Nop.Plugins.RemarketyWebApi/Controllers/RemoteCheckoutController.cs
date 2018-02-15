using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Orders;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    public class RemoteCheckoutController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;

        public RemoteCheckoutController(ILogger logger, 
            IProductService productService, 
            IWorkContext workContext, 
            IShoppingCartService shoppingCartService, 
            IStoreContext storeContext)
        {
            _logger = logger;
            _productService = productService;
            _workContext = workContext;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
        }

        [Route("checkout/remotecheckout")]
        public ActionResult RemoteCheckout(string products)
        {
            if (string.IsNullOrEmpty(products))
            {
                return Redirect("/cart");
            }

            var skuList = products.Split(',');

            foreach (var sku in skuList)
            {
                var product = _productService.GetProductBySku(sku);


                if (product == null)
                {
                    _logger.Warning($"RemarketyWebApi.RemoteCheckout: unknown product sku ({sku})");
                    continue;
                }

                //we can add only simple products
                if (product.ProductType != ProductType.SimpleProduct)
                {
                    _logger.Error($"RemarketyWebApi.RemoteCheckout: Only simple products could be added to the cart (sku: {sku})");
                    continue;
                }

                int quantity = 1;

                var cartType = ShoppingCartType.ShoppingCart;


                //get standard warnings without attribute validations
                //first, try to find existing shopping cart item
                var cart = _workContext.CurrentCustomer.ShoppingCartItems
                    .Where(sci => sci.ShoppingCartType == cartType)
                    .LimitPerStore(_storeContext.CurrentStore.Id)
                    .ToList();
                var shoppingCartItem = _shoppingCartService.FindShoppingCartItemInTheCart(cart, cartType, product);
                var quantityToValidate = shoppingCartItem?.Quantity + quantity ?? quantity;
                var addToCartWarnings = _shoppingCartService
                    .GetShoppingCartItemWarnings(_workContext.CurrentCustomer, cartType,
                    product, _storeContext.CurrentStore.Id, string.Empty,
                    decimal.Zero, null, null, quantityToValidate, false, true, false, false, false);

                if (addToCartWarnings.Count > 0)
                {
                    var warningsString = String.Join(", ", addToCartWarnings);
                    _logger.Error($"RemarketyWebApi.RemoteCheckout: can't add product to cart (sku: {sku}, error: {warningsString})");

                    continue;
                }

                addToCartWarnings = _shoppingCartService.AddToCart(customer: _workContext.CurrentCustomer,
                    product: product,
                    shoppingCartType: cartType,
                    storeId: _storeContext.CurrentStore.Id,
                    quantity: quantity);

                if (addToCartWarnings.Count > 0)
                {
                    var warningsString = String.Join(", ", addToCartWarnings);
                    //cannot be added to the cart
                    _logger.Error($"RemarketyWebApi.RemoteCheckout: can't add product to cart (sku: {sku}, error: {warningsString})");
                }
            }

            return Redirect("/cart");
        }
    }
}