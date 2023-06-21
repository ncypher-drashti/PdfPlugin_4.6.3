using Nop.Core;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public partial class SalesQuotationDocument : BaseEntity
    {
        /// <summary>
        /// Gets or Sets sale quotation identification
        /// </summary>
        public int SaleQuotationId { get; set; }

        /// <summary>
        /// Gets or Sets download identification    
        /// </summary>
        public int DownloadId { get; set; }
    }
}
