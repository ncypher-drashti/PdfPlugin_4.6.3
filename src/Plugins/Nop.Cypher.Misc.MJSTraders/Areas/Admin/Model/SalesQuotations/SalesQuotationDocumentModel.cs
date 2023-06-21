using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public partial record SalesQuotationDocumentModel : BaseNopEntityModel
    {
        public int SaleQuotationId { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotationDocument.Download")]
        [UIHint("Download")]
        public int DownloadId { get; set; }
        public Guid DownloadGuid { get; set; }

        public string FileName { get; set; }
    }
}
