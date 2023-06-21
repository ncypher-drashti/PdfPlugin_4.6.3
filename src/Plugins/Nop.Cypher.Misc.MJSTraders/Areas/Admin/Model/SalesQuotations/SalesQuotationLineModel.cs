using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public partial record SalesQuotationLineModel : BaseNopEntityModel
    {
        public int SaleQuotationId { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationLine.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationLine.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationLine.Qty")]
        public int Qty { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationLine.Price")]
        public string Price { get; set; }
    }
}
