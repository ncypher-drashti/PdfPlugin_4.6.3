using Nop.Core;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public class SalesQuotationCustomer : BaseEntity
    {
        /// <summary>
        /// Gets or Sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets company
        /// </summary>
        public string Company { get; set; }
    }
}
