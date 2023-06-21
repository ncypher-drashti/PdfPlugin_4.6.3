using Nop.Web.Framework.Models;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public record SalesQuotationSearchModel : BaseSearchModel
    {
        #region Properties

        public int SalesQuotationCustomerId { get; set; }

        #endregion
    }
}
