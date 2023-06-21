using Nop.Cypher.Misc.MJSTraders.Model.PONumbers;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Factories.PONumbers
{
    public partial interface IPONumberModelFactory
    {
        /// <summary>
        /// Get PONumber for Customers
        /// </summary>
        /// <param name="customerId">Customer identification</param>
        /// <param name="page">Number of items page; pass null to load the first page</param>
        /// <returns>PO number list</returns>
        Task<PONumbersModel> PreparePONumbersModelAsync(int customerId, int? page);
    }
}
