using HarfBuzzSharp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Services.Messages;
using Nop.Cypher.Misc.MJSTraders.Services.PDF;
using Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Media;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.SalesQuotations
{
    public partial class SalesQuotationModelFactory : ISalesQuotationModelFactory
    {
        #region Fields

        private readonly ISalesQuotationService _salesQuotationService;
        private readonly IMJSTradersWorkFlowMessageService _mjsTradersWorkFlowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IDownloadService _downloadService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICustomerService _customerService;
        private readonly IMJSTPdfService _mJSTPdfService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public SalesQuotationModelFactory(ISalesQuotationService salesQuotationService,
            IMJSTradersWorkFlowMessageService mjsTradersWorkFlowMessageService,
            LocalizationSettings localizationSettings,
            IDownloadService downloadService,
            IDateTimeHelper dateTimeHelper,
            ICustomerService customerService,
            IMJSTPdfService mJSTPdfService,
            IWorkContext workContext)
        {
            _salesQuotationService = salesQuotationService;
            _mjsTradersWorkFlowMessageService = mjsTradersWorkFlowMessageService;
            _localizationSettings = localizationSettings;
            _downloadService = downloadService;
            _dateTimeHelper = dateTimeHelper;
            _customerService = customerService;
            _mJSTPdfService = mJSTPdfService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        public virtual void PrepareAvailableEntities(IList<SelectListItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items.Add(new SelectListItem { Value = MJSTradersUtilities.MJSTraders, Text = MJSTradersUtilities.MJSTraders });
            items.Add(new SelectListItem { Value = MJSTradersUtilities.MJSEnterprises, Text = MJSTradersUtilities.MJSEnterprises });
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomAlphanumericCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var stringChars = new char[length];
            for (var i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        #endregion

        #region Methods

        #region Sales quotation Customer

        /// <summary>
        /// Prepare sales quotaion list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion list model</param>
        /// <returns>Sales quotation list model</returns>
        public virtual async Task<SalesQuotationCustomerListModel> PrepareSalesQuotationCustomerListModelAsync(SalesQuotationCustomerSearchModel searchModel)
        {
            var salesQuotations = await _salesQuotationService.GetSalesQuotationCustomerAsync(name: searchModel.SearchName,
              email: searchModel.SearchEmail, company: searchModel.SearchCompany, title: searchModel.SearchTitle,
              referenceNumber: searchModel.SearchReferenceNumber, pageIndex: searchModel.Page - 1,
              pageSize: searchModel.PageSize);

            var model = new SalesQuotationCustomerListModel().PrepareToGrid(searchModel, salesQuotations, () =>
            {
                return salesQuotations.Select(x =>
                {
                    return new SalesQuotationCustomerModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        Company = x.Company,
                    };
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare sales quotation search model
        /// </summary>
        /// <param name="searchModel">Sales quotation search model</param>
        /// <returns>Sales quotation search model</returns>
        public virtual SalesQuotationCustomerSearchModel PrepareSalesQuotationCustomerSearchModel(SalesQuotationCustomerSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare sales quotation model
        /// </summary>
        /// <param name="model">Sales quotation model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation model</returns>
        public virtual SalesQuotationCustomerModel PrepareSalesQuotationCustomerModel(SalesQuotationCustomerModel model, SalesQuotationCustomer salesQuotation)
        {
            if (salesQuotation != null)
            {
                model ??= new SalesQuotationCustomerModel
                {
                    Id = salesQuotation.Id,
                };

                model.Name = salesQuotation.Name;
                model.Email = salesQuotation.Email;
                model.Company = salesQuotation.Company;

                //prepare nested search model
                //PrepareSalesQuotationLineSearchModel(model.SalesQuotationLineSearchModel, salesQuotation);
            }

            return model;
        }

        #endregion

        #region Sales quotation

        /// <summary>
        /// Prepare sales quotaion list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion list model</param>
        /// <returns>Sales quotation list model</returns>
        public virtual async Task<SalesQuotationListModel> PrepareSalesQuotationListModelAsync(SalesQuotationSearchModel searchModel)
        {
            var salesQuotations = await _salesQuotationService.GetAllSalesQuotationsAsync(salesQuotationCustomerId: searchModel.SalesQuotationCustomerId,
              pageIndex: searchModel.Page - 1,
              pageSize: searchModel.PageSize);

            var model = new SalesQuotationListModel().PrepareToGrid(searchModel, salesQuotations, () =>
            {
                return salesQuotations.Select(x =>
                {
                    return new SalesQuotationModel
                    {
                        Id = x.Id,
                        QuotationTitle = x.Title,
                        Name = x.Name,
                        Email = x.Email,
                        EmailCC = x.EmailCC,
                        ReferenceNumber = x.ReferenceNumber,
                    };
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare sales quotation search model
        /// </summary>
        /// <param name="searchModel">Sales quotation search model</param>
        /// <returns>Sales quotation search model</returns>
        public virtual SalesQuotationSearchModel PrepareSalesQuotationSearchModel(SalesQuotationSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare sales quotation model
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quotation customer</param>
        /// <param name="model">Sales quotation model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation model</returns>
        public virtual async Task<SalesQuotationModel> PrepareSalesQuotationModelAsync(SalesQuotationCustomer salesQuotationCustomer, SalesQuotationModel model, SalesQuotation salesQuotation)
        {
            if (salesQuotation != null)
            {
                model ??= new SalesQuotationModel
                {
                    Id = salesQuotation.Id,
                    ReferenceNumber = salesQuotation.ReferenceNumber,
                };
                if (string.IsNullOrEmpty(model.ReferenceNumber))
                {
                    gereratecode:
                    var randomCode = GenerateRandomAlphanumericCode(10);
                    if (_salesQuotationService.GetSalesQuotationByReferenceNumberAsync(randomCode) != null)
                        goto gereratecode;
                    model.ReferenceNumber = randomCode;
                }
                model.CreatedBy = salesQuotation.CreatedBy ?? await _customerService.GetCustomerFullNameAsync(await _workContext.GetCurrentCustomerAsync());
                model.SalesQuotationCustomerId = salesQuotation.SalesQuotationCustomerId;
                model.QuotationTitle = salesQuotation.Title;
                model.Name = salesQuotation.Name;
                model.Email = salesQuotation.Email;
                model.EmailCC = salesQuotation.EmailCC;
                model.Company = salesQuotation.Company;
                model.Designation = salesQuotation.Designation;
                model.InquiryDate = salesQuotation.InquiryDate;
                model.GenerateDate = salesQuotation.GenerateDate;
                model.IsTaxInclusive = salesQuotation.IsTaxInclusive;
                model.DeliveryTerms = salesQuotation.DeliveryTerms;
                model.DeliveryCharges = salesQuotation.DeliveryCharges;
                model.PaymentTerms = salesQuotation.PaymentTerms;
                model.ValidUntil = salesQuotation.ValidUntilUtc.HasValue ?
                    await _dateTimeHelper.ConvertToUserTimeAsync((DateTime)salesQuotation.ValidUntilUtc) : salesQuotation.ValidUntilUtc;
                model.SalesQuotationNote = salesQuotation.SalesQuotationNote;
                model.SendFromEntity = salesQuotation.SendFromEntity;

                //prepare nested search model
                PrepareSalesQuotationLineSearchModel(model.SalesQuotationLineSearchModel, salesQuotation);
            }
            else if (salesQuotationCustomer != null)
            {
                model ??= new SalesQuotationModel();
                gereratecode:
                var randomCode = GenerateRandomAlphanumericCode(10);
                if (await _salesQuotationService.GetSalesQuotationByReferenceNumberAsync(randomCode) != null)
                    goto gereratecode;
                model.ReferenceNumber = randomCode;
                model.Company = salesQuotationCustomer.Company;
                model.SalesQuotationCustomerId = salesQuotationCustomer.Id;
                model.Name = salesQuotationCustomer.Name;
                model.Email = salesQuotationCustomer.Email;
                model.CreatedBy = await _customerService.GetCustomerFullNameAsync(await _workContext.GetCurrentCustomerAsync());
            }

            //Prepare Enitties
            PrepareAvailableEntities(model.AvailableEntities);

            return model;
        }

        /// <summary>
        /// Send 'send sales quotation' notifications 
        /// </summary>
        /// <param name="salesQuotation">Sales quotation</param>
        public virtual async Task SendNotificationAsync(SalesQuotation salesQuotation , Stream stream)
        {
            var language = await _workContext.GetWorkingLanguageAsync();
            var vendor = await _workContext.GetCurrentVendorAsync();
            var salesQuotationAttachmentFilePath = await _mJSTPdfService.PrintSalesQuotationToPdfAsync(stream,salesQuotation, language, vendor);
            var salesQuotationAttachmentFileName = $"sales_quotation_{salesQuotation.ReferenceNumber}.pdf";
            var salesQuotationCustomerNotificationQueuedEmailIds = await _mjsTradersWorkFlowMessageService
               .SendSalesQuotationCustomerNotificationAsync(salesQuotation, _localizationSettings.DefaultAdminLanguageId, salesQuotationAttachmentFilePath, salesQuotationAttachmentFileName);

            //// send email to cc
            //if (!string.IsNullOrWhiteSpace(salesQuotation.EmailCC))
            //{
            //    foreach (var email in salesQuotation.Email.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
            //    {
            //       _mjsTradersWorkFlowMessageService
            // .SendSalesQuotationCustomerNotification(salesQuotation, _localizationSettings.DefaultAdminLanguageId, salesQuotationAttachmentFilePath, salesQuotationAttachmentFileName);

            //    }
            //}

        }


        #endregion

        #region Sales quotation line

        /// <summary>
        /// Prepare sales quotaion line list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion line list model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation line list model</returns>
        public virtual async Task<SalesQuotationLineListModel> PrepareSalesQuotationLineListModelAsync(SalesQuotationLineSearchModel searchModel, SalesQuotation salesQuotation)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            var salesQuotationLines = await _salesQuotationService.GetSalesQuotationLineAsync(salesQuotationId: salesQuotation.Id,
              pageIndex: searchModel.Page - 1,
              pageSize: searchModel.PageSize);

            var model = new SalesQuotationLineListModel().PrepareToGrid(searchModel, salesQuotationLines, () =>
            {
                return salesQuotationLines.Select(x =>
                {
                    return new SalesQuotationLineModel
                    {
                        Id = x.Id,
                        SaleQuotationId = x.SaleQuotationId,
                        Name = x.Name,
                        Description = x.Description,
                        Qty = x.Qty,
                        Price = x.Price,
                    };
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare sales quotation line search model
        /// </summary>
        /// <param name="searchModel">Sales quotation line search model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation search line model</returns>
        public virtual SalesQuotationLineSearchModel PrepareSalesQuotationLineSearchModel(SalesQuotationLineSearchModel searchModel, SalesQuotation salesQuotation)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            searchModel.SaleQuotationId = salesQuotation.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare sales quotation line model
        /// </summary>
        /// <param name="model">Sales quotation line model</param>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        /// <returns>Sales quotation line model</returns>
        public virtual SalesQuotationLineModel PrepareSalesQuotationLineModel(SalesQuotationLineModel model, SalesQuotationLine salesQuotationLine)
        {
            if (salesQuotationLine != null)
            {
                model ??= new SalesQuotationLineModel
                {
                    Id = salesQuotationLine.Id,
                };

                model.Name = salesQuotationLine.Name;
                model.Description = salesQuotationLine.Description;
                model.Qty = salesQuotationLine.Qty;
                model.Price = salesQuotationLine.Price;
                model.SaleQuotationId = salesQuotationLine.SaleQuotationId;
            }

            return model;
        }

        #endregion

        #region Sale quotation documnet

        /// <summary>
        /// Prepare sales quotaion document list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion document list model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation document list model</returns>
        public virtual async Task<SalesQuotationDocumentListModel> PrepareSalesQuotationDocumentListModelAsync(SalesQuotationDocumentSearchModel searchModel, SalesQuotation salesQuotation)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            var salesQuotationDocuments = await _salesQuotationService.GetAllSalesQuotationDocumentsAsync(salesQuotationId: salesQuotation.Id,
              pageIndex: searchModel.Page - 1,
              pageSize: searchModel.PageSize);

            var model = await new SalesQuotationDocumentListModel().PrepareToGridAsync(searchModel, salesQuotationDocuments, () =>
             {
                 return salesQuotationDocuments.SelectAwait(async x =>
                 {
                     var download = await _downloadService.GetDownloadByIdAsync(x.DownloadId);
                     return new SalesQuotationDocumentModel
                     {
                         Id = x.Id,
                         SaleQuotationId = x.SaleQuotationId,
                         DownloadId = x.DownloadId,
                         DownloadGuid = download?.DownloadGuid ?? Guid.Empty,
                         FileName = download?.Filename ?? string.Empty,
                     };
                 });
             });

            return model;
        }

        /// <summary>
        /// Prepare sales quotation line search model
        /// </summary>
        /// <param name="searchModel">Sales quotation document search model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation search document model</returns>
        public virtual SalesQuotationDocumentSearchModel PrepareSalesQuotationDocumentSearchModel(SalesQuotationDocumentSearchModel searchModel, SalesQuotation salesQuotation)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            searchModel.SaleQuotationId = salesQuotation.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion

        #endregion
    }
}
