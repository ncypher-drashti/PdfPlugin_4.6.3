using Nop.Web.Framework.Models;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public partial record SalesQuotationDocumentSearchModel : BaseSearchModel
    {
        #region Properties

        public int SaleQuotationId { get; set; }

        #endregion
    }
}
