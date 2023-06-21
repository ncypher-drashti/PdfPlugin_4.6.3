using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public record SalesQuotationCustomerModel : BaseNopEntityModel
    {
        public SalesQuotationCustomerModel()
        {
            SalesQuotationSearchModel = new SalesQuotationSearchModel();
        }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Company")]
        public string Company { get; set; }

        public SalesQuotationSearchModel SalesQuotationSearchModel { get; set; }
    }
}
