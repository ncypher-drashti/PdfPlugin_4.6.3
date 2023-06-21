using Nop.Web.Areas.Admin.Models.Orders;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.Orders
{
    public partial record CustomOrderModel : OrderModel
    {
        public string OrderPONumber { get; set; }
    }
}
