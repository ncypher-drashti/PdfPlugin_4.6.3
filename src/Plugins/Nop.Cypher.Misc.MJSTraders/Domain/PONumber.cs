using Nop.Core;
using System;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public class PONumber: BaseEntity
    {
        /// <summary>
        /// Gets or Sets title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets Customer identification
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or Sets download identification
        /// </summary>
        public int DownloadId { get; set; }

        /// <summary>
        /// Gets or Sets is approve
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the date and time of PO number creation
        /// </summary>
        public DateTime CreateOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of PO number approbation
        /// </summary>
        public DateTime? ApproveOnUtc { get; set; }
      
    }
}
