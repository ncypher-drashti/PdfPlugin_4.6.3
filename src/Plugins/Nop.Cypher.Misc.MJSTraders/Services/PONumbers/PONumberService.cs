using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data;
using Nop.Services.Customers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.PONumbers
{
    public partial class PONumberService : IPONumberService
    {
        #region Fields

        private readonly IRepository<PONumber> _poNumberRepository;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public PONumberService(IRepository<PONumber> poNumberRepository,
            ICustomerService customerService)
        {
            _poNumberRepository = poNumberRepository;
            _customerService = customerService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get PO number by identification
        /// </summary>
        /// <param name="id">PO number identification</param>
        /// <returns>PO number</returns>
        public virtual async Task<PONumber> GetPONumberByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _poNumberRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get PO numbers
        /// </summary>
        /// <param name="customerId">Customer identification ; '0' return all record</param>
        /// <param name="title">PO number title ; 'null' return all record</param>
        /// <param name="customerName">Customer name ; 'null' return all record</param>
        /// <param name="customerEmail">Customer email ; 'null' return all record</param>
        /// <param name="isApprove">PO number approved; '0' return all record</param>
        /// <param name="isDelete">PO number deleted ; '0' return all record</param>
        /// <param name="displayDeleted">Display deleted record</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of PO number</returns>
        public virtual async Task<IPagedList<PONumber>> GetPONumbersAsync(int customerId = 0,
            string title = null,
            string customerName = null,
            string customerEmail = null,
            string isApprove = "0",
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _poNumberRepository.Table;

            if (customerId != 0)
                query = query.Where(x => x.CustomerId == customerId);

            if (title != null)
                query = query.Where(x => x.Title.Contains(title));

            if (customerName != null)
            {
                var customer = await _customerService.GetAllCustomersAsync(firstName: customerName);
                query = query.Where(x => customer.Any(c => c.Id == x.CustomerId));
            }

            if (customerEmail != null)
            {
                var customer = await _customerService.GetAllCustomersAsync(email: customerEmail);
                query = query.Where(x => customer.Any(c => c.Id == x.CustomerId));
            }

            if (isApprove != "0")
                query = query.Where(x => x.IsApproved == Convert.ToBoolean(isApprove));

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Insert PO number into table
        /// </summary>
        /// <param name="poNumber">PO number</param>
        public virtual async Task InsertPONumberAsync(PONumber poNumber)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            await _poNumberRepository.InsertAsync(poNumber);
        }

        /// <summary>
        /// Delete PO number from table
        /// </summary>
        /// <param name="poNumber">PO number</param>
        public virtual async Task DeletePONumberAsync(PONumber poNumber)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            await _poNumberRepository.DeleteAsync(poNumber);
        }

        /// <summary>
        /// Update PO number into table
        /// </summary>
        /// <param name="poNumber">PO number</param>
        public virtual async Task UpdatePONumberAsync(PONumber poNumber)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            await _poNumberRepository.UpdateAsync(poNumber);
        }

        #endregion
    }
}