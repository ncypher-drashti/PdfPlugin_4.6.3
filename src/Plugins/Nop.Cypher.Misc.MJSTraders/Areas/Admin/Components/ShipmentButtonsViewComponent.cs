using Microsoft.AspNetCore.Mvc;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components
{
    [ViewComponent(Name = "ShipmentButtons")]
    public partial class ShipmentButtonsViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        #endregion

        #region ctor

        public ShipmentButtonsViewComponent(IOrderService orderService,
            ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            ShipmentModel shipmentModel = (ShipmentModel)additionalData;

            if (shipmentModel == null)
                return Content(string.Empty);

            if (shipmentModel.Id == 0)
                return Content(string.Empty);

            // check order customer is corporate customer
            var order = await _orderService.GetOrderByIdAsync(shipmentModel.OrderId);

            if (!await(await _customerService.GetCustomerByIdAsync(order.CustomerId)).IsPOCustomerAsync())
                return Content(string.Empty);

            return View(shipmentModel.Id);
        }

        #endregion
    }
}
