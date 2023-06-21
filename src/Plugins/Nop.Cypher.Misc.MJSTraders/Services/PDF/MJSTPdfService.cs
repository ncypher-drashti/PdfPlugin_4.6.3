using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Core.Infrastructure;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Common.Pdf;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Nop.Cypher.Misc.MJSTraders.Services.PDF
{
    public partial class MJSTPdfService : PdfService, IMJSTPdfService
    {
        #region Defalut

        public static string FontDirectory => "~/Plugins/Cypher.MJSTraders/App_Data/Pdf/";
        public static string BoldFontFileName => "OpenSans-Bold.ttf";
        public static string RegularFontFileName => "OpenSans-Regular.ttf";
        public static string SemiBoldFontFileName => "OpenSans-SemiBold.ttf";

        #endregion

        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGiftCardService _giftCardService;
        private readonly IHtmlFormatter _htmlFormatter;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMeasureService _measureService;
        private readonly INopFileProvider _fileProvider;
        private readonly IOrderService _orderService;
        private readonly IPaymentPluginManager _paymentPluginManager;
        private readonly IPaymentService _paymentService;
        private readonly IPictureService _pictureService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductService _productService;
        private readonly IRewardPointService _rewardPointService;
        private readonly ISettingService _settingService;
        private readonly IShipmentService _shipmentService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly MeasureSettings _measureSettings;
        private readonly TaxSettings _taxSettings;
        private readonly VendorSettings _vendorSettings;

        private readonly IPODocumentService _pODocumentService = EngineContext.Current.Resolve<IPODocumentService>();
        private readonly ICustomerService _customerService = EngineContext.Current.Resolve<ICustomerService>();

        #endregion

        #region Ctor

        public MJSTPdfService(AddressSettings addressSettings,
            CatalogSettings catalogSettings,
            CurrencySettings currencySettings,
            IAddressAttributeFormatter addressAttributeFormatter,
            IAddressService addressService,
            ICountryService countryService,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            IGiftCardService giftCardService,
            IHtmlFormatter htmlFormatter,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IMeasureService measureService,
            INopFileProvider fileProvider,
            IOrderService orderService,
            IPaymentPluginManager paymentPluginManager,
            IPaymentService paymentService,
            IPictureService pictureService,
            IPriceFormatter priceFormatter,
            IProductService productService,
            IRewardPointService rewardPointService,
            ISettingService settingService,
            IShipmentService shipmentService,
            IStateProvinceService stateProvinceService,
            IStoreContext storeContext,
            IStoreService storeService,
            IVendorService vendorService,
            IWorkContext workContext,
            MeasureSettings measureSettings,
            TaxSettings taxSettings,
            VendorSettings vendorSettings) : base(addressSettings,
              catalogSettings,
              currencySettings,
              addressAttributeFormatter,
              addressService,
              countryService,
              currencyService,
              dateTimeHelper,
              giftCardService,
              htmlFormatter,
              languageService,
              localizationService,
              measureService,
              fileProvider,
              orderService,
              paymentPluginManager,
              paymentService,
              pictureService,
              priceFormatter,
              productService,
              rewardPointService,
              settingService,
              shipmentService,
              stateProvinceService,
              storeContext,
              storeService,
              vendorService,
              workContext,
              measureSettings,
              taxSettings,
              vendorSettings)
        {
            _addressSettings = addressSettings;
            _addressService = addressService;
            _catalogSettings = catalogSettings;
            _countryService = countryService;
            _currencySettings = currencySettings;
            _addressAttributeFormatter = addressAttributeFormatter;
            _currencyService = currencyService;
            _dateTimeHelper = dateTimeHelper;
            _giftCardService = giftCardService;
            _htmlFormatter = htmlFormatter;
            _languageService = languageService;
            _localizationService = localizationService;
            _measureService = measureService;
            _fileProvider = fileProvider;
            _orderService = orderService;
            _paymentPluginManager = paymentPluginManager;
            _paymentService = paymentService;
            _pictureService = pictureService;
            _priceFormatter = priceFormatter;
            _productService = productService;
            _rewardPointService = rewardPointService;
            _settingService = settingService;
            _shipmentService = shipmentService;
            _storeContext = storeContext;
            _stateProvinceService = stateProvinceService;
            _storeService = storeService;
            _vendorService = vendorService;
            _workContext = workContext;
            _measureSettings = measureSettings;
            _taxSettings = taxSettings;
            _vendorSettings = vendorSettings;
        }

        #endregion

        #region Override

        /// <summary>
        /// Write PDF invoice to the specified stream
        /// </summary>
        /// <param name="stream">Stream to save PDF</param>
        /// <param name="order">Order</param>
        /// <param name="language">Language; null to use a language used when placing an order</param>
        /// <param name="store">Store</param>
        /// <param name="vendor">Vendor to limit products; null to print all products. If specified, then totals won't be printed</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// </returns>
        public override async Task PrintOrderToPdfAsync(Stream stream, Order order, Language language = null, Store store = null, Vendor vendor = null)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //store info
            store ??= await _storeContext.GetCurrentStoreAsync();

            var orderStore = order.StoreId == 0 || order.StoreId == store?.Id ?
                store : await _storeService.GetStoreByIdAsync(order.StoreId);

            //language info
            language ??= await _languageService.GetLanguageByIdAsync(order.CustomerLanguageId);

            if (language?.Published != true)
                language = await _workContext.GetWorkingLanguageAsync();

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = await _settingService.LoadSettingAsync<PdfSettings>(orderStore.Id);

            byte[] logo = null;
            var logoPicture = await _pictureService.GetPictureByIdAsync(pdfSettingsByStore.LogoPictureId);
            if (logoPicture != null)
            {
                var logoFilePath = await _pictureService.GetThumbLocalPathAsync(logoPicture, 0, false);

                if (logoPicture.MimeType == MimeTypes.ImageSvg)
                {
                    logo = await _pictureService.ConvertSvgToPngAsync(logoFilePath);
                }
                else
                {
                    logo = await _fileProvider.ReadAllBytesAsync(logoFilePath);
                }
            }

            var date = await _dateTimeHelper.ConvertToUserTimeAsync(order.CreatedOnUtc, DateTimeKind.Utc);

            //a vendor should have access only to products
            var orderItems = await _orderService.GetOrderItemsAsync(order.Id, vendorId: vendor?.Id ?? 0);

            var column1Lines = string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                new List<string>()
                : pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

            var column2Lines = string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                new List<string>()
                : pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();



            var source = new InvoiceSource()
            {
                StoreUrl = orderStore.Url?.Trim('/'),
                Language = language,
                FontFamily = pdfSettingsByStore.FontFamily,
                OrderDateUser = date,
                LogoData = logo,
                OrderNumberText = order.CustomOrderNumber,
                PageSize = pdfSettingsByStore.LetterPageSizeEnabled ? PageSizes.Letter : PageSizes.A4,
                BillingAddress = await GetBillingAddressAsync(vendor, language, order),
                ShippingAddress = await GetShippingAddressAsync(language, order),
                Products = await GetOrderProductItemsAsync(order, orderItems, language),
                ShowSkuInProductList = _catalogSettings.ShowSkuOnProductDetailsPage,
                ShowVendorInProductList = _vendorSettings.ShowVendorOnOrderDetailsPage,
                CheckoutAttributes = vendor is null ? order.CheckoutAttributeDescription : string.Empty, //vendors cannot see checkout attributes
                Totals = vendor is null ? await GetTotalsAsync(language, order) : new(), //vendors cannot see totals
                OrderNotes = await GetOrderNotesAsync(pdfSettingsByStore, order, language),
                FooterTextColumn1 = column1Lines,
                FooterTextColumn2 = column2Lines
            };

            // check order customer is po or not
            if (await (await _customerService.GetCustomerByIdAsync(order.CustomerId)).IsPOCustomerAsync())
            {
                var purchaseOrderDocs = await _pODocumentService.GetPODocumentByOrderIdAsync(order.Id);
                if (purchaseOrderDocs != null)
                {
                    source.POOrder = purchaseOrderDocs.Title1;
                }
            }

            await using var pdfStream = new MemoryStream();
            new InvoiceDocument(source, _localizationService)
                .GeneratePdf(pdfStream);

            pdfStream.Position = 0;
            await pdfStream.CopyToAsync(stream);
        }

        /// <summary>
        /// Write ZIP archive with invoices to the specified stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="language">Language; null to use a language used when placing an order</param>
        /// <param name="vendor">Vendor to limit products; null to print all products. If specified, then totals won't be printed</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task PrintOrdersToPdfAsync(Stream stream, IList<Order> orders, Language language = null, Vendor vendor = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (orders == null)
                throw new ArgumentNullException(nameof(orders));

            var currentStore = await _storeContext.GetCurrentStoreAsync();

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            foreach (var order in orders)
            {
                var entryName = string.Format("{0} {1}", await _localizationService.GetResourceAsync("Pdf.Order"), order.CustomOrderNumber);

                await using var fileStreamInZip = archive.CreateEntry($"{entryName}.pdf").Open();
                await using var pdfStream = new MemoryStream();
                await PrintOrderToPdfAsync(pdfStream, order, language, currentStore, vendor);
                pdfStream.Position = 0;
                await pdfStream.CopyToAsync(fileStreamInZip);
            }
        }

        #endregion

        #region method

        /// <summary>
        /// Print an sales quotation to PDF
        /// </summary>
        /// <param name="salesQuotation">Sales quotation</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        /// <param name="vendorId">Vendor identifier to limit products; 0 to print all products. If specified, then totals won't be printed</param>
        /// <returns>A path of generated file</returns>
        public virtual async Task PrintSalesQuotationToPdfAsync(Stream stream, SalesQuotation salesQuotation, Language language = null, Vendor vendor = null)
        {
            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));
        
            //language info
            language ??= await _languageService.GetLanguageByIdAsync(salesQuotation.Id);

            if (language?.Published != true)
                language = await _workContext.GetWorkingLanguageAsync();

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            var pdfSettingsByStore = await _settingService.LoadSettingAsync<PdfSettings>(salesQuotation.Id);

            byte[] logo = null;
            var logoPicture = await _pictureService.GetPictureByIdAsync(pdfSettingsByStore.LogoPictureId);
            if (logoPicture != null)
            {
                var logoFilePath = await _pictureService.GetThumbLocalPathAsync(logoPicture, 0, false);

                if (logoPicture.MimeType == MimeTypes.ImageSvg)
                {
                    logo = await _pictureService.ConvertSvgToPngAsync(logoFilePath);
                }
                else
                {
                    logo = await _fileProvider.ReadAllBytesAsync(logoFilePath);
                }
            }

            var date = await _dateTimeHelper.ConvertToUserTimeAsync(salesQuotation.InquiryDate, DateTimeKind.Utc);

            //a vendor should have access only to products
            var orderItems = await _orderService.GetOrderItemsAsync(salesQuotation.Id, vendorId: vendor?.Id ?? 0);

            var column1Lines = string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) ?
                new List<string>()
                : pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

            var column2Lines = string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2) ?
                new List<string>()
                : pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();



            var source = new InvoiceSource()
            {
                Language = language,
                FontFamily = pdfSettingsByStore.FontFamily,
                OrderDateUser = date,
                LogoData = logo,
                SalesQuotationNumberText = "SALES QUOTE",
                SalesQuotationCustomerId = salesQuotation.SalesQuotationCustomerId,
                PageSize = pdfSettingsByStore.LetterPageSizeEnabled ? PageSizes.Letter : PageSizes.A4,
                ShowSkuInProductList = _catalogSettings.ShowSkuOnProductDetailsPage,
                ShowVendorInProductList = _vendorSettings.ShowVendorOnOrderDetailsPage,
                FooterTextColumn1 = column1Lines,
                FooterTextColumn2 = column2Lines
            };

            await using var pdfStream = new MemoryStream();
            new InvoiceDocument(source, _localizationService)
                .GeneratePdf(pdfStream);

            pdfStream.Position = 0;
            await pdfStream.CopyToAsync(stream);
      
        }

        /// <summary>
        /// Print sales quotations to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="salesQuotations">Sales quotations</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        /// <param name="vendorId">Vendor identifier to limit products; 0 to print all products. If specified, then totals won't be printed</param>
        public virtual async Task PrintSalesQuotationsToPdfAsync(Stream stream, IList<SalesQuotation> salesQuotations, Language language = null, Vendor vendor = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (salesQuotations == null)
                throw new ArgumentNullException(nameof(salesQuotations));

            var currentStore = await _storeContext.GetCurrentStoreAsync();

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            foreach (var salesQuotation in salesQuotations)
            {
                var entryName = string.Format("{0} {1}", await _localizationService.GetResourceAsync("Pdf.SalesQuotation"), language);

                await using var fileStreamInZip = archive.CreateEntry($"{entryName}.pdf").Open();
                await using var pdfStream = new MemoryStream();
                await PrintSalesQuotationToPdfAsync(pdfStream, salesQuotation, language,vendor);
                pdfStream.Position = 0;
                await pdfStream.CopyToAsync(fileStreamInZip);
            }
        }

        #endregion
    }
}