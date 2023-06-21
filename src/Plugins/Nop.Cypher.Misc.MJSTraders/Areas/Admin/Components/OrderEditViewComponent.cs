using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components
{
    [ViewComponent(Name = "OrderEdit")]
    public partial class OrderEditViewComponent: NopViewComponent
    {
        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var orderModel = (OrderModel)additionalData;

            if (orderModel == null)
                return Content("");

            if (orderModel.Id == 0)
                return Content("");

            return View(orderModel);
        }

        #endregion
    }
}
