using Nop.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Model.PONumbers
{
    public record PONumberModel : BaseNopEntityModel
    {
        public string Title { get; set; }

        public int CustomerId { get; set; }

        public int DownloadId { get; set; }

        public bool IsDelete { get; set; }

        public bool IsApprove { get; set; }

        public DateTime CreateOnUtc { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? ApproveOnUtc { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? DeleteOnUtc { get; set; }
    }
}
