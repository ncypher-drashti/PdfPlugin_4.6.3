using Nop.Core;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public partial class SalesQuotationLine : BaseEntity
    {
        /// <summary>
        /// Gets or Sets sale quotation identification
        /// </summary>
        public int SaleQuotationId { get; set; }

        /// <summary>
        /// Gets or Sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets quantity
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or Sets price
        /// </summary>
        public string Price { get; set; }
    }
}
