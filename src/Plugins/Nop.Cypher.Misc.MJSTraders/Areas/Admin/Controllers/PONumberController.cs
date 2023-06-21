using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Services.Messages;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class PONumberController : BaseAdminController
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IPONumberModelFactory _poNumberModelFactory;
        private readonly IPONumberService _poNumberService;
        private readonly IMJSTradersWorkFlowMessageService _mjsTradersWorkFlowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IDownloadService _downloadService;
        private readonly ICustomerService _customerService;
        private readonly IRepository<Download> _downloadRepository;

        #endregion

        #region Ctor

        public PONumberController(IWorkContext workContext,
            IPONumberModelFactory poNumberModelFactory,
            IPONumberService poNumberService,
            IMJSTradersWorkFlowMessageService mjsTradersWorkFlowMessageService,
            LocalizationSettings localizationSettings,
            IDownloadService downloadService,
            ICustomerService customerService,
            IRepository<Download> downloadRepository)
        {
            _workContext = workContext;
            _poNumberModelFactory = poNumberModelFactory;
            _poNumberService = poNumberService;
            _mjsTradersWorkFlowMessageService = mjsTradersWorkFlowMessageService;
            _localizationSettings = localizationSettings;
            _downloadService = downloadService;
            _customerService = customerService;
            _downloadRepository = downloadRepository;
        }

        #endregion

        #region Methods

        #region List

        public virtual async Task<IActionResult> List()
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            var model = await _poNumberModelFactory.PreparePONumberSearchModelAsync(new PONumberSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> PONumberList(PONumberSearchModel searchModel)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            //prepare model
            var model = await _poNumberModelFactory.PreparePONumberListModelAsync(searchModel);

            return Json(model);
        }

        #endregion

        #region Update/Delete

        [HttpPost]
        public virtual async Task<IActionResult> PONumberUpdate(PONumberModel model)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            bool isTitleChanged = false;

            var poNumber = await _poNumberService.GetPONumberByIdAsync(model.Id);

            if (!poNumber.Title.Equals(model.Title, StringComparison.InvariantCultureIgnoreCase))
                isTitleChanged = true;

            var isApproveChange = false;

            if (poNumber.IsApproved != model.IsApproved)
                isApproveChange = true;

            poNumber.Title = model.Title;
            if (poNumber != null)
            {
                poNumber.IsApproved = model.IsApproved;
            }

            //Update PO number
            await _poNumberService.UpdatePONumberAsync(poNumber);

            if (isTitleChanged)
            {
                var download = await _downloadService.GetDownloadByIdAsync(poNumber.Id);
                download.Filename = poNumber.Title;

                await _downloadRepository.UpdateAsync(download);
            }

            if (isApproveChange)
            {
                //update approve on utc 
                poNumber.ApproveOnUtc = DateTime.UtcNow;
                await _poNumberService.UpdatePONumberAsync(poNumber);

                if (poNumber.IsApproved)
                    await _mjsTradersWorkFlowMessageService.SendCustomerApprovePoNumberNotificationAsync(poNumber, _localizationSettings.DefaultAdminLanguageId);
            }

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeletePO(int id)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            var poNumber = await _poNumberService.GetPONumberByIdAsync(id);
            if (poNumber != null)
            {

                // delete download 
                var download = await _downloadService.GetDownloadByIdAsync(poNumber.DownloadId);
                if (download != null)
                    await _downloadService.DeleteDownloadAsync(download);

                await _poNumberService.DeletePONumberAsync(poNumber);
            }

            return new NullJsonResult();
        }
        #endregion

        #endregion
    }
}
