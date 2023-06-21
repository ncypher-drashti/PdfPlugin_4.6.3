using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Factories.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Components
{
    [ViewComponent(Name = "OrderListOrderPurchaseOrderNumber")]
    public class OrderListOrderPurchaseOrderNumberViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IPODocumentService _poDocumentService;
        private readonly IPODocumentModelFactory _poDocumentModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public OrderListOrderPurchaseOrderNumberViewComponent(IPODocumentService poDocumentService,
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
            int orderId = (int)additionalData;

            if (orderId == 0)
                return Content("");

            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return Content("");

            var poDocument = await _poDocumentService.GetPODocumentByOrderIdAsync(orderId);

            if (poDocument == null)
                return Content("");

            //prepare po document model
            var model = _poDocumentModelFactory.PreparePODocumentModel(null, poDocument);

            return View(model);
        }

        #endregion
    }
}
