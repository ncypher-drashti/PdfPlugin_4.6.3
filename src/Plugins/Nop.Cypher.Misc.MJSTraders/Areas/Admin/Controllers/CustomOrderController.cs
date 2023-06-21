using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.Orders;
using Nop.Cypher.Misc.MJSTraders.Services.Orders;
using Nop.Cypher.Misc.MJSTraders.Services.PDF;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Controllers
{
    public partial class CustomOrderController : OrderController
    {
        #region Fields

        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressService _addressService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDownloadService _downloadService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IExportManager _exportManager;
        private readonly IGiftCardService _giftCardService;
        private readonly IImportManager _importManager;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPdfService _pdfService;
        private readonly IPermissionService _permissionService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductService _productService;
        private readonly IShipmentService _shipmentService;
        private readonly IShippingService _shippingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly OrderSettings _orderSettings;


        private readonly IStoreService _storeService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly MJSTradersSettings _mjsTradersSettings;
        private readonly ICustomOrderService _customOrderService;
        private readonly AddressSettings _addressSettings;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly CatalogSettings _catalogSettings;
        private readonly ICountryService _countryService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly OrderSettings _ordersetting;
        private readonly IPODocumentService _poDocumentService;
        private readonly IMJSTPdfService _mjstPdfService;

        #endregion

        #region Ctor

        public CustomOrderController(IAddressAttributeParser addressAttributeParser,
            IAddressService addressService,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            IExportManager exportManager,
            IGiftCardService giftCardService,
            IImportManager importManager,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IOrderModelFactory orderModelFactory,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IPaymentService paymentService,
            IPdfService pdfService,
            IPermissionService permissionService,
            IPriceCalculationService priceCalculationService,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeParser productAttributeParser,
            IProductAttributeService productAttributeService,
            IProductService productService,
            IShipmentService shipmentService,
            IShippingService shippingService,
            IShoppingCartService shoppingCartService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            OrderSettings orderSettings,

            IStoreService storeService,
            IPriceFormatter priceFormatter,
            MJSTradersSettings mjsTradersSettings,
            ICustomOrderService customOrderService,
            AddressSettings addressSettings,
            IBaseAdminModelFactory baseAdminModelFactory,
            CatalogSettings catalogSettings,
            ICountryService countryService,
            IPaymentPluginManager paymentPluginManager,
            IGenericAttributeService genericAttributeService,
            IMJSTPdfService mjstPdfService,
            IPODocumentService poDocumentService) : base(  addressAttributeParser,
              addressService,
              customerActivityService,
              customerService,
              dateTimeHelper,
              downloadService,
              encryptionService,
              eventPublisher,
              exportManager,
              giftCardService,
              importManager,
              localizationService,
              notificationService,
              orderModelFactory,
              orderProcessingService,
              orderService,
              paymentService,
              pdfService,
              permissionService,
              priceCalculationService,
              productAttributeFormatter,
              productAttributeParser,
              productAttributeService,
              productService,
              shipmentService,
              shippingService,
              shoppingCartService,
              storeContext,
              workContext,
              workflowMessageService,
              orderSettings
            )
        {
            _addressAttributeParser = addressAttributeParser;
            _addressService = addressService;
            _customerActivityService = customerActivityService;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _downloadService = downloadService;
            _encryptionService = encryptionService;
            _eventPublisher = eventPublisher;
            _exportManager = exportManager;
            _giftCardService = giftCardService;
            _importManager = importManager;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _orderModelFactory = orderModelFactory;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _paymentService = paymentService;
            _pdfService = pdfService;
            _permissionService = permissionService;
            _priceCalculationService = priceCalculationService;
            _productAttributeFormatter = productAttributeFormatter;
            _productAttributeParser = productAttributeParser;
            _productAttributeService = productAttributeService;
            _productService = productService;
            _shipmentService = shipmentService;
            _shippingService = shippingService;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
            _workContext = workContext;
            _workflowMessageService = workflowMessageService;
            _orderSettings = orderSettings;

            _storeService = storeService;
            _priceFormatter = priceFormatter;
            _mjsTradersSettings = mjsTradersSettings;
            _customOrderService = customOrderService;
            _addressSettings = addressSettings;
            _baseAdminModelFactory = baseAdminModelFactory;
            _catalogSettings = catalogSettings;
            _countryService = countryService;
            _paymentPluginManager = paymentPluginManager;
            _genericAttributeService = genericAttributeService;
            _poDocumentService = poDocumentService;
            _mjstPdfService = mjstPdfService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare paged order list model
        /// </summary>
        /// <param name="searchModel">Order search model</param>
        /// <returns>Order list model</returns>
        public virtual async Task<CustomOrderListModel> PrepareOrderListModelAsync(CustomOrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter orders
            var orderStatusIds = (searchModel.OrderStatusIds?.Contains(0) ?? true) ? null : searchModel.OrderStatusIds.ToList();
            var paymentStatusIds = (searchModel.PaymentStatusIds?.Contains(0) ?? true) ? null : searchModel.PaymentStatusIds.ToList();
            var shippingStatusIds = (searchModel.ShippingStatusIds?.Contains(0) ?? true) ? null : searchModel.ShippingStatusIds.ToList();

            var vendor = await _workContext.GetCurrentVendorAsync();
            if (vendor != null)
                searchModel.VendorId = vendor.Id;
            var startDateValue = !searchModel.StartDate.HasValue ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(searchModel.StartDate.Value, await _dateTimeHelper.GetCurrentTimeZoneAsync());
            var endDateValue = !searchModel.EndDate.HasValue ? null
                : (DateTime?)_dateTimeHelper.ConvertToUtcTime(searchModel.EndDate.Value, await _dateTimeHelper.GetCurrentTimeZoneAsync()).AddDays(1);
            var product = await _productService.GetProductByIdAsync(searchModel.ProductId);
            var filterByProductId = product != null && (vendor == null || product.VendorId == vendor.Id)
                ? searchModel.ProductId : 0;


            int poCusomerRolId = _mjsTradersSettings.POCustomerId;
            var customerRoleIds = new List<int>();

            bool isAll = false;
            bool isOnlyCoperate = false;
            if (searchModel.OrderType == "onlineorders")
            {
            }
            else if (searchModel.OrderType == "corporateorders")
            {
                isOnlyCoperate = true;
            }
            else
            {
                isAll = true;
            }


            //get orders
            var orders = await _customOrderService.SearchOrdersAsync(storeId: searchModel.StoreId,
                vendorId: searchModel.VendorId,
                productId: filterByProductId,
                warehouseId: searchModel.WarehouseId,
                paymentMethodSystemName: searchModel.PaymentMethodSystemName,
                createdFromUtc: startDateValue,
                createdToUtc: endDateValue,
                osIds: orderStatusIds,
                psIds: paymentStatusIds,
                ssIds: shippingStatusIds,
                billingPhone: searchModel.BillingPhone,
                billingEmail: searchModel.BillingEmail,
                billingLastName: searchModel.BillingLastName,
                billingCountryId: searchModel.BillingCountryId,
                orderNotes: searchModel.OrderNotes,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                isAll: isAll, isOnlyCoperate: isOnlyCoperate);
            //  customerRoleIds: customerRoleIds.Any()? customerRoleIds : null);

            //prepare list model
            var model = await new CustomOrderListModel().PrepareToGridAsync(searchModel, orders, () =>
            {
                //fill in model values from the entity
                return orders.SelectAwait(async order =>
                {
                    var billingAddress = await _addressService.GetAddressByIdAsync(order.BillingAddressId);

                    //fill in model values from the entity
                    var orderModel = new CustomOrderModel
                    {
                        Id = order.Id,
                        OrderStatusId = order.OrderStatusId,
                        PaymentStatusId = order.PaymentStatusId,
                        ShippingStatusId = order.ShippingStatusId,
                        CustomerEmail = billingAddress.Email,
                        CustomerFullName = $"{billingAddress.FirstName} {billingAddress.LastName}",
                        CustomerId = order.CustomerId,
                        CustomOrderNumber = order.CustomOrderNumber
                    };

                    //convert dates to the user time
                    orderModel.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(order.CreatedOnUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    orderModel.StoreName = (await _storeService.GetStoreByIdAsync(order.StoreId))?.Name ?? "Deleted";
                    orderModel.OrderStatus = await _localizationService.GetLocalizedEnumAsync(order.OrderStatus);
                    orderModel.PaymentStatus = await _localizationService.GetLocalizedEnumAsync(order.PaymentStatus);
                    orderModel.ShippingStatus = await _localizationService.GetLocalizedEnumAsync(order.ShippingStatus);
                    orderModel.OrderTotal = await _priceFormatter.FormatPriceAsync(order.OrderTotal, true, false);

                    // order po number
                    var poDocuments = await _poDocumentService.GetPODocumentByOrderIdAsync(order.Id);
                    if (poDocuments != null)
                    {
                        orderModel.OrderPONumber = poDocuments.Title1;
                    }

                    return orderModel;
                });
            });

            return model;
        }


        /// <summary>
        /// Prepare order search model
        /// </summary>
        /// <param name="searchModel">Order search model</param>
        /// <returns>Order search model</returns>
        public virtual async Task<CustomOrderSearchModel> PrepareOrderSearchModelAsync(CustomOrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.IsLoggedInAsVendor = await _workContext.GetCurrentVendorAsync() != null;
            searchModel.BillingPhoneEnabled = _addressSettings.PhoneEnabled;

            //prepare available order, payment and shipping statuses
            await _baseAdminModelFactory.PrepareOrderStatusesAsync(searchModel.AvailableOrderStatuses);
            if (searchModel.AvailableOrderStatuses.Any())
            {
                if (searchModel.OrderStatusIds?.Any() ?? false)
                {
                    var ids = searchModel.OrderStatusIds.Select(id => id.ToString());
                    searchModel.AvailableOrderStatuses.Where(statusItem => ids.Contains(statusItem.Value)).ToList()
                        .ForEach(statusItem => statusItem.Selected = true);
                }
                else
                    searchModel.AvailableOrderStatuses.FirstOrDefault().Selected = true;
            }

            await _baseAdminModelFactory.PreparePaymentStatusesAsync(searchModel.AvailablePaymentStatuses);
            if (searchModel.AvailablePaymentStatuses.Any())
            {
                if (searchModel.PaymentStatusIds?.Any() ?? false)
                {
                    var ids = searchModel.PaymentStatusIds.Select(id => id.ToString());
                    searchModel.AvailablePaymentStatuses.Where(statusItem => ids.Contains(statusItem.Value)).ToList()
                        .ForEach(statusItem => statusItem.Selected = true);
                }
                else
                    searchModel.AvailablePaymentStatuses.FirstOrDefault().Selected = true;
            }

            await _baseAdminModelFactory.PrepareShippingStatusesAsync(searchModel.AvailableShippingStatuses);
            if (searchModel.AvailableShippingStatuses.Any())
            {
                if (searchModel.ShippingStatusIds?.Any() ?? false)
                {
                    var ids = searchModel.ShippingStatusIds.Select(id => id.ToString());
                    searchModel.AvailableShippingStatuses.Where(statusItem => ids.Contains(statusItem.Value)).ToList()
                        .ForEach(statusItem => statusItem.Selected = true);
                }
                else
                    searchModel.AvailableShippingStatuses.FirstOrDefault().Selected = true;
            }

            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            //prepare available vendors
            await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

            //prepare available warehouses
            await _baseAdminModelFactory.PrepareWarehousesAsync(searchModel.AvailableWarehouses);

            //prepare available payment methods
            searchModel.AvailablePaymentMethods = (await _paymentPluginManager.LoadAllPluginsAsync()).Select(method =>
                 new SelectListItem { Text = method.PluginDescriptor.FriendlyName, Value = method.PluginDescriptor.SystemName }).ToList();
            searchModel.AvailablePaymentMethods.Insert(0, new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = string.Empty });

            //prepare available billing countries
            searchModel.AvailableCountries = (await _countryService.GetAllCountriesForBillingAsync(showHidden: true))
                .Select(country => new SelectListItem { Text = country.Name, Value = country.Id.ToString() }).ToList();
            searchModel.AvailableCountries.Insert(0, new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });

            //prepare grid
            searchModel.SetGridPageSize();

            searchModel.HideStoresList = _catalogSettings.IgnoreStoreLimitations || searchModel.AvailableStores.SelectionIsNotPossible();

            return searchModel;
        }

        #endregion

        #region Order list

        public override IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public override async Task<IActionResult> List(List<int> orderStatuses = null, List<int> paymentStatuses = null, List<int> shippingStatuses = null)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return AccessDeniedView();

            if (orderStatuses == null)
            {
                orderStatuses = new List<int>
                {
                    (Int32)OrderStatus.Pending,
                    (Int32)OrderStatus.Processing
                };
            }

            //prepare model
            var model = await PrepareOrderSearchModelAsync(new CustomOrderSearchModel
            {
                OrderStatusIds = orderStatuses,
                PaymentStatusIds = paymentStatuses,
                ShippingStatusIds = shippingStatuses
            });

            // get last selected order type
            string orderType = await _genericAttributeService.GetAttributeAsync<string>(await _workContext.GetCurrentCustomerAsync(), MJSTradersUtilities.LastSelectedOrderTypeFilter, _storeContext.GetCurrentStore().Id);

            if (!string.IsNullOrWhiteSpace(orderType))
                model.OrderType = orderType;
            else
                model.OrderType = "all";

            // prepare order type ddl
            model.AvailableOrderTypes.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Orders.List.OrderType.all"), Value = "all", Selected = model.OrderType == "all" });
            model.AvailableOrderTypes.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Orders.List.OrderType.OnlineOrders"), Value = "onlineorders", Selected = model.OrderType == "onlineorders" });
            model.AvailableOrderTypes.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Orders.List.OrderType.corporateOrders"), Value = "corporateorders", Selected = model.OrderType == "corporateorders" });


            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CustomOrderList(CustomOrderSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
                return await AccessDeniedDataTablesJson();

            // save order type value
            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(), MJSTradersUtilities.LastSelectedOrderTypeFilter, searchModel.OrderType, _storeContext.GetCurrentStore().Id);

            //prepare model
            var model =await PrepareOrderListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Save check number

        public virtual async Task<IActionResult> SaveCheckNumber(CheckNumberModel model)
        {
            // get order
            var order = await _orderService.GetOrderByIdAsync(model.OrderId);

            if (!string.IsNullOrWhiteSpace(model.CheckNumber))
            {
                // save check number
                await _genericAttributeService.SaveAttributeAsync(order, MJSTradersUtilities.CheckNumber, model.CheckNumber, _storeContext.GetCurrentStore().Id);

                return Json(new { status = true });
            }
            else
                return Json(new { status = false, message = await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.CheckMoneyOrder.Order.required.CheckNumber") });
        }

        #endregion

        #region Shipment page related changes 

        #region print cash memo

        //public virtual async Task<IActionResult> PrintCashMemo(int id)
        //{
        //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageOrders))
        //        return AccessDeniedView();

        //    var ordershipment = await _shipmentService.GetShipmentByIdAsync(id);
        //    if (ordershipment == null)
        //    {
        //        _notificationService.ErrorNotification("Nop.Cypher.Misc.MJSTraders.Shipment.CashMemo.Error.ShipmentNotFound");
        //        return RedirectToAction("ShipmentList", "Order");
        //    }

        //    byte[] bytes;
        //    using (var stream = new MemoryStream())
        //    {
        //        await _mjstPdfService.PrintCashMemoPDFAsync(stream, ordershipment, _ordersetting.GeneratePdfInvoiceInCustomerLanguage ? 0 : (await _workContext.GetWorkingLanguageAsync()).Id);
        //        bytes = stream.ToArray();
        //    }

        //    return File(bytes, MimeTypes.ApplicationPdf, $"cash_memo_{id}.pdf");
        //}


        #endregion


        #endregion
    }
}