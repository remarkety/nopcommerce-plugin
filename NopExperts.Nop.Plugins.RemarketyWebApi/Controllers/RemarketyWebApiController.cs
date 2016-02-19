﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Models.Blogs;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models;


namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    [RoutePrefix("RemarketyWebApi")]
    public class RemarketyWebApiController : ApiController
    {
        private const string DEFAULT_CURRENSY = "ILS";

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

        // settings
        private readonly EmailAccountSettings _emailAccountSettings;

        // repository (for perfomance optimization)
        private readonly IRepository<Product> _productRepository;

        public RemarketyWebApiController()
        {
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

            // TODO: add fields to settings
            return new StoreSettingsResponseModel
            {
                StoreFrontUrl = store.Url,
                Name = store.Name,
                Timezone = "Israel",
                Currency = DEFAULT_CURRENSY,
                //LogoUrl = "",
                ContactInfo = new StoreSettingsResponseModel.ContactInfoModel
                {
                    Name = store.CompanyName,
                    Email = emailAccount.Email,
                    // Phone = null
                },
                Address = new StoreSettingsResponseModel.AddressModel
                {
                    //Address1 = "",
                    //Address2 = "",
                    City = "",
                    Country = "Israel",
                    //State = "",
                    //Zip = ""
                },
                OrderStatuses = enumList.ToList()
            };
        }

        #endregion

        #region /products


        [HttpGet]
        [Route("products")]
        public MultipleProductResponseModel Products(int page = 0, int limit = int.MaxValue, [FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
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
            var productVendor = _vendorService.GetVendorById(product.Id);

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

        private ProductResponseModel.OptionModel PrepareProductOptionsModel(ProductAttributeMapping productAttributeMapping)
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

        private ProductResponseModel.ProductVariantModel PrepareProductVariantModel(ProductAttributeCombination productAttributeCombination)
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
                Currency = DEFAULT_CURRENSY,
                FullfilmentService = null,
                InventoryQuantity = productAttributeCombination.StockQuantity,
                Price = productAttributeCombination.OverriddenPrice ?? product.Price,
                RequiresShipping = product.IsShipEnabled,
                Sku = productAttributeCombination.Sku,
                Title = null,
                UpdatedAt = null,
                Taxable = product.IsTaxExempt,
                Weight = product.Weight
            };
        }

        #endregion

        #endregion

        #region /customers

        [HttpGet]
        [Route("customers")]
        public MultipleCustomerResponseModel Customers(int page = 0, int limit = int.MaxValue, [FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var defaultRoles = new[]
           {
                _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered).Id
            };

            var customers = _customerService.GetAllCustomers(updatedAt, customerRoleIds: defaultRoles, pageIndex: page, pageSize: limit);

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
            var customerGroups = customer.CustomerRoles.Select(PrepareCustomerGroupModel).ToList();
            var defaultAddress = PrepareCustomerAddressModel(customer);

            var newsletterSubscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, _storeContext.CurrentStore.Id);

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
                DefaultAddress = defaultAddress,
                FirstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName),
                Gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender),
                Groups = customerGroups,
                Info = new object(),
                LastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName),
                VerifiedEmail = null
            };
        }

        private CustomerResponseModel.CustomerAddressModel PrepareCustomerAddressModel(Customer customer)
        {
            var defaultAddress = customer.ShippingAddress ?? customer.Addresses.FirstOrDefault();

            if (defaultAddress == null)
            {
                return null;
            }

            return new CustomerResponseModel.CustomerAddressModel
            {
                Country = defaultAddress.Country?.Name,
                Phone = defaultAddress.PhoneNumber,
                Zip = defaultAddress.ZipPostalCode,
                CountryCode = defaultAddress.Country?.ThreeLetterIsoCode,
                ProvinceCode = defaultAddress.StateProvince?.Abbreviation
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
        [Route("orders/count")]
        public CountResponseModel OrdersConut([FromUri(Name = "updated_at_min")] DateTime? updatedAt = null)
        {
            var ordersCount = _orderService.SearchOrders(createdFromUtc: updatedAt).Count;

            return new CountResponseModel
            {
                Count = ordersCount
            };
        }

        #endregion
    }
}