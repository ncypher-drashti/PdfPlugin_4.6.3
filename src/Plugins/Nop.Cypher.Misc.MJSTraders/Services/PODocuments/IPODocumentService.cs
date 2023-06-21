using Nop.Cypher.Misc.MJSTraders.Domain;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.PODocuments
{
    public partial interface IPODocumentService
    {
        /// <summary>
        /// Get PO document by identification
        /// </summary>
        /// <param name="id">PO document identification</param>
        /// <returns>PO document</returns>
        Task<PODocument> GetPODocumentByIdAsync(int id);

        /// <summary>
        /// Get PO document by order identification
        /// </summary>
        /// <param name="orderId">Order identification</param>
        /// <returns>PO document</returns>
        Task<PODocument> GetPODocumentByOrderIdAsync(int orderId);

        /// <summary>
        /// Insert PO Document data into table
        /// </summary>
        /// <param name="poDocument">PO document data</param>
        Task InsertPODocumentAsync(PODocument poDocument);

        /// <summary>
        /// Update PO Document data into table
        /// </summary>
        /// <param name="poDocument">PO document data</param>
        Task UpdatePODocumentAsync(PODocument poDocument);

        /// <summary>
        /// Delete PO Document data from table
        /// </summary>
        /// <param name="poDocument">PO document data</param>
        Task DeletePODocumentAsync(PODocument poDocument);
    }
}