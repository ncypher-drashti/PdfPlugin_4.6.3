using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components
{
    [ViewComponent(Name = "PODocument")]
    public partial class PODocumentViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IPODocumentService _poDocumentService;
        private readonly IPODocumentModelFactory _poDocumentModelFactory;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public PODocumentViewComponent(IWorkContext workContext,
            IPODocumentService poDocumentService,
            IPODocumentModelFactory poDocumentModelFactory,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _workContext = workContext;
            _poDocumentService = poDocumentService;
            _poDocumentModelFactory = poDocumentModelFactory;
            _orderService = orderService;
            _customerService = customerService;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return Content("");

            if (orderId == 0)
                return Content("");

            // order
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (!await (await _customerService.GetCustomerByIdAsync(order.CustomerId)).IsPOCustomerAsync())
                return Content("");

            var poDocument = await _poDocumentService.GetPODocumentByOrderIdAsync(orderId);
            var model = new PODocumentModel();

            if (poDocument != null)
            {
                model = _poDocumentModelFactory.PreparePODocumentModel(null, poDocument);
            }
            else
            {
                model.OrderId = orderId;
            }

            return View(model);
        }

        #endregion
    }
}
