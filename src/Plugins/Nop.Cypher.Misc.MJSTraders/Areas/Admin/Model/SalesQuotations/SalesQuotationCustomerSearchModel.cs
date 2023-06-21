using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public record SalesQuotationCustomerSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.SalesQuotationCustomer.Fields.SearchName")]
        public string SearchName { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.SalesQuotationCustomer.Fields.SearchEmail")]
        public string SearchEmail { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.SalesQuotationCustomer.Fields.SearchCompany")]
        public string SearchCompany { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.SalesQuotationCustomer.Fields.SearchTitle")]
        public string SearchTitle { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.SalesQuotationCustomer.Fields.SearchReferenceNumber")]
        public string SearchReferenceNumber { get; set; }
    }
}
