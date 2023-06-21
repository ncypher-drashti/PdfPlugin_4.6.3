using Nop.Core;
using System;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public partial class PODocument : BaseEntity
    {
        /// <summary>
        /// Gets or Sets order identification
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets title 1
        /// </summary>
        public string Title1 { get; set; }

        /// <summary>
        /// Gets or Sets download file 1
        /// </summary>
        public int DownloadId1 { get; set; }

        /// <summary>
        /// Gets or sets title 2
        /// </summary>
        public string Title2 { get; set; }

        /// <summary>
        /// Gets or Sets download file 2
        /// </summary>
        public int DownloadId2 { get; set; }

        /// <summary>
        /// Gets or sets title 3
        /// </summary>
        public string Title3 { get; set; }

        /// <summary>
        /// Gets or Sets download file 3
        /// </summary>
        public int DownloadId3 { get; set; }

        /// <summary>
        /// Gets or sets title 4
        /// </summary>
        public string Title4 { get; set; }

        /// <summary>
        /// Gets or Sets download file 4
        /// </summary>
        public int DownloadId4 { get; set; }

        /// <summary>
        /// Gets or sets title 5
        /// </summary>
        public string Title5 { get; set; }

        /// <summary>
        /// Gets or Sets download file 5
        /// </summary>
        public int DownloadId5 { get; set; }

        /// <summary>
        /// Gets or Sets is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or Sets date time when PO document create
        /// </summary>
        public DateTime CreateOnUtc { get; set; }

        /// <summary>
        /// Gets or Sets date time when PO document update
        /// </summary>
        public DateTime? UpdateOnUtc { get; set; }
    }
}
