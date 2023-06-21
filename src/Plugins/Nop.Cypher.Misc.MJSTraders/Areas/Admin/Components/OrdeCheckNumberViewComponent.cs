using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.Orders;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components
{
    [ViewComponent(Name = "OrderCheckNumber")]
    public partial class OrderCheckNumberViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreContext _storeContext;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public OrderCheckNumberViewComponent(IGenericAttributeService genericAttributeService,
            IStoreContext storeContext,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _genericAttributeService = genericAttributeService;
            _storeContext = storeContext;
            _orderService = orderService;
            _customerService = customerService;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var orderModel = (OrderModel)additionalData;

            if (orderModel == null)
                return Content(string.Empty);

            if (orderModel.Id == 0)
                return Content(string.Empty);

            // check order payment method
            if (orderModel.PaymentMethod == "Payments.CheckMoneyOrder" || orderModel.PaymentMethod == "Check / Money Order")
            {
                var order = await _orderService.GetOrderByIdAsync(orderModel.Id);
                if (order == null)
                    return Content(string.Empty);

                // this is only for po customer
                if (!await (await _customerService.GetCustomerByIdAsync(order.CustomerId)).IsPOCustomerAsync())
                    return Content(string.Empty);

                string checkNumber = await _genericAttributeService.GetAttributeAsync<string>(order, MJSTradersUtilities.CheckNumber, _storeContext.GetCurrentStore().Id);
                if (string.IsNullOrWhiteSpace(checkNumber))
                {
                    checkNumber = "N/A";
                }

                var model = new CheckNumberModel() { OrderId = orderModel.Id, CheckNumber = checkNumber };

                return View(model);
            }

            return Content(string.Empty);
        }

        #endregion
    }
}
