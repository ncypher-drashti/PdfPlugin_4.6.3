using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Model.PONumbers
{
    public record PONumbersModel : BaseNopModel
    {
        public PONumbersModel()
        {
            PONumbers = new List<PONumberModel>();
        }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.MyAccount.PONumber.Fields.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.MyAccount.PONumber.Fields.DownloadId")]
        [UIHint("Download")]
        public int DownloadId { get; set; }

        public int CustomerId { get; set; }

        public IList<PONumberModel> PONumbers { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested class

        /// <summary>
        /// Class that has only page for route value. Used for (My Account) PO numbers pagination
        /// </summary>
        public partial record CustomerPONumberRouteValues : IRouteValues
        {
            public int PageNumber { get; set; }
        }

        #endregion
    }
}
