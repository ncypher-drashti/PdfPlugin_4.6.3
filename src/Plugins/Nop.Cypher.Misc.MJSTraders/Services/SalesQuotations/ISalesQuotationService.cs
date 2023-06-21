using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Domain;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations
{
    public partial interface ISalesQuotationService
    {
        #region Sales Quotation Customer

        /// <summary>
        /// Get sales quotation customer by identification
        /// </summary>
        /// <param name="id">identification</param>
        /// <returns>sales quotation customer</returns>
        Task<SalesQuotationCustomer> GetSalesQuotationCustomerByIdAsync(int id);

        /// <summary>
        /// Get sales quotation Customer
        /// </summary>
        /// <param name="name">Customer name ; 'null' return all record</param>
        /// <param name="email">Customer email ; 'null' return all record</param>
        /// <param name="company">Customer company ; 'null' return all record</param>
        /// <param name="title">Customer title ; 'null' return all record</param>
        /// <param name="referenceNumber">Customer reference Number ; 'null' return all record</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation</returns>
        Task<IPagedList<SalesQuotationCustomer>> GetSalesQuotationCustomerAsync(string name = null,
            string email = null, string company = null, string title = null, string referenceNumber = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Insert sales quation Customer into table
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quatation Customer</param>
        Task InsertSalesQuotationCustomerAsync(SalesQuotationCustomer salesQuotationCustomer);

        /// <summary>
        /// Update sales quation into table
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quatation Customer</param>
        Task UpdateSalesQuotationCustomerAsync(SalesQuotationCustomer salesQuotationCustomer);

        /// <summary>
        /// Delete sales quation from table
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quatation Customer</param>
        Task DeleteSalesQuotationCustomerAsync(SalesQuotationCustomer salesQuotationCustomer);

        #endregion

        #region Sales quotation

        /// <summary>
        /// Get sales quotation by identification
        /// </summary>
        /// <param name="id">identification</param>
        /// <returns>Sales quotation</returns>
        Task<SalesQuotation> GetSalesQuotationByIdAsync(int id);

        /// <summary>
        /// Get sales quotation by reference number
        /// </summary>
        /// <param name="referenceNumber">Reference number</param>
        /// <returns>Sales quotation</returns>
        Task<SalesQuotation> GetSalesQuotationByReferenceNumberAsync(string referenceNumber);

        /// <summary>
        /// Get sales quotation
        /// </summary>
        /// <param name="salesQuotationCustomerId">Sales quotation customer id ; '0' return none record</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation</returns>
        Task<IPagedList<SalesQuotation>> GetAllSalesQuotationsAsync(int salesQuotationCustomerId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Insert sales quation into table
        /// </summary>
        /// <param name="salesQuotation">Sales quatation</param>
        Task InsertSalesQuotationAsync(SalesQuotation salesQuotation);

        /// <summary>
        /// Update sales quation into table
        /// </summary>
        /// <param name="salesQuotation">Sales quatation</param>
        Task UpdateSalesQuotationAsync(SalesQuotation salesQuotation);

        /// <summary>
        /// Delete sales quation from table
        /// </summary>
        /// <param name="salesQuotation">Sales quatation</param>
        Task DeleteSalesQuotationAsync(SalesQuotation salesQuotation);

        #endregion

        #region Sales quotation line

        /// <summary>
        /// Get sales quotation line by identification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SalesQuotationLine> GetSalesQuotationLineByIdAsync(int id);

        /// <summary>
        /// Get sales quotation
        /// </summary>
        /// <param name="salesQuotationId">Sales quotation identification</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation</returns>
        Task<IPagedList<SalesQuotationLine>> GetSalesQuotationLineAsync(int salesQuotationId,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Insert sales quotation line data into table
        /// </summary>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        Task InsertSalesQuotationLineAsync(SalesQuotationLine salesQuotationLine);

        /// <summary>
        /// Update sales quotation line data into table
        /// </summary>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        Task UpdateSalesQuotationLineAsync(SalesQuotationLine salesQuotationLine);

        /// <summary>
        /// Delete sales quotation line data from table
        /// </summary>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        Task DeleteSalesQuotationLineAsync(SalesQuotationLine salesQuotationLine);

        #endregion

        #region Sales quotation document

        /// <summary>
        /// Get sales quotation doucment by identification
        /// </summary>
        /// <param name="saleQuatationDocumnetId">Sales quotation document identifire</param>
        /// <returns>Sales quotation document</returns>
        Task<SalesQuotationDocument> GetSalesQuotationDocumentByIdAsync(int saleQuatationDocumnetId);

        /// <summary>
        /// Get sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationId">Sales quotation identification</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation doucment</returns>
        Task<IPagedList<SalesQuotationDocument>> GetAllSalesQuotationDocumentsAsync(int salesQuotationId,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        /// <summary>
        /// Insert sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationDocument">Sales quotation document</param>
        Task InsertSalesQuotationDocumentAsync(SalesQuotationDocument salesQuotationDocument);

        /// <summary>
        /// Update sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationDocument">Sales quotation document</param>
        Task UpdateSalesQuotationDocumentAsync(SalesQuotationDocument salesQuotationDocument);

        /// <summary>
        /// Delete sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationDocument">Sales quotation document</param>
        Task DeleteSalesQuotationDocumentAsync(SalesQuotationDocument salesQuotationDocument);

        #endregion
    }
}