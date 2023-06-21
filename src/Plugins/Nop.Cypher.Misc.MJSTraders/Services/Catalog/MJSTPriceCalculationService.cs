using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.Catalog
{
    public partial class MJSTPriceCalculationService : ShoppingCartService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly IAclService _aclService;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly ICheckoutAttributeService _checkoutAttributeService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IDateRangeService _dateRangeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductService _productService;
        private readonly IRepository<ShoppingCartItem> _sciRepository;
        private readonly IShippingService _shippingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly OrderSettings _orderSettings;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        #endregion

        #region Ctor

        public MJSTPriceCalculationService(CatalogSettings catalogSettings,
              IAclService aclService,
              IActionContextAccessor actionContextAccessor,
              ICheckoutAttributeParser checkoutAttributeParser,
              ICheckoutAttributeService checkoutAttributeService,
              ICurrencyService currencyService,
              ICustomerService customerService,
              IDateRangeService dateRangeService,
              IDateTimeHelper dateTimeHelper,
              IGenericAttributeService genericAttributeService,
              ILocalizationService localizationService,
              IPermissionService permissionService,
              IPriceCalculationService priceCalculationService,
              IPriceFormatter priceFormatter,
              IProductAttributeParser productAttributeParser,
              IProductAttributeService productAttributeService,
              IProductService productService,
              IRepository<ShoppingCartItem> sciRepository,
              IShippingService shippingService,
              IStaticCacheManager staticCacheManager,
              IStoreContext storeContext,
              IStoreService storeService,
              IStoreMappingService storeMappingService,
              IUrlHelperFactory urlHelperFactory,
              IUrlRecordService urlRecordService,
              IWorkContext workContext,
              OrderSettings orderSettings,
              ShoppingCartSettings shoppingCartSettings) : base (catalogSettings,
                aclService,
                actionContextAccessor,
                checkoutAttributeParser,
                checkoutAttributeService,
                currencyService,
                customerService,
                dateRangeService,
                dateTimeHelper,
                genericAttributeService,
                localizationService,
                permissionService,
                priceCalculationService,
                priceFormatter,
                productAttributeParser,
                productAttributeService,
                productService,
                sciRepository,
                shippingService,
                staticCacheManager,
                storeContext,
                storeService,
                storeMappingService,
                urlHelperFactory,
                urlRecordService,
                workContext,
                orderSettings,
                shoppingCartSettings)
        {
            _catalogSettings = catalogSettings;
            _aclService = aclService;
            _actionContextAccessor = actionContextAccessor;
            _checkoutAttributeParser = checkoutAttributeParser;
            _checkoutAttributeService = checkoutAttributeService;
            _currencyService = currencyService;
            _customerService = customerService;
            _dateRangeService = dateRangeService;
            _dateTimeHelper = dateTimeHelper;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _priceCalculationService = priceCalculationService;
            _priceFormatter = priceFormatter;
            _productAttributeParser = productAttributeParser;
            _productAttributeService = productAttributeService;
            _productService = productService;
            _sciRepository = sciRepository;
            _shippingService = shippingService;
            _staticCacheManager = staticCacheManager;
            _storeContext = storeContext;
            _storeService = storeService;
            _storeMappingService = storeMappingService;
            _urlHelperFactory = urlHelperFactory;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
            _orderSettings = orderSettings;
            _shoppingCartSettings = shoppingCartSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the shopping cart unit price (one item)
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customer">Customer</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Product attributes (XML format)</param>
        /// <param name="customerEnteredPrice">Customer entered price (if specified)</param>
        /// <param name="rentalStartDate">Rental start date (null for not rental products)</param>
        /// <param name="rentalEndDate">Rental end date (null for not rental products)</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
        /// <param name="discountAmount">Applied discount amount</param>
        /// <param name="appliedDiscounts">Applied discounts</param>
        /// <returns>Shopping cart unit price (one item)</returns>
        public override async Task<(decimal unitPrice, decimal discountAmount, List<Discount> appliedDiscounts)> GetUnitPriceAsync(Product product,
              Customer customer,
              Store store,
              ShoppingCartType shoppingCartType,
              int quantity,
              string attributesXml,
              decimal customerEnteredPrice,
              DateTime? rentalStartDate, DateTime? rentalEndDate,
              bool includeDiscounts)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var discountAmount = decimal.Zero;
            var appliedDiscounts = new List<Discount>();

            decimal finalPrice;

            var combination = await _productAttributeParser.FindProductAttributeCombinationAsync(product, attributesXml);
            if (combination?.OverriddenPrice.HasValue ?? false)
            {
                (_, finalPrice, discountAmount, appliedDiscounts) = await _priceCalculationService.GetFinalPriceAsync(product,
                       customer,
                       store,
                       combination.OverriddenPrice.Value,
                       decimal.Zero,
                       includeDiscounts,
                       quantity,
                       product.IsRental ? rentalStartDate : null,
                       product.IsRental ? rentalEndDate : null);
            }
            else
            {
                //summarize price of all attributes
                var attributesTotalPrice = decimal.Zero;
                var attributeValues = await _productAttributeParser.ParseProductAttributeValuesAsync(attributesXml);
                if (attributeValues != null)
                {
                    foreach (var attributeValue in attributeValues)
                    {
                        attributesTotalPrice += await _priceCalculationService.GetProductAttributeValuePriceAdjustmentAsync(product, attributeValue, customer,store, product.CustomerEntersPrice ? (decimal?)customerEnteredPrice : null);
                    }
                }

                //get price of a product (with previously calculated price of all attributes)
                bool isCustomerEntersPrice = false;
                isCustomerEntersPrice = product.CustomerEntersPrice;

                // check impersonate mode
                if (_workContext.OriginalCustomerIfImpersonated != null)
                {
                    isCustomerEntersPrice = true;
                }

                if (isCustomerEntersPrice)
                {
                    finalPrice = customerEnteredPrice;
                }
                else
                {
                    int qty;
                    if (_shoppingCartSettings.GroupTierPricesForDistinctShoppingCartItems)
                    {
                        //the same products with distinct product attributes could be stored as distinct "ShoppingCartItem" records
                        //so let's find how many of the current products are in the cart
                        qty = (await GetShoppingCartAsync(customer, shoppingCartType: shoppingCartType, productId: product.Id))
                            .Sum(x => x.Quantity);

                        if (qty == 0)
                        {
                            qty = quantity;
                        }
                    }
                    else
                    {
                        qty = quantity;
                    }

                    (_, finalPrice, discountAmount, appliedDiscounts) = await _priceCalculationService.GetFinalPriceAsync(product,
                        customer,
                        store,
                        attributesTotalPrice,
                        includeDiscounts,
                        qty,
                        product.IsRental ? rentalStartDate : null,
                        product.IsRental ? rentalEndDate : null);
                }
            }

            //rounding
            if (_shoppingCartSettings.RoundPricesDuringCalculation)
                finalPrice = await _priceCalculationService.RoundPriceAsync(finalPrice);

            return (finalPrice, discountAmount, appliedDiscounts);
        }

        #endregion
    }
}