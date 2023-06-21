using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Factories.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Order;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Components
{
    [ViewComponent(Name = "PODocumentOrderDetail")]
    public class PODocumentOrderDetailViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IPODocumentService _poDocumentService;
        private readonly IPODocumentModelFactory _poDocumentModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public PODocumentOrderDetailViewComponent(IPODocumentService poDocumentService,
            IPODocumentModelFactory poDocumentModelFactory,
            IWorkContext workContext)
        {
            _poDocumentService = poDocumentService;
            _poDocumentModelFactory = poDocumentModelFactory;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var orderDetailsModel = (OrderDetailsModel)additionalData;

            if (orderDetailsModel == null || orderDetailsModel.Id == 0)
                return Content("");

            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return Content("");

            var poDocument = await _poDocumentService.GetPODocumentByOrderIdAsync(orderDetailsModel.Id);

            if (poDocument == null)
                return Content("");

            //prepare po document model
            var model = _poDocumentModelFactory.PreparePODocumentModel(null, poDocument);

            return View(model);
        }

        #endregion
    }
}
