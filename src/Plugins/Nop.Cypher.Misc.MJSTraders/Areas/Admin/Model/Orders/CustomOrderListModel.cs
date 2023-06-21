using Nop.Web.Framework.Models;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.Orders
{
    /// <summary>
    /// Represents an order list model
    /// </summary>
    public partial record CustomOrderListModel : BasePagedListModel<CustomOrderModel>
    {
    }
}
