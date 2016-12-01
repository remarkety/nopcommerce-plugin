using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Tax;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Seo;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Framework.Themes;
using NopExperts.Nop.Plugins.RemarketyWebApi.Filters;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Cart;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Customer;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Order;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Product;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models.RemarketyWebApi.Store;
using NopExperts.Nop.Plugins.RemarketyWebApi.Settings;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    [AuthorizationTokenRequired]
    [RoutePrefix("RemarketyWebApi")]
    public class RemarketyWebApiController : ApiController
    {
        private const string DEFAULT_CURRENSY = "USD";
        private const string DEFAULT_LOCALE = "en_US";

        // services
        private readonly IStoreContext _storeContext;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IVendorService _vendorService;
        private readonly IPictureService _pictureService;
        private readonly ILogger _logger;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IWorkContext _workContext;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ITaxService _taxService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IThemeContext _themeContext;
        private readonly ILanguageService _languageService;
        private readonly IRepository<Customer> _customersRepository;

        // settings
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly TaxSettings _taxSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly RemarketyApiSettings _remarketyApiSettings;
        private readonly RemarketyStoreAddressSettings _remarketyStoreAddressSettings;

        // repository (for perfomance optimization)
        private readonly IRepository<Product> _productRepository;
        private readonly IProductAttributeParser _productAttributeParser;

        public RemarketyWebApiController()
        {
            _languageService = EngineContext.Current.Resolve<ILanguageService>();
            _themeContext = EngineContext.Current.Resolve<IThemeContext>();
            _dateTimeHelper = EngineContext.Current.Resolve<IDateTimeHelper>();
            _productAttributeParser = EngineContext.Current.Resolve<IProductAttributeParser>();
            _currencyService = EngineContext.Current.Resolve<ICurrencyService>();
            _priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
            _taxService = EngineContext.Current.Resolve<ITaxService>();
            _taxSettings = EngineContext.Current.Resolve<TaxSettings>();
            _orderTotalCalculationService = EngineContext.Current.Resolve<IOrderTotalCalculationService>();
            _shoppingCartService = EngineContext.Current.Resolve<IShoppingCartService>();
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
            _logger = EngineContext.Current.Resolve<ILogger>();
            _newsLetterSubscriptionService = EngineContext.Current.Resolve<INewsLetterSubscriptionService>();
            _vendorService = EngineContext.Current.Resolve<IVendorService>();
            _pictureService = EngineContext.Current.Resolve<IPictureService>();
            _emailAccountService = EngineContext.Current.Resolve<IEmailAccountService>();
            _emailAccountSettings = EngineContext.Current.Resolve<EmailAccountSettings>();
            _storeContext = EngineContext.Current.Resolve<IStoreContext>();
            _productRepository = EngineContext.Current.Resolve<IRepository<Product>>();
            _productService = EngineContext.Current.Resolve<IProductService>();
            _orderService = EngineContext.Current.Resolve<IOrderService>();
            _customerService = EngineContext.Current.Resolve<ICustomerService>();
            _currencySettings = EngineContext.Current.Resolve<CurrencySettings>();
            _remarketyApiSettings = EngineContext.Current.Resolve<RemarketyApiSettings>();
            _remarketyStoreAddressSettings = EngineContext.Current.Resolve<RemarketyStoreAddressSettings>();

            _customersRepository = EngineContext.Current.Resolve<IRepository<Customer>>();
        }

        #region /store

        [HttpGet]
        [Route("store/settings")]
        public StoreSettingsResponseModel StoreSettings()
        {
            var store = _storeContext.CurrentStore;
            var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);

            var enumList = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(x => new StoreSettingsResponseModel.OrderStatusModel
                {
                    Code = ((int)x).ToString(),
                    Name = x.ToString()
                });

            var logoPath = _pictureService.GetPictureUrl(_remarketyApiSettings.StoreLogoPictureId);
            var locale = _languageService.GetAllLanguages(storeId: store.Id).FirstOrDefault()?.LanguageCulture;


            return new StoreSettingsResponseModel
            {
                StoreFrontUrl = store.Url,
                Name = store.Name,
                Timezone = _remarketyApiSettings.TimeZone, //"Asia/Jerusalem",
                Currency = GetCurrentCurrencyCode(),
                Locale = locale?.Replace('-', '_') ?? DEFAULT_LOCALE,
                LogoUrl = logoPath,
                ContactInfo = new StoreSettingsResponseModel.ContactInfoModel
                {
                    Name = store.CompanyName,
                    Email = emailAccount.Email,
                    // Phone = null
                },
                Address = new StoreSettingsResponseModel.AddressModel
                {
                    Address1 = _remarketyStoreAddressSettings.Address1,
                    Address2 = _remarketyStoreAddressSettings.Address2,
                    City = _remarketyStoreAddressSettings.City,
                    Country = _remarketyStoreAddressSettings.Country,
                    State = _remarketyStoreAddressSettings.State,
                    Zip = _remarketyStoreAddressSettings.Zip
                },
                OrderStatuses = enumList.ToList()
            };
        }

        #endregion

        #region /products


        [HttpGet]
        [Route("products")]
        public MultipleProductResponseModel Products(int page = 0, int limit = int.MaxValue,
            [FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var products = _productRepository.TableNoTracking;

            var filteredProducts = updatedAt.HasValue
                ? products.Where(x => !x.Deleted && x.Published && x.UpdatedOnUtc >= updatedAt)
                : products.Where(x => !x.Deleted && x.Published);

            var pagedProducts = new PagedList<Product>(filteredProducts.ToList(), page, limit);

            return new MultipleProductResponseModel
            {
                Products = pagedProducts.Select(PrepareProductResponseModel).ToList()
            };
        }

        [HttpGet]
        [Route("products/{productId:int}")]
        public SingleProductResponseModel ProductById(int productId)
        {
            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                _logger.Error($"RemarketyWebApi (products/{productId}): unknown productId ({productId})");

                return new SingleProductResponseModel
                {
                    Product = null
                };
            }

            return new SingleProductResponseModel
            {
                Product = PrepareProductResponseModel(product)
            };
        }

        [HttpGet]
        [Route("products/count")]
        public CountResponseModel ProductsConut([FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var products = _productRepository.TableNoTracking;

            var productsCount = updatedAt.HasValue
                ? products.Count(x => !x.Deleted && x.Published && x.UpdatedOnUtc >= updatedAt)
                : products.Count(x => !x.Deleted && x.Published);

            return new CountResponseModel
            {
                Count = productsCount
            };
        }


        #region util methods

        private ProductResponseModel PrepareProductResponseModel(Product product)
        {
            var productUrl = HttpUtility.UrlDecode(Url.Link("Product", new { SeName = product.GetSeName() }));
            var productVendor = _vendorService.GetVendorById(product.VendorId);

            var defaultPicture = product.ProductPictures.FirstOrDefault();

            ProductResponseModel.ImageModel productImage = null;

            if (defaultPicture != null)
            {
                productImage = PrepareProductImageModel(defaultPicture);
            }

            var productImages = product.ProductPictures.Select(PrepareProductImageModel).ToList();
            var productOptions = product.ProductAttributeMappings.Select(PrepareProductOptionsModel).ToList();
            var productCategories = product.ProductCategories.Select(PrepareProductCategoryModel).ToList();
            var productTags = product.ProductTags.Select(x => x.Name).ToArray();
            var productVariants = new List<ProductResponseModel.ProductVariantModel>();

            if (product.ProductAttributeCombinations.Any())
            {
                productVariants = product.ProductAttributeCombinations.Select(PrepareProductVariantModel).ToList();
            }
            else
            {
                productVariants.Add(PrepareProductVariantModel(new ProductAttributeCombination
                {
                    Product = product,
                    Sku = product.Sku,
                    StockQuantity = product.StockQuantity
                }));
            }

            return new ProductResponseModel
            {
                Id = product.Id,
                BodyHtml = product.FullDescription,
                CreatedAt = product.CreatedOnUtc,
                UpdatedAt = product.UpdatedOnUtc,
                ProductExists = !product.Deleted,
                PublishedAt = product.AvailableStartDateTimeUtc,
                Sku = product.Sku,
                Title = product.Name,
                Vendor = productVendor?.Name,
                Url = productUrl,
                Tags = productTags,
                ProductCategories = productCategories,
                Image = productImage,
                Images = productImages,
                Options = productOptions,
                Variants = productVariants
            };
        }

        private ProductResponseModel.ProductCategoryModel PrepareProductCategoryModel(ProductCategory productCategory)
        {
            return new ProductResponseModel.ProductCategoryModel
            {
                Code = productCategory.CategoryId,
                Name = productCategory.Category?.Name
            };
        }

        private ProductResponseModel.OptionModel PrepareProductOptionsModel(
            ProductAttributeMapping productAttributeMapping)
        {
            var values = productAttributeMapping.ProductAttributeValues.Select(x => x.Name).ToArray();

            return new ProductResponseModel.OptionModel
            {
                Id = productAttributeMapping.ProductAttribute.Id,
                Name = productAttributeMapping.ProductAttribute.Name,
                Values = values
            };
        }

        private ProductResponseModel.ImageModel PrepareProductImageModel(ProductPicture productPicture)
        {
            return new ProductResponseModel.ImageModel
            {
                Id = productPicture.PictureId,
                ProductId = productPicture.ProductId,
                Src = _pictureService.GetPictureUrl(productPicture.PictureId)
            };
        }

        private ProductResponseModel.ProductVariantModel PrepareProductVariantModel(
            ProductAttributeCombination productAttributeCombination)
        {
            var product = productAttributeCombination.Product;

            return new ProductResponseModel.ProductVariantModel
            {
                Id = productAttributeCombination.Id,
                Barcode = productAttributeCombination.Gtin ?? product.Gtin,
                CompareAtPrice = null,
                CreatedAt = null,
                ProductId = product.Id,
                //Image = productAttributeCombination.
                Currency = GetCurrentCurrencyCode(),
                FullfilmentService = null,
                InventoryQuantity = productAttributeCombination.StockQuantity,
                Price = productAttributeCombination.OverriddenPrice ?? product.Price,
                RequiresShipping = product.IsShipEnabled,
                Sku = productAttributeCombination.Sku,
                Title = null,
                UpdatedAt = null,
                Taxable = !product.IsTaxExempt,
                Weight = product.Weight
            };
        }

        #endregion

        #endregion

        #region /customers

        [HttpGet]
        [Route("customers")]
        public MultipleCustomerResponseModel Customers(int page = 0, int limit = int.MaxValue,
            [FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var defaultRoles = new[]
            {
                _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered).Id
            };

            var customers = _customerService.GetAllCustomers(updatedAt, customerRoleIds: defaultRoles, pageIndex: page,
                pageSize: limit);

            return new MultipleCustomerResponseModel
            {
                Customers = customers.Select(PrepareCustomerResponseModel).ToList()
            };
        }

        [HttpGet]
        [Route("customers/{customerId:int}")]
        public SingleCustomerResponseModel CustomerById(int customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);

            if (customer == null)
            {
                _logger.Error($"RemarketyWebApi (customers/{customerId}): unknown customerId ({customerId})");

                return new SingleCustomerResponseModel
                {
                    Customer = null
                };
            }

            return new SingleCustomerResponseModel
            {
                Customer = PrepareCustomerResponseModel(customer)
            };
        }

        [HttpGet]
        [Route("customers/count")]
        public CountResponseModel CustomersConut([FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var defaultRoles = new[]
            {
                _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered).Id
            };

            var customers = _customerService.GetAllCustomers(updatedAt, customerRoleIds: defaultRoles);

            return new CountResponseModel
            {
                Count = customers.Count
            };
        }

        #region util methods

        private CustomerResponseModel PrepareCustomerResponseModel(Customer customer)
        {
            var defaultAddress = customer.ShippingAddress ?? customer.Addresses.FirstOrDefault();
            var defaultAddressModel = PrepareAddressModel(defaultAddress);
            var customerGroups = customer.CustomerRoles.Select(PrepareCustomerGroupModel).ToList();

            var newsletterSubscription =
                _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email,
                    _storeContext.CurrentStore.Id);

            string birthDateString = null;
            var birthDate = customer.GetAttribute<DateTime?>(SystemCustomerAttributeNames.DateOfBirth);

            if (birthDate != null)
            {
                birthDateString = birthDate.Value.ToString("yy-MM-dd");
            }

            return new CustomerResponseModel
            {
                Id = customer.Id,
                CreatedAt = customer.CreatedOnUtc,
                UpdatedAt = customer.CreatedOnUtc,
                Title = String.Empty,
                Email = customer.Email,
                Tags = new string[] { },
                AcceptsMarketing = newsletterSubscription?.Active ?? false,
                BirthDate = birthDateString,
                DefaultAddress = defaultAddressModel,
                FirstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName),
                Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender),
                Groups = customerGroups,
                Info = new object(),
                LastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName),
                VerifiedEmail = null
            };
        }

        private AddressModel PrepareAddressModel(Address address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressModel
            {
                Country = address.Country?.Name,
                Phone = address.PhoneNumber,
                Zip = address.ZipPostalCode,
                CountryCode = address.Country?.ThreeLetterIsoCode,
                ProvinceCode = address.StateProvince?.Abbreviation
            };
        }

        private CustomerResponseModel.GroupModel PrepareCustomerGroupModel(CustomerRole role)
        {
            return new CustomerResponseModel.GroupModel
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        #endregion

        #endregion

        #region /orders

        [HttpGet]
        [Route("orders/")]
        public MultipleOrderResponseModel GetOrders(int page = 0, int limit = int.MaxValue,
            [FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var orders = _orderService
                .SearchOrders(createdFromUtc: updatedAt, pageIndex: page, pageSize: limit)
                .Select(PrepareOrderResponseModel)
                .ToList();

            return new MultipleOrderResponseModel
            {
                Orders = orders
            };
        }

        [HttpGet]
        [Route("orders/{orderId:int}")]
        public SingleOrderResponseModel GetOrderById(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);

            if (order == null)
            {
                _logger.Error($"RemarketyAPI: orders/{orderId} - missing order data (orderId: {orderId})");
                return null;
            }

            return new SingleOrderResponseModel
            {
                Order = PrepareOrderResponseModel(order)
            };
        }


        [HttpGet]
        [Route("orders/count")]
        public CountResponseModel OrdersConut([FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var ordersCount = _orderService.SearchOrders(createdFromUtc: updatedAt).Count;

            return new CountResponseModel
            {
                Count = ordersCount
            };
        }

        #region utils

        private OrderResponseModel PrepareOrderResponseModel(Order order)
        {
            try
            {
                var totalWeight = order.OrderItems.Sum(x => x.ItemWeight);
                var totalLineItemsPrice = order.OrderItems.Sum(x => x.PriceInclTax);

                var orderCustomerModel = PrepareCustomerResponseModel(order.Customer);
                var discountCodes = order.DiscountUsageHistory.Select(PrepareDiscountCodeModel).ToList();
                var lineItemsModel = order.OrderItems.Select(PrepareLineItemModel).ToList();
                var taxLinesModel = order.TaxRatesDictionary.Select(PrepareTaxLineModel).ToList();

                var orderStatusModel = new OrderResponseModel.StatusModel
                {
                    Name = order.OrderStatus.ToString(),
                    Code = order.OrderStatusId.ToString()
                };

                var orderPaymentStatusModel = new OrderResponseModel.StatusModel
                {
                    Name = order.PaymentStatus.ToString(),
                    Code = order.PaymentStatusId.ToString()
                };

                var shippingLinesModel = new List<ShippingLineModel>
                {
                    PrepareShippingLineModel(order)
                };

                if (String.IsNullOrEmpty(orderCustomerModel.Email))
                {
                    orderCustomerModel.Email = order.BillingAddress.Email ?? order.ShippingAddress.Email;

                    var newsletterSubscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(orderCustomerModel.Email,
                               _storeContext.CurrentStore.Id);

                    orderCustomerModel.AcceptsMarketing = newsletterSubscription?.Active ?? false;
                }

                return new OrderResponseModel
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedOnUtc,
                    Customer = orderCustomerModel,
                    UpdatedAt = order.CreatedOnUtc,
                    Currency = GetCurrentCurrencyCode(),
                    Email = order.BillingAddress.Email ?? order.ShippingAddress.Email ?? order.Customer.Email,
                    DiscountCodes = discountCodes,
                    FulfillmentStatus = null,
                    LineItems = lineItemsModel,
                    Name = $"#{order.Id}",
                    Note = order.OrderNotes.FirstOrDefault(x => x.DisplayToCustomer)?.Note,
                    ShippingLines = shippingLinesModel,
                    Status = orderStatusModel,
                    FinancialStatus = orderPaymentStatusModel,
                    SubtotalPrice = order.OrderSubtotalInclTax,
                    TaxLines = taxLinesModel,
                    TaxesIncluded = true,
                    Test = false,
                    TotalDiscounts = order.OrderSubTotalDiscountInclTax,
                    TotalLineItemsPrice = totalLineItemsPrice,
                    TotalPrice = order.OrderTotal,
                    TotalShipping = order.OrderShippingInclTax,
                    TotalTax = order.OrderTax,
                    TotalWeight = totalWeight
                };

            }
            catch (Exception ex)
            {
                _logger.Error($"RemarketyWebApi, orders/(id): {order?.Id}, {ex.Message} ", ex);

                throw;
            }
        }

        private TaxLineModel PrepareTaxLineModel(KeyValuePair<decimal, decimal> taxInfo)
        {
            return new TaxLineModel
            {
                Title = null,
                Price = taxInfo.Value,
                Rate = taxInfo.Key
            };
        }

        private ShippingLineModel PrepareShippingLineModel(Order order)
        {
            return new ShippingLineModel
            {
                Title = order.ShippingMethod,
                Code = order.ShippingMethod,
                Price = order.OrderShippingInclTax
            };
        }

        private DiscountCodeModel PrepareDiscountCodeModel(DiscountUsageHistory discountUsageHistory)
        {
            var discount = discountUsageHistory.Discount;

            return new DiscountCodeModel
            {
                Code = discount.Id.ToString(),
                Type = discount.UsePercentage ? "percentage" : "fixed_amount",
                Amount = discount.DiscountAmount
            };
        }

        private LineItemModel PrepareLineItemModel(OrderItem orderItem)
        {
            var product = orderItem.Product;
            var productVendor = _vendorService.GetVendorById(product.VendorId);

            return new LineItemModel
            {
                Name = product.Name,
                Title = product.Name,
                Sku = product.Sku,
                ProductId = product.Id,
                Vendor = productVendor?.Name,
                TaxLines = null,
                Price = orderItem.PriceInclTax,
                Taxable = true,
                Quantity = orderItem.Quantity,
                VariantId = null,
                VariantTitle = null
            };
        }

        #endregion

        #endregion

        #region /carts

        [HttpGet]
        [Route("carts/")]
        public MultipleCartResponseModel GetCarts(int page = 0, int limit = int.MaxValue,
            [FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var defaultRoles = new[]
            {
                _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered).Id
            };

            //var customers = _customerService.GetAllCustomers(customerRoleIds: defaultRoles, pageIndex: page,
            //    pageSize: limit, loadOnlyWithShoppingCart: true);
            // .Where(x => x.LastActivityDateUtc >= (updatedAt ?? DateTime.MinValue));

            var query = _customersRepository.TableNoTracking;

            query = query.Where(c => !c.Deleted);
            query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(defaultRoles).Any());
            query = query.Where(c => c.ShoppingCartItems.Any());

            if (updatedAt.HasValue)
            {
                query = query.Where(c => c.LastActivityDateUtc >= updatedAt.Value);
            }

            query = query.OrderByDescending(c => c.LastActivityDateUtc);

            var customers = new PagedList<Customer>(query, page, limit);

            return new MultipleCartResponseModel
            {
                Carts = customers.Select(PrepareCartResponseModel).ToList()
            };
        }

        [HttpGet]
        [Route("carts/{cartId:int}")]
        public SingleCartResponseModel GetCartById(int cartId)
        {
            var customer = _customerService.GetCustomerById(cartId);

            if (customer == null)
            {
                _logger.Error($"RemarketyWebApi (carts/{cartId}): unknown cartId ({cartId})");

                return new SingleCartResponseModel
                {
                    Cart = null
                };
            }

            return new SingleCartResponseModel
            {
                Cart = PrepareCartResponseModel(customer)
            };
        }

        private CartResponseModel PrepareCartResponseModel(Customer customer)
        {
            var shoppingCartItems =
                customer.ShoppingCartItems.Where(x => x.ShoppingCartType == ShoppingCartType.ShoppingCart).ToList();

            if (!shoppingCartItems.Any())
            {
                return null;
            }

            decimal orderSubTotalDiscountAmountBase;
            Discount orderSubTotalAppliedDiscount;
            decimal subTotalWithoutDiscountBase;
            decimal subTotalWithDiscountBase;
            var subTotalIncludingTax = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax &&
                                       !_taxSettings.ForceTaxExclusionFromOrderSubtotal;
            _orderTotalCalculationService.GetShoppingCartSubTotal(shoppingCartItems, subTotalIncludingTax,
                out orderSubTotalDiscountAmountBase, out orderSubTotalAppliedDiscount,
                out subTotalWithoutDiscountBase, out subTotalWithDiscountBase);
            decimal subtotalBase = subTotalWithoutDiscountBase;
            decimal subtotal = _currencyService.ConvertFromPrimaryStoreCurrency(subtotalBase,
                _workContext.WorkingCurrency);

            var totalWeight = shoppingCartItems.Sum(x => x.Product.Weight);
            var shoppingCartCreatedAt = shoppingCartItems.Min(x => x.CreatedOnUtc);
            var shoppingCartUpdatedAt = shoppingCartItems.Max(x => x.UpdatedOnUtc);
            var billingAddress = PrepareAddressModel(customer.BillingAddress);
            var shippingAddress = PrepareAddressModel(customer.ShippingAddress);
            var customerResponseModel = PrepareCustomerResponseModel(customer);
            var lineItemsModel = shoppingCartItems.Select(PrepareLineItemModel).ToList();
            List<DiscountCodeModel> discountCodes = new List<DiscountCodeModel>();

            var cartUrl = Url.Link("RemarketyRemoteCheckout",
                new
                {
                    products =
                        String.Join(",",
                            shoppingCartItems.Select(x => x.Product.FormatSku(x.AttributesXml, _productAttributeParser)))
                });


            if (orderSubTotalAppliedDiscount != null)
            {
                discountCodes.Add(
                    PrepareDiscountCodeModel(new DiscountUsageHistory { Discount = orderSubTotalAppliedDiscount }));
            }

            return new CartResponseModel
            {
                Customer = customerResponseModel,
                CreatedAt = shoppingCartCreatedAt,
                Id = customer.Id,
                UpdatedAt = shoppingCartUpdatedAt,
                Currency = GetCurrentCurrencyCode(),
                Email = customer.Email,
                TaxLines = new List<TaxLineModel>(),
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
                Note = null,
                TotalWeight = totalWeight,
                TaxesIncluded = _workContext.TaxDisplayType == TaxDisplayType.IncludingTax,
                TotalDiscounts = orderSubTotalDiscountAmountBase,
                TotalShipping = null,
                DiscountCodes = discountCodes,
                SubtotalPrice = subTotalWithoutDiscountBase,
                ShippingLines = null,
                FulfillmentStatus = null,
                LineItems = lineItemsModel,
                TotalLineItemsPrice = null,
                AcceptsMarketing = customerResponseModel?.AcceptsMarketing ?? false,
                TotalTax = null,
                TotalPrice = subtotal,
                AbandonedCheckoutUrl = cartUrl,
                CartToken = Guid.NewGuid()
            };
        }

        private LineItemModel PrepareLineItemModel(ShoppingCartItem shoppingCartItem)
        {
            var product = shoppingCartItem.Product;
            var productVendor = _vendorService.GetVendorById(product.VendorId);

            Discount scDiscount;
            decimal shoppingCartItemDiscountBase;
            decimal taxRate;
            decimal shoppingCartItemSubTotalWithDiscountBase = _taxService.GetProductPrice(product,
                _priceCalculationService.GetSubTotal(shoppingCartItem, true, out shoppingCartItemDiscountBase,
                    out scDiscount), out taxRate);
            decimal shoppingCartItemSubTotalWithDiscount =
                _currencyService.ConvertFromPrimaryStoreCurrency(shoppingCartItemSubTotalWithDiscountBase,
                    _workContext.WorkingCurrency);

            var taxLines = new List<TaxLineModel>
            {
                new TaxLineModel {Rate = taxRate}
            };

            var sku = shoppingCartItem.Product.FormatSku(shoppingCartItem.AttributesXml, _productAttributeParser);

            return new LineItemModel
            {
                Name = product.Name,
                Title = product.Name,
                TaxLines = taxLines,
                Sku = sku,
                Price = shoppingCartItemSubTotalWithDiscount,
                ProductId = product.Id,
                Vendor = productVendor?.Name,
                Taxable = !product.IsTaxExempt,
                Quantity = shoppingCartItem.Quantity,
                VariantId = null,
                VariantTitle = null
            };
        }

        private string GetCurrentCurrencyCode()
        {
            var orimaryStoreCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);

            return orimaryStoreCurrency.CurrencyCode ?? DEFAULT_CURRENSY;
        }

        #endregion
    }
}