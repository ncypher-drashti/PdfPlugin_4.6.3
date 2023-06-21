using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PODocuments
{
    public partial record PODocumentModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Order")]
        public int OrderId { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Title")]
        public string Title1 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Download")]
        [UIHint("Download")]
        public int DownloadId1 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Title")]
        public string Title2 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Download")]
        [UIHint("Download")]
        public int DownloadId2 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Title")]
        public string Title3 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Download")]
        [UIHint("Download")]
        public int DownloadId3 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Title")]
        public string Title4 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Download")]
        [UIHint("Download")]
        public int DownloadId4 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Title")]
        public string Title5 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.Download")]
        [UIHint("Download")]
        public int DownloadId5 { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.CreateOnUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime CreateOnUtc { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.PODocument.UpdateOnUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? UpdateOnUtc { get; set; }
    }
}
