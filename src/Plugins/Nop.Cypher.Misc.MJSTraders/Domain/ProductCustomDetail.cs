using Nop.Core;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public partial class ProductCustomDetail : BaseEntity
    {
        /// <summary>
        /// Gets or Sets product identification
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product unit.
        /// </summary>
        public string ProductUnit { get; set; }

        /// <summary>
        /// Gets or sets the product piece per unit.
        /// </summary>
        public string ProductPiecePerUnit { get; set; }
    }
}
