using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Factories.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Model.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Services.Messages;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Controllers;
using System;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Controllers
{
    public class PONumberController : BasePublicController
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IPONumberModelFactory _poNumberModelFactory;
        private readonly IPONumberService _poNumberService;
        private readonly IMJSTradersWorkFlowMessageService _mjsTradersWorkFlowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IDownloadService _downloadService;
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public PONumberController(IWorkContext workContext,
            IPONumberModelFactory poNumberModelFactory,
            IPONumberService poNumberService,
            IMJSTradersWorkFlowMessageService mjsTradersWorkFlowMessageService,
            LocalizationSettings localizationSettings,
            ILocalizationService localizationService,
            IDownloadService downloadService,
            IGenericAttributeService genericAttributeService,
            IStoreContext storeContext)
        {
            _workContext = workContext;
            _poNumberModelFactory = poNumberModelFactory;
            _poNumberService = poNumberService;
            _mjsTradersWorkFlowMessageService = mjsTradersWorkFlowMessageService;
            _localizationSettings = localizationSettings;
            _localizationService = localizationService;
            _downloadService = downloadService;
            _genericAttributeService = genericAttributeService;
            _storeContext = storeContext;
        }

        #endregion

        #region Method

        public virtual async Task<IActionResult> CustomerPONumbers(int? pageNumber)
        {
            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return RedirectToRoute("CustomerInfo");

            var model =await _poNumberModelFactory.PreparePONumbersModelAsync((await _workContext.GetCurrentCustomerAsync()).Id, pageNumber);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteCustomerPONumbers(int ponumberId)
        {
            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return RedirectToRoute("CustomerInfo");

            if (ponumberId > 0)
            {
                var poNumber = await _poNumberService.GetPONumberByIdAsync(ponumberId);
                if (poNumber != null)
                {

                    // delete download 
                    var download = await _downloadService.GetDownloadByIdAsync(poNumber.DownloadId);
                    if (download != null)
                        await _downloadService.DeleteDownloadAsync(download);

                    await _poNumberService.DeletePONumberAsync(poNumber);
                }
            }

            return Json(new
            {
                message = await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.PONumber.Deleted"),
                redirect = Url.RouteUrl("Nop.Cypher.Misc.MJSTraders.MyAccount.MJSTraders.CustomerPONumbers"),
            });
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreatePONumber(PONumbersModel model)
        {
            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return RedirectToRoute("CustomerInfo");

            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    var downloadFile = await _downloadService.GetDownloadByIdAsync(model.DownloadId);
                    var poNumber = new PONumber
                    {
                        Title = downloadFile.Filename,
                        DownloadId = model.DownloadId,
                        IsApproved = false,
                        CreateOnUtc = DateTime.UtcNow,
                        CustomerId = model.CustomerId,
                    };

                    //Insert PO number
                    await _poNumberService.InsertPONumberAsync(poNumber);

                    //send admin notification
                    await _mjsTradersWorkFlowMessageService.SendAdminUploadPoNumberNotificationAsync(poNumber, _localizationSettings.DefaultAdminLanguageId);

                    //send customer notification
                    //_mjsTradersWorkFlowMessageService.SendCustomerUploadPoNumberNotification(poNumber, _localizationSettings.DefaultAdminLanguageId);
                }
            }

            return RedirectToAction("CustomerPONumbers");
        }

        public virtual async Task<IActionResult> SelectPurchaseOrder(int purchaseorderid)
        {
            if (purchaseorderid == 0)
                return Json(new { status = false, message = await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.OrderMapping.PurchaseOrder.NotFound") });

            var po = await _poNumberService.GetPONumberByIdAsync(purchaseorderid);

            if (po == null)
                return Json(new { status = false, message = await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.OrderMapping.PurchaseOrder.NotFound") });

            await _genericAttributeService.SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(), MJSTradersUtilities.LastSelectedPurchaseOrderId, purchaseorderid, (await _storeContext.GetCurrentStoreAsync()).Id);

            return Json(new { status = true });
        }

        #endregion
    }
}
