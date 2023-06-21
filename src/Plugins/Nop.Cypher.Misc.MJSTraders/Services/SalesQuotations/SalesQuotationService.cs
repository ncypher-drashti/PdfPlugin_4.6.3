using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations
{
    public partial class SalesQuotationService : ISalesQuotationService
    {
        #region Fields

        private readonly IRepository<SalesQuotationCustomer> _salesQuotationCustomerRepository;
        private readonly IRepository<SalesQuotation> _salesQuotationRepository;
        private readonly IRepository<SalesQuotationLine> _salesQuotationLineRepository;
        private readonly IRepository<SalesQuotationDocument> _salesQuotationDocumentRepository;

        #endregion

        #region Ctor

        public SalesQuotationService(IRepository<SalesQuotationCustomer> salesQuotationCustomerRepository,
            IRepository<SalesQuotation> salesQuotationRepository,
            IRepository<SalesQuotationLine> salesQuotationLineRepository,
            IRepository<SalesQuotationDocument> salesQuotationDocumentRepository)
        {
            _salesQuotationCustomerRepository = salesQuotationCustomerRepository;
            _salesQuotationRepository = salesQuotationRepository;
            _salesQuotationLineRepository = salesQuotationLineRepository;
            _salesQuotationDocumentRepository = salesQuotationDocumentRepository;
        }

        #endregion

        #region Methods

        #region Sales Quotation Customer

        /// <summary>
        /// Get sales quotation customer by identification
        /// </summary>
        /// <param name="id">identification</param>
        /// <returns>sales quotation customer</returns>
        public virtual async Task<SalesQuotationCustomer> GetSalesQuotationCustomerByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _salesQuotationCustomerRepository.GetByIdAsync(id);
        }

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
        public virtual async Task<IPagedList<SalesQuotationCustomer>> GetSalesQuotationCustomerAsync(string name = null,
             string email = null, string company = null, string title = null, string referenceNumber = null,
             int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _salesQuotationCustomerRepository.Table;

            query = query.Where(x => x.Email != null);

            if (!string.IsNullOrEmpty(title))
            {
                query = from c in query
                        join sq in _salesQuotationRepository.Table
                             on c.Id equals sq.SalesQuotationCustomerId
                        where sq.Title.Contains(title)
                        select c;
            }

            if (!string.IsNullOrEmpty(referenceNumber))
            {
                query = from c in query
                        join sq in _salesQuotationRepository.Table
                             on c.Id equals sq.SalesQuotationCustomerId
                        where sq.ReferenceNumber == referenceNumber
                        select c;
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => x.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(company))
            {
                //query = query.Where(x => x.Company.Contains(company));
                query = from c in query
                        join sq in _salesQuotationRepository.Table
                             on c.Id equals sq.SalesQuotationCustomerId
                        where sq.Company.Contains(company) || c.Company.Contains(company)
                        select c;
            }
            query = query.OrderByDescending(x => x.Id).Distinct();

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Insert sales quation Customer into table
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quatation Customer</param>
        public virtual async Task InsertSalesQuotationCustomerAsync(SalesQuotationCustomer salesQuotationCustomer)
        {
            if (salesQuotationCustomer == null)
                throw new ArgumentNullException(nameof(salesQuotationCustomer));

            await _salesQuotationCustomerRepository.InsertAsync(salesQuotationCustomer);
        }

        /// <summary>
        /// Update sales quation into table
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quatation Customer</param>
        public virtual async Task UpdateSalesQuotationCustomerAsync(SalesQuotationCustomer salesQuotationCustomer)
        {
            if (salesQuotationCustomer == null)
                throw new ArgumentNullException(nameof(salesQuotationCustomer));

            await _salesQuotationCustomerRepository.UpdateAsync(salesQuotationCustomer);
        }

        /// <summary>
        /// Delete sales quation from table
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quatation Customer</param>
        public virtual async Task DeleteSalesQuotationCustomerAsync(SalesQuotationCustomer salesQuotationCustomer)
        {
            if (salesQuotationCustomer == null)
                throw new ArgumentNullException(nameof(salesQuotationCustomer));

            await _salesQuotationCustomerRepository.DeleteAsync(salesQuotationCustomer);
        }

        #endregion

        #region Sales quotation

        /// <summary>
        /// Get sales quotation by identification
        /// </summary>
        /// <param name="id">identification</param>
        /// <returns>Sales quotation</returns>
        public virtual async Task<SalesQuotation> GetSalesQuotationByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _salesQuotationRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get sales quotation by reference number
        /// </summary>
        /// <param name="referenceNumber">Reference number</param>
        /// <returns>Sales quotation</returns>
        public virtual async Task<SalesQuotation> GetSalesQuotationByReferenceNumberAsync(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber.Trim()))
                return null;

            return await _salesQuotationRepository.Table.Where(sq => sq.ReferenceNumber == referenceNumber).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get sales quotation
        /// </summary>
        /// <param name="salesQuotationCustomerId">Sales quotation customer id ; '0' return none record</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation</returns>
        public virtual async Task<IPagedList<SalesQuotation>> GetAllSalesQuotationsAsync(int salesQuotationCustomerId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _salesQuotationRepository.Table;

            if (salesQuotationCustomerId > 0)
                query = query.Where(x => x.SalesQuotationCustomerId == salesQuotationCustomerId);

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Insert sales quation into table
        /// </summary>
        /// <param name="salesQuotation">Sales quatation</param>
        public virtual async Task InsertSalesQuotationAsync(SalesQuotation salesQuotation)
        {
            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            await _salesQuotationRepository.InsertAsync(salesQuotation);
        }

        /// <summary>
        /// Update sales quation into table
        /// </summary>
        /// <param name="salesQuotation">Sales quatation</param>
        public virtual async Task UpdateSalesQuotationAsync(SalesQuotation salesQuotation)
        {
            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            await _salesQuotationRepository.UpdateAsync(salesQuotation);
        }

        /// <summary>
        /// Delete sales quation from table
        /// </summary>
        /// <param name="salesQuotation">Sales quatation</param>
        public virtual async Task DeleteSalesQuotationAsync(SalesQuotation salesQuotation)
        {
            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            await _salesQuotationRepository.DeleteAsync(salesQuotation);
        }

        #endregion

        #region Sales quotation line

        /// <summary>
        /// Get sales quotation line by identification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<SalesQuotationLine> GetSalesQuotationLineByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _salesQuotationLineRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get sales quotation
        /// </summary>
        /// <param name="salesQuotationId">Sales quotation identification</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation</returns>
        public virtual async Task<IPagedList<SalesQuotationLine>> GetSalesQuotationLineAsync(int salesQuotationId,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _salesQuotationLineRepository.Table;

            query = query.Where(x => x.SaleQuotationId == salesQuotationId);

            return await query.OrderByDescending(x => x.Id).ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Insert sales quotation line data into table
        /// </summary>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        public virtual async Task InsertSalesQuotationLineAsync(SalesQuotationLine salesQuotationLine)
        {
            if (salesQuotationLine == null)
                throw new ArgumentNullException(nameof(salesQuotationLine));

            await _salesQuotationLineRepository.InsertAsync(salesQuotationLine);
        }

        /// <summary>
        /// Update sales quotation line data into table
        /// </summary>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        public virtual async Task UpdateSalesQuotationLineAsync(SalesQuotationLine salesQuotationLine)
        {
            if (salesQuotationLine == null)
                throw new ArgumentNullException(nameof(salesQuotationLine));

            await _salesQuotationLineRepository.UpdateAsync(salesQuotationLine);
        }

        /// <summary>
        /// Delete sales quotation line data from table
        /// </summary>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        public virtual async Task DeleteSalesQuotationLineAsync(SalesQuotationLine salesQuotationLine)
        {
            if (salesQuotationLine == null)
                throw new ArgumentNullException(nameof(salesQuotationLine));

            await _salesQuotationLineRepository.DeleteAsync(salesQuotationLine);
        }

        #endregion

        #region Sales quotation document

        /// <summary>
        /// Get sales quotation doucment by identification
        /// </summary>
        /// <param name="saleQuatationDocumnetId">Sales quotation document identifire</param>
        /// <returns>Sales quotation document</returns>
        public virtual async Task<SalesQuotationDocument> GetSalesQuotationDocumentByIdAsync(int saleQuatationDocumnetId)
        {
            if (saleQuatationDocumnetId == 0)
                return null;

            return await _salesQuotationDocumentRepository.GetByIdAsync(saleQuatationDocumnetId);
        }

        /// <summary>
        /// Get sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationId">Sales quotation identification</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of sales quotation doucment</returns>
        public virtual async Task<IPagedList<SalesQuotationDocument>> GetAllSalesQuotationDocumentsAsync(int salesQuotationId,
             int pageIndex = 0,
             int pageSize = int.MaxValue)
        {
            if (salesQuotationId == 0)
                return null;
            var query = _salesQuotationDocumentRepository.Table.Where(s => s.SaleQuotationId == salesQuotationId);

            return await query.OrderByDescending(x => x.Id).ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Insert sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationDocument">Sales quotation document</param>
        public virtual async Task InsertSalesQuotationDocumentAsync(SalesQuotationDocument salesQuotationDocument)
        {
            if (salesQuotationDocument == null)
                throw new ArgumentNullException(nameof(salesQuotationDocument));

            await _salesQuotationDocumentRepository.InsertAsync(salesQuotationDocument);
        }

        /// <summary>
        /// Update sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationDocument">Sales quotation document</param>
        public virtual async Task UpdateSalesQuotationDocumentAsync(SalesQuotationDocument salesQuotationDocument)
        {
            if (salesQuotationDocument == null)
                throw new ArgumentNullException(nameof(salesQuotationDocument));

            await _salesQuotationDocumentRepository.UpdateAsync(salesQuotationDocument);
        }

        /// <summary>
        /// Delete sales quotation doucment
        /// </summary>
        /// <param name="salesQuotationDocument">Sales quotation document</param>
        public virtual async Task DeleteSalesQuotationDocumentAsync(SalesQuotationDocument salesQuotationDocument)
        {
            if (salesQuotationDocument == null)
                throw new ArgumentNullException(nameof(salesQuotationDocument));

            await _salesQuotationDocumentRepository.DeleteAsync(salesQuotationDocument);
        }

        #endregion

        #endregion
    }
}