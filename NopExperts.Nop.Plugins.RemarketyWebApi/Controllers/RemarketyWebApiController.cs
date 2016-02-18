using System;
using System.Linq;
using System.Web.Http;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Messages;
using Nop.Services.Orders;
using NopExperts.Nop.Plugins.RemarketyWebApi.Models;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Controllers
{
    [RoutePrefix("RemarketyWebApi")]
    public class RemarketyWebApiController : ApiController
    {
        // services
        private readonly IStoreContext _storeContext;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        // settings
        private readonly EmailAccountSettings _emailAccountSettings;

        // repository (for perfomance optimization)
        private readonly IRepository<Product> _productRepository;

        public RemarketyWebApiController()
        {
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
                Currency = "ILS",
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
        [Route("products/{productId:int}")]
        public SingleProductResponseModel ProductById(int productId)
        {
            return new SingleProductResponseModel
            {
                Product = new ProductResponseModel
                {
                    Id = productId
                }
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

        #endregion

        #region /customers

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