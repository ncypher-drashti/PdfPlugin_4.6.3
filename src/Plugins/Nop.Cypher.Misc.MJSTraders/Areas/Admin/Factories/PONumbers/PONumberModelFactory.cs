using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Customers;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.PONumbers
{
    public partial class PONumberModelFactory : IPONumberModelFactory
    {
        #region Fields

        private readonly IPONumberService _poNumberService;
        private readonly ICustomerService _customerService;
        private readonly IDownloadService _downloadService;
        private readonly ILocalizationService _localizationService;
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public PONumberModelFactory(IPONumberService poNumberService,
            ICustomerService customerService,
            IDownloadService downloadService,
            ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService)
        {
            _poNumberService = poNumberService;
            _customerService = customerService;
            _downloadService = downloadService;
            _localizationService = localizationService;
            _genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare default item
        /// </summary>
        /// <param name="items">Available items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use "All" text</param>
        protected virtual async Task PrepareDefaultItemAsync(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText = defaultItemText ?? await _localizationService.GetResourceAsync("Admin.Common.All");

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }

        /// <summary>
        /// Prepare default item for search approve
        /// </summary>
        /// <param name="items">Available items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use "All" text</param>
        public virtual async Task PrepareAvailableApproveItemsAsync(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items.Add(new SelectListItem { Value = "true", Text = "Approve only" });
            items.Add(new SelectListItem { Value = "false", Text = "Unapprove only" });

            //insert special item for the default value
            await PrepareDefaultItemAsync(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare default item for search delete
        /// </summary>
        /// <param name="items">Available items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use "All" text</param>
        public virtual async Task PrepareAvailableDeleteItemsAsync(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items.Add(new SelectListItem { Value = "true", Text = "Delete only" });
            items.Add(new SelectListItem { Value = "false", Text = "Undelete only", Selected = true });

            //insert special item for the default value
            await PrepareDefaultItemAsync(items, withSpecialDefaultItem, defaultItemText);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare PO number list model
        /// </summary>
        /// <param name="searchModel">PO number list model</param>
        /// <returns>PO number list model</returns>
        public virtual async Task<PONumberListModel> PreparePONumberListModelAsync(PONumberSearchModel searchModel)
        {
            var poNumbers = await _poNumberService.GetPONumbersAsync(title: searchModel.SearchTitle,
                customerName: searchModel.SearchCustomer,
                customerEmail: searchModel.SearchEmail,
                isApprove: searchModel.SearchIsApprove,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            var model = await new PONumberListModel().PrepareToGridAsync(searchModel, poNumbers, () =>
             {
                 return poNumbers.SelectAwait(async x =>
                 {
                     var customer = await _customerService.GetCustomerByIdAsync(x.CustomerId);
                     var download = await _downloadService.GetDownloadByIdAsync(x.DownloadId);
                     return new PONumberModel
                     {
                         Id = x.Id,
                         CustomerId = x.CustomerId,
                         CustomerName = await _customerService.GetCustomerFullNameAsync(customer),
                         CompanyName = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CompanyAttribute),
                         CustomerEmail = customer.Email,
                         ApproveOnUtc = x.ApproveOnUtc,
                         CreateOnUtc = x.CreateOnUtc,
                         DownloadId = x.DownloadId,
                         DownloadGuid = download.DownloadGuid,
                         IsApproved = x.IsApproved,
                         Title = x.Title
                     };
                 });
             });
            return model;
        }

        /// <summary>
        /// Prepare PO number search model
        /// </summary>
        /// <param name="searchModel">PO number search model</param>
        /// <returns>PO number search model</returns>
        public virtual async Task<PONumberSearchModel> PreparePONumberSearchModelAsync(PONumberSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available item for search approve 
            await PrepareAvailableApproveItemsAsync(searchModel.AvailableApproveItem, true, await _localizationService.GetResourceAsync("Admin.Common.All"));

            //prepare available item for search delete 
            await PrepareAvailableDeleteItemsAsync(searchModel.AvailableDeleteItem, true, await _localizationService.GetResourceAsync("Admin.Common.All"));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion
    }
}
