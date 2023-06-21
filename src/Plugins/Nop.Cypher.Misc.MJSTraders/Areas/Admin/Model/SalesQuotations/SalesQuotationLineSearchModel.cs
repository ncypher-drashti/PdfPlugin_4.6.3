using Nop.Web.Framework.Models;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public partial record SalesQuotationLineSearchModel : BaseSearchModel
    {
        #region Properties

        public int SaleQuotationId { get; set; }

        #endregion
    }
}
