using Nop.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Model.PODocuments
{
    public partial record PODocumentModel : BaseNopEntityModel
    {
        public int OrderId { get; set; }

        public string Title1 { get; set; }

        [UIHint("Download")]
        public int DownloadId1 { get; set; }

        public string Title2 { get; set; }

        [UIHint("Download")]
        public int DownloadId2 { get; set; }

        public string Title3 { get; set; }

        [UIHint("Download")]
        public int DownloadId3 { get; set; }

        public string Title4 { get; set; }

        [UIHint("Download")]
        public int DownloadId4 { get; set; }

        public string Title5 { get; set; }

        [UIHint("Download")]
        public int DownloadId5 { get; set; }

        public bool IsActive { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime CreateOnUtc { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? UpdateOnUtc { get; set; }
    }
}
