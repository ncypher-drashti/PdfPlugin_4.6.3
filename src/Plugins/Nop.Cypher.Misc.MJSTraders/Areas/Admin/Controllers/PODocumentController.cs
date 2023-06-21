using Microsoft.AspNetCore.Mvc;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework;
using System;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class PODocumentController : BaseAdminController
    {
        #region Fields

        private readonly IPODocumentService _poDocumentService;
        private readonly ILocalizationService _localizationService;
        private readonly IDownloadService _downloadService;
        private readonly INotificationService _notificationService;

        #endregion

        #region Ctor

        public PODocumentController(IPODocumentService poDocumentService,
            ILocalizationService localizationService,
            IDownloadService downloadService,
            INotificationService notificationService)
        {
            _poDocumentService = poDocumentService;
            _localizationService = localizationService;
            _downloadService = downloadService;
            _notificationService = notificationService;
        }

        #endregion

        #region Methods

        [HttpPost]
        public virtual async Task<IActionResult> SavePODocumentDetails(PODocumentModel model)
        {
            if (model != null)
            {
                var poDocument = await _poDocumentService.GetPODocumentByOrderIdAsync(model.OrderId);

                if (poDocument == null)
                {
                    var newPODocument = new PODocument
                    {
                        OrderId = model.OrderId,
                        Title1 = model.Title1,
                        DownloadId1 = model.DownloadId1,
                        Title2 = model.Title2,
                        DownloadId2 = model.DownloadId2,
                        Title3 = model.Title3,
                        DownloadId3 = model.DownloadId3,
                        Title4 = model.Title4,
                        DownloadId4 = model.DownloadId4,
                        Title5 = model.Title5,
                        DownloadId5 = model.DownloadId5,
                        IsActive = model.IsActive,
                        CreateOnUtc = DateTime.UtcNow,
                    };

                    //Insert po document
                    await _poDocumentService.InsertPODocumentAsync(newPODocument);
                }
                else
                {
                    poDocument.Title1 = model.Title1;
                    poDocument.DownloadId1 = model.DownloadId1;
                    poDocument.Title2 = model.Title2;
                    poDocument.DownloadId2 = model.DownloadId2;
                    poDocument.Title3 = model.Title3;
                    poDocument.DownloadId3 = model.DownloadId3;
                    poDocument.Title4 = model.Title4;
                    poDocument.DownloadId4 = model.DownloadId4;
                    poDocument.Title5 = model.Title5;
                    poDocument.DownloadId5 = model.DownloadId5;
                    poDocument.IsActive = model.IsActive;
                    poDocument.UpdateOnUtc = DateTime.UtcNow;

                    //Update po document
                    await _poDocumentService.UpdatePODocumentAsync(poDocument);
                }

                // manage title
                if (string.IsNullOrWhiteSpace(poDocument.Title1) && poDocument.DownloadId1 > 0)
                {
                    var download = await _downloadService.GetDownloadByIdAsync(poDocument.DownloadId1);
                    poDocument.Title1 = download.Filename;
                }
                if (string.IsNullOrWhiteSpace(poDocument.Title2) && poDocument.DownloadId2 > 0)
                {
                    var download = await _downloadService.GetDownloadByIdAsync(poDocument.DownloadId2);
                    poDocument.Title2 = download.Filename;
                }
                if (string.IsNullOrWhiteSpace(poDocument.Title3) && poDocument.DownloadId3 > 0)
                {
                    var download = await _downloadService.GetDownloadByIdAsync(poDocument.DownloadId3);
                    poDocument.Title3 = download.Filename;
                }
                if (string.IsNullOrWhiteSpace(poDocument.Title4) && poDocument.DownloadId4 > 0)
                {
                    var download = await _downloadService.GetDownloadByIdAsync(poDocument.DownloadId4);
                    poDocument.Title4 = download.Filename;
                }
                if (string.IsNullOrWhiteSpace(poDocument.Title5) && poDocument.DownloadId5 > 0)
                {
                    var download = await _downloadService.GetDownloadByIdAsync(poDocument.DownloadId5);
                    poDocument.Title5 = download.Filename;
                }
                await _poDocumentService.UpdatePODocumentAsync(poDocument);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Notification.Success"));

                return Json(new
                {
                    Result = true,
                });
            }

            return Json(new
            {
                Result = false,
            });
        }

        #endregion
    }
}
