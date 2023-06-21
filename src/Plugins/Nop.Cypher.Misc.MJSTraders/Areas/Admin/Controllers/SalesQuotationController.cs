using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.SalesQuotations;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Security;
using Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public  class SalesQuotationController : BaseAdminController
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly ISalesQuotationModelFactory _salesQuotationModelFactory;
        private readonly ISalesQuotationService _salesQuotationService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public SalesQuotationController(IWorkContext workContext,
            ISalesQuotationModelFactory salesQuotationModelFactory,
            ISalesQuotationService salesQuotationService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            IDateTimeHelper dateTimeHelper,
            IPermissionService permissionService)
        {
            _workContext = workContext;
            _salesQuotationModelFactory = salesQuotationModelFactory;
            _salesQuotationService = salesQuotationService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _dateTimeHelper = dateTimeHelper;
            _permissionService = permissionService;
        }

        #endregion

        #region Methods

        #region Sales quotation Customer

        #region List

        public virtual async Task<IActionResult> CustomerList()
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            //Prepare model
            var model = _salesQuotationModelFactory.PrepareSalesQuotationCustomerSearchModel(new SalesQuotationCustomerSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationCustomerList(SalesQuotationCustomerSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _salesQuotationModelFactory.PrepareSalesQuotationCustomerListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Create

        public virtual async Task<IActionResult> CustomerCreate()
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            //Prepare model
            var model = _salesQuotationModelFactory.PrepareSalesQuotationCustomerModel(new SalesQuotationCustomerModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> CustomerCreate(SalesQuotationCustomerModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    var salesQuotationCustomer = new SalesQuotationCustomer
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Company = model.Company,
                    };

                    //Insert sales quotation
                    await _salesQuotationService.InsertSalesQuotationCustomerAsync(salesQuotationCustomer);

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationCustomer.Added"));

                    if (!continueEditing)
                        return RedirectToAction("CustomerList");

                    return RedirectToAction("CustomerEdit", new { id = salesQuotationCustomer.Id });
                }
            }

            return View(model);
        }

        public virtual async Task<IActionResult> AddQuote(int customerId)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return RedirectToAction("List", "Customer");

            var salesQuotationCustomer = (await _salesQuotationService.GetSalesQuotationCustomerAsync(email: customer.Email)).FirstOrDefault();

            if (salesQuotationCustomer == null)
            {
                salesQuotationCustomer = new SalesQuotationCustomer
                {
                    Name = await _customerService.GetCustomerFullNameAsync(customer),
                    Email = customer.Email,
                    Company = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CompanyAttribute),
                };

                //Insert sales quotation
                await _salesQuotationService.InsertSalesQuotationCustomerAsync(salesQuotationCustomer);
            }

            return RedirectToAction("CustomerEdit", new { id = salesQuotationCustomer.Id });
        }

        #endregion

        #region Update

        public virtual async Task<IActionResult> CustomerEdit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            var salesQuotationCustomer = await _salesQuotationService.GetSalesQuotationCustomerByIdAsync(id);

            if (salesQuotationCustomer == null)
                return RedirectToAction("CustomerList");

            if (string.IsNullOrWhiteSpace(salesQuotationCustomer.Email))
                return RedirectToAction("CustomerList");

            var customer = await _customerService.GetCustomerByEmailAsync(salesQuotationCustomer.Email);
            if (string.IsNullOrEmpty(salesQuotationCustomer.Name))
                salesQuotationCustomer.Name = await _customerService.GetCustomerFullNameAsync(customer);
            if (string.IsNullOrEmpty(salesQuotationCustomer.Company))
                salesQuotationCustomer.Company = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CompanyAttribute);

            await _salesQuotationService.UpdateSalesQuotationCustomerAsync(salesQuotationCustomer);

            //Prepare model
            var model = _salesQuotationModelFactory.PrepareSalesQuotationCustomerModel(null, salesQuotationCustomer);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> CustomerEdit(SalesQuotationCustomerModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            //try to get a sales quotation with the specified id
            var salesQuotationCustomer = await _salesQuotationService.GetSalesQuotationCustomerByIdAsync(model.Id);
            if (salesQuotationCustomer == null)
                return RedirectToAction("CustomerList");

            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    salesQuotationCustomer.Name = model.Name;
                    salesQuotationCustomer.Email = model.Email;
                    salesQuotationCustomer.Company = model.Company;

                    //Insert sales quotation
                    await _salesQuotationService.UpdateSalesQuotationCustomerAsync(salesQuotationCustomer);

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationCustomer.Updated"));

                    if (!continueEditing)
                        return RedirectToAction("CustomerList");

                    return RedirectToAction("CustomerEdit", new { id = salesQuotationCustomer.Id });
                }
            }

            //prepare model
            model = _salesQuotationModelFactory.PrepareSalesQuotationCustomerModel(model, salesQuotationCustomer);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationCustomerDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            //try to get a sales quotation customer with the specified id
            var salesQuotationCustomer = await _salesQuotationService.GetSalesQuotationCustomerByIdAsync(id);
            if (salesQuotationCustomer == null)
                return RedirectToAction("CustomerList");

            var salesQuotations = await _salesQuotationService.GetAllSalesQuotationsAsync(id);
            if (salesQuotations.Any())
            {
                foreach (var salesQuotation in salesQuotations)
                {
                    var salesQuotationLines = (await _salesQuotationService.GetSalesQuotationLineAsync(salesQuotation.Id)).ToList();
                    foreach (var salesQuotationLine in salesQuotationLines)
                    {
                        await _salesQuotationService.DeleteSalesQuotationLineAsync(salesQuotationLine);
                    }

                    var salesQuotationDocuments = await _salesQuotationService.GetAllSalesQuotationDocumentsAsync(salesQuotation.Id);
                    foreach (var salesQuotationDocument in salesQuotationDocuments)
                    {
                        await _salesQuotationService.DeleteSalesQuotationDocumentAsync(salesQuotationDocument);
                    }
                    await _salesQuotationService.DeleteSalesQuotationAsync(salesQuotation);
                }
            }

            await _salesQuotationService.DeleteSalesQuotationCustomerAsync(salesQuotationCustomer);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationCustomer.Deleted"));

            return RedirectToAction("CustomerList");
        }

        [HttpPost]
        public virtual async Task<IActionResult> CustomerDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation customer with the specified id
            var salesQuotationCustomer = await _salesQuotationService.GetSalesQuotationCustomerByIdAsync(id);
            if (salesQuotationCustomer == null)
                return new NullJsonResult();

            var salesQuotations = await _salesQuotationService.GetAllSalesQuotationsAsync(id);
            if (salesQuotations.Any())
            {
                foreach (var salesQuotation in salesQuotations)
                {
                    var salesQuotationLines = (await _salesQuotationService.GetSalesQuotationLineAsync(salesQuotation.Id)).ToList();
                    foreach (var salesQuotationLine in salesQuotationLines)
                    {
                        await _salesQuotationService.DeleteSalesQuotationLineAsync(salesQuotationLine);
                    }

                    var salesQuotationDocuments = await _salesQuotationService.GetAllSalesQuotationDocumentsAsync(salesQuotation.Id);
                    foreach (var salesQuotationDocument in salesQuotationDocuments)
                    {
                        await _salesQuotationService.DeleteSalesQuotationDocumentAsync(salesQuotationDocument);
                    }
                    await _salesQuotationService.DeleteSalesQuotationAsync(salesQuotation);
                }
            }

            await _salesQuotationService.DeleteSalesQuotationCustomerAsync(salesQuotationCustomer);

            return new NullJsonResult();
        }

        #endregion

        #endregion

        #region Sales quotation

        #region List

        public virtual async Task<IActionResult> List()
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            //Prepare model
            var model = _salesQuotationModelFactory.PrepareSalesQuotationSearchModel(new SalesQuotationSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationList(SalesQuotationSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _salesQuotationModelFactory.PrepareSalesQuotationListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Create

        public virtual async Task<IActionResult> Create(int salesQuotationCustomerId)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            var salesQuotationCustomer = await _salesQuotationService.GetSalesQuotationCustomerByIdAsync(salesQuotationCustomerId);
            if (salesQuotationCustomer == null)
                return RedirectToAction("CustomerList");

            //Prepare model
            var model = await _salesQuotationModelFactory.PrepareSalesQuotationModelAsync(salesQuotationCustomer, new SalesQuotationModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(SalesQuotationModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    var salesQuotation = new SalesQuotation
                    {
                        CreatedBy = model.CreatedBy,
                        ReferenceNumber = model.ReferenceNumber,
                        SalesQuotationCustomerId = model.SalesQuotationCustomerId,
                        Title = model.QuotationTitle,
                        Name = model.Name,
                        Email = model.Email,
                        EmailCC = model.EmailCC,
                        Company = model.Company,
                        Designation = model.Designation,
                        InquiryDate = model.InquiryDate,
                        GenerateDate = model.GenerateDate,
                        IsTaxInclusive = model.IsTaxInclusive,
                        DeliveryTerms = model.DeliveryTerms,
                        DeliveryCharges = model.DeliveryCharges,
                        PaymentTerms = model.PaymentTerms,
                        ValidUntilUtc = model.ValidUntil.HasValue ?
                        _dateTimeHelper.ConvertToUtcTime((DateTime)model.ValidUntil, await _dateTimeHelper.GetCurrentTimeZoneAsync()) : model.ValidUntil,
                        SalesQuotationNote = model.SalesQuotationNote,
                        SendFromEntity = model.SendFromEntity,
                    };

                    //Insert sales quotation
                    await _salesQuotationService.InsertSalesQuotationAsync(salesQuotation);

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Added"));

                    if (!continueEditing)
                        return RedirectToAction("CustomerEdit", new { id = salesQuotation.SalesQuotationCustomerId });

                    return RedirectToAction("Edit", new { id = salesQuotation.Id });
                }
            }

            return View(model);
        }

        #endregion

        #region Update

        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            var salesQuotation = await _salesQuotationService.GetSalesQuotationByIdAsync(id);

            if (salesQuotation == null)
                return RedirectToAction("CustomerEdit", new { id = salesQuotation.SalesQuotationCustomerId });

            //Prepare model
            var model = await _salesQuotationModelFactory.PrepareSalesQuotationModelAsync(null, null, salesQuotation);

            model.salesQuotationNewLineModel.SaleQuotationId = id;
            model.SalesQuotationDocumentModel.SaleQuotationId = id;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(SalesQuotationModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            //try to get a sales quotation with the specified id
            var salesQuotation = await _salesQuotationService.GetSalesQuotationByIdAsync(model.Id);
            if (salesQuotation == null)
                return RedirectToAction("CustomerEdit", new { id = salesQuotation.SalesQuotationCustomerId });

            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    salesQuotation.CreatedBy = model.CreatedBy;
                    salesQuotation.Title = model.QuotationTitle;
                    salesQuotation.Name = model.Name;
                    salesQuotation.ReferenceNumber = model.ReferenceNumber;
                    salesQuotation.Email = model.Email;
                    salesQuotation.EmailCC = model.EmailCC;
                    salesQuotation.Company = model.Company;
                    salesQuotation.InquiryDate = model.InquiryDate;
                    salesQuotation.GenerateDate = model.GenerateDate;
                    salesQuotation.IsTaxInclusive = model.IsTaxInclusive;
                    salesQuotation.SalesQuotationNote = model.SalesQuotationNote;
                    salesQuotation.SendFromEntity = model.SendFromEntity;
                    salesQuotation.DeliveryTerms = model.DeliveryTerms;
                    salesQuotation.Designation = model.Designation;
                    salesQuotation.DeliveryCharges = model.DeliveryCharges;
                    salesQuotation.PaymentTerms = model.PaymentTerms;
                    salesQuotation.ValidUntilUtc = model.ValidUntil.HasValue ?
                    _dateTimeHelper.ConvertToUtcTime((DateTime)model.ValidUntil, await _dateTimeHelper.GetCurrentTimeZoneAsync()) : model.ValidUntil;

                    //Insert sales quotation
                    await _salesQuotationService.UpdateSalesQuotationAsync(salesQuotation);

                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Updated"));

                    if (!continueEditing)
                        return RedirectToAction("CustomerEdit", new { id = salesQuotation.SalesQuotationCustomerId });

                    return RedirectToAction("Edit", new { id = salesQuotation.Id });
                }
            }

            //prepare model
            model = await _salesQuotationModelFactory.PrepareSalesQuotationModelAsync(null, model, salesQuotation);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation with the specified id
            var salesQuotation = await _salesQuotationService.GetSalesQuotationByIdAsync(id);
            if (salesQuotation == null)
                return new NullJsonResult();

            var salesQuotationLines = (await _salesQuotationService.GetSalesQuotationLineAsync(salesQuotation.Id)).ToList();
            foreach (var salesQuotationLine in salesQuotationLines)
            {
                await _salesQuotationService.DeleteSalesQuotationLineAsync(salesQuotationLine);
            }

            var salesQuotationDocuments = await _salesQuotationService.GetAllSalesQuotationDocumentsAsync(salesQuotation.Id);
            foreach (var salesQuotationDocument in salesQuotationDocuments)
            {
                await _salesQuotationService.DeleteSalesQuotationDocumentAsync(salesQuotationDocument);
            }

            await _salesQuotationService.DeleteSalesQuotationAsync(salesQuotation);

            //_notificationService.SuccessNotification(_localizationService.GetResource("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Deleted"));

            return new NullJsonResult();
        }

        #endregion

        #region SendNotification

        public virtual async Task<IActionResult> SendNotification(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return AccessDeniedView();

            //try to get a sales quotation with the specified id
            var salesQuotation = await _salesQuotationService.GetSalesQuotationByIdAsync(id);
            if (salesQuotation == null)
                return RedirectToAction("List");

            await _salesQuotationModelFactory.SendNotificationAsync(salesQuotation,null,);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Sent"));

            return RedirectToAction("Edit", new { id = id });
        }

        #endregion

        #endregion

        #region Sales quotation line

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationLineList(SalesQuotationLineSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation with the specified id
            var salesQuotation = await _salesQuotationService.GetSalesQuotationByIdAsync(searchModel.SaleQuotationId)
                ?? throw new ArgumentException("No sales quotation found with the specified id");

            //prepare model
            var model = await _salesQuotationModelFactory.PrepareSalesQuotationLineListModelAsync(searchModel, salesQuotation);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationLineUpdate(SalesQuotationLineModel model)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation line with the specified id
            var salesQuotationLine = await _salesQuotationService.GetSalesQuotationLineByIdAsync(model.Id)
                    ?? throw new ArgumentException("No sales quotation line found with the specified id");

            salesQuotationLine.Name = model.Name;
            salesQuotationLine.Description = model.Description;
            salesQuotationLine.Qty = model.Qty;
            salesQuotationLine.Price = model.Price;

            //update sales quotation line
            await _salesQuotationService.UpdateSalesQuotationLineAsync(salesQuotationLine);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationLineDelete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation with the specified id
            var salesQuotationLine = await _salesQuotationService.GetSalesQuotationLineByIdAsync(id)
                    ?? throw new ArgumentException("No sales quotation line found with the specified id");

            //Delete  sales quotation line
            await _salesQuotationService.DeleteSalesQuotationLineAsync(salesQuotationLine);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> SalesQuotationLineAddPopup(int salesQuotationId)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            //prepare model
            var model = _salesQuotationModelFactory.PrepareSalesQuotationLineModel(new SalesQuotationLineModel(), null);

            model.SaleQuotationId = salesQuotationId;

            return View(model);
        }

        [HttpPost]
        //[FormValueRequired("save")]
        public virtual async Task<IActionResult> SalesQuotationLineAddPopup(SalesQuotationLineModel model)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            if (model != null)
            {
                var salesQuotationLine = new SalesQuotationLine
                {
                    Name = model.Name,
                    Description = model.Description,
                    Qty = model.Qty,
                    Price = model.Price,
                    SaleQuotationId = model.SaleQuotationId,
                };

                //Insert sales quotatation line
                await _salesQuotationService.InsertSalesQuotationLineAsync(salesQuotationLine);

                //_notificationService.SuccessNotification(_localizationService.GetResource("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationLine.Added"));
            }


            ViewBag.RefreshPage = true;
            return Json(new { Result = true });
            //return View(new SalesQuotationLineModel());
        }

        #endregion

        #region Sales quotation document

        [HttpPost]
        public virtual async Task<IActionResult> SalesQuotationDocumentList(SalesQuotationDocumentSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation with the specified id
            var salesQuotation = await _salesQuotationService.GetSalesQuotationByIdAsync(searchModel.SaleQuotationId)
                ?? throw new ArgumentException("No sales quotation found with the specified id");

            //prepare model
            var model = await _salesQuotationModelFactory.PrepareSalesQuotationDocumentListModelAsync(searchModel, salesQuotation);

            return Json(model);
        }

        public virtual async Task<IActionResult> CreateSalesQuotationDocument(SalesQuotationDocumentModel model)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            if (model != null)
            {
                var salesQuotationDocument = new SalesQuotationDocument
                {
                    DownloadId = model.DownloadId,
                    SaleQuotationId = model.SaleQuotationId,
                };

                //Insert sales quotatation document
                await _salesQuotationService.InsertSalesQuotationDocumentAsync(salesQuotationDocument);
            }

            ViewBag.RefreshPage = true;
            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteSalesQuotationDocument(int id)
        {
            if (!await _permissionService.AuthorizeAsync(QuotationPermissionProvider.AccessSalesQuotation))
                return await AccessDeniedDataTablesJson();

            //try to get a sales quotation with the specified id
            var salesQuotationDocument = await _salesQuotationService.GetSalesQuotationDocumentByIdAsync(id)
                    ?? throw new ArgumentException("No sales quotation document found with the specified id");

            //Delete  sales quotation document
            await _salesQuotationService.DeleteSalesQuotationDocumentAsync(salesQuotationDocument);

            return new NullJsonResult();
        }

        #endregion

        #endregion
    }
}
