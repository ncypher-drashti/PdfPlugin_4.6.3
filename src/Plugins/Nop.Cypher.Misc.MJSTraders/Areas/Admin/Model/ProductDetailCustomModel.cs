using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model
{
    public partial record ProductDetailCustomModel : BaseNopEntityModel
    {
        public int ProductId{ get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.ProductDetailCustom.ProductUnit")]
        public string ProductUnit { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.ProductDetailCustom.ProductPiecePerUnit")]
        public string ProductPiecePerUnit { get; set; }
    }
}
