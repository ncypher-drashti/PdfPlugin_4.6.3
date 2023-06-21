using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Domain;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.PONumbers
{
    public partial interface IPONumberService
    {
        /// <summary>
        /// Get PO number by identification
        /// </summary>
        /// <param name="id">PO number identification</param>
        /// <returns>PO number</returns>
        Task<PONumber> GetPONumberByIdAsync(int id);

        /// <summary>
        /// Get PO numbers
        /// </summary>
        /// <param name="customerId">Customer identification ; '0' return all record</param>
        /// <param name="title">PO number title ; 'null' return all record</param>
        /// <param name="customerName">Customer name ; 'null' return all record</param>
        /// <param name="customerEmail">Customer email ; 'null' return all record</param>
        /// <param name="isApprove">PO number approved; '0' return all record</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of PO number</returns>
        Task<IPagedList<PONumber>> GetPONumbersAsync(int customerId = 0, 
            string title = null, 
            string customerName = null, 
            string customerEmail = null, 
            string isApprove = "0", 
            int pageIndex = 0, 
            int pageSize = int.MaxValue);

        /// <summary>
        /// Insert PO number into table
        /// </summary>
        /// <param name="poNumber">PO number</param>
        Task InsertPONumberAsync(PONumber poNumber);

        /// <summary>
        /// Delete PO number from table
        /// </summary>
        /// <param name="poNumber">PO number</param>
        Task DeletePONumberAsync(PONumber poNumber);

        /// <summary>
        /// Update PO number into table
        /// </summary>
        /// <param name="poNumber">PO number</param>
        Task UpdatePONumberAsync(PONumber poNumber);
    }
}