using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.PODocuments
{
    public partial class PODocumentService : IPODocumentService
    {
        #region Fields

        private readonly IRepository<PODocument> _poDocumentRepository;

        #endregion

        #region Ctor

        public PODocumentService(IRepository<PODocument> poDocumentRepository)
        {
            _poDocumentRepository = poDocumentRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get PO document by identification
        /// </summary>
        /// <param name="id">PO document identification</param>
        /// <returns>PO document</returns>
        public virtual async Task<PODocument> GetPODocumentByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _poDocumentRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Get PO document by order identification
        /// </summary>
        /// <param name="orderId">Order identification</param>
        /// <returns>PO document</returns>
        public virtual async Task<PODocument> GetPODocumentByOrderIdAsync(int orderId)
        {
            if (orderId == 0)
                return null;

            var query = _poDocumentRepository.Table;

            query = query.Where(x => x.OrderId == orderId);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert PO Document data into table
        /// </summary>
        /// <param name="poDocument">PO document data</param>
        public virtual async Task InsertPODocumentAsync(PODocument poDocument)
        {
            if (poDocument == null)
                throw new ArgumentNullException();

            await _poDocumentRepository.InsertAsync(poDocument);
        }

        /// <summary>
        /// Update PO Document data into table
        /// </summary>
        /// <param name="poDocument">PO document data</param>
        public virtual async Task UpdatePODocumentAsync(PODocument poDocument)
        {
            if (poDocument == null)
                throw new ArgumentNullException();

            await _poDocumentRepository.UpdateAsync(poDocument);
        }

        /// <summary>
        /// Delete PO Document data from table
        /// </summary>
        /// <param name="poDocument">PO document data</param>
        public virtual async Task DeletePODocumentAsync(PODocument poDocument)
        {
            if (poDocument == null)
                throw new ArgumentNullException();

            await _poDocumentRepository.DeleteAsync(poDocument);
        }

        #endregion
    }
}