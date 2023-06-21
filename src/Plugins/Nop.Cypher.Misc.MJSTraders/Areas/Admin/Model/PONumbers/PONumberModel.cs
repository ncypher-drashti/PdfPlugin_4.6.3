using Nop.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PONumbers
{
    public record PONumberModel : BaseNopEntityModel
    {
        public string Title { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } 

        public string CompanyName { get; set; }

        public string CustomerEmail { get; set; } 

        public int DownloadId { get; set; }
        public Guid DownloadGuid { get; set; }

        public bool IsApproved { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime CreateOnUtc { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? ApproveOnUtc { get; set; }
    }
}
