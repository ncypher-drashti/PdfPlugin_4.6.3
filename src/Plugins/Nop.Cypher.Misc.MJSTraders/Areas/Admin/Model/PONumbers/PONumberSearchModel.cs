using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PONumbers
{
    public record PONumberSearchModel : BaseSearchModel
    {
        public PONumberSearchModel()
        {
            AvailableApproveItem = new List<SelectListItem>();
            AvailableDeleteItem = new List<SelectListItem>();
        }

        #region Properties

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.PONumber.Fields.SearchTitle")]
        public string SearchTitle { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.PONumber.Fields.SearchCustomer")]
        public string SearchCustomer { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.PONumber.Fields.SearchEmail")]
        public string SearchEmail { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.PONumber.Fields.SearchIsApprove")]
        public string SearchIsApprove { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.PONumber.Fields.SearchIsDelete")]
        public string SearchIsDelete { get; set; }

        public IList<SelectListItem> AvailableApproveItem { get; set; }
        public IList<SelectListItem> AvailableDeleteItem { get; set; }

        #endregion
    }
}
