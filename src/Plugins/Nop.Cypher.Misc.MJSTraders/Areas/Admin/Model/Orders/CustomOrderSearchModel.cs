using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.Orders
{
    public partial record CustomOrderSearchModel : OrderSearchModel
    {

        public CustomOrderSearchModel() : base()
        {
            this.AvailableOrderTypes = new List<SelectListItem>();
        }

        // order type
        // 1. online
        // 2. corporate ( po customer )

        public IList<SelectListItem> AvailableOrderTypes { get; set; }

        [NopResourceDisplayName("Admin.Orders.List.OrderType")]
        public string OrderType { get; set; }
    }
}
