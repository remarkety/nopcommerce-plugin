using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Web.Controllers;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    public class RemarketyWidgetController : BasePublicController
    {
        private readonly ISettingService _settingService;
        private readonly RemarketyApiSettings _remarketyApiSettings;
        private readonly IProductService _productService;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IShoppingCartService _shoppingCartService;

        public RemarketyWidgetController(ISettingService settingService,
            RemarketyApiSettings remarketyApiSettings, IProductService productService, 
            ILogger logger, IWorkContext workContext, IStoreContext storeContext, 
            IShoppingCartService shoppingCartService)
        {
            _settingService = settingService;
            _remarketyApiSettings = remarketyApiSettings;
            _productService = productService;
            _logger = logger;
            _workContext = workContext;
            _storeContext = storeContext;
            _shoppingCartService = shoppingCartService;
        }

        public ActionResult GetStoreRemarketyWebTracking()
        {
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetStoreRemarketyWebTracking.cshtml", model: _remarketyApiSettings.RemarketyStoreId);
        }

        public ActionResult GetProductDetailsRemarketyWebTracking(string additionalData)
        {
            return View("~/Plugins/NopExperts.RemarketyWebApi/Views/RemarketyWidget/GetProductDetailsRemarketyWebTracking.cshtml",model: additionalData);
        }

        public ActionResult RemoteCheckout(string products)
        {
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