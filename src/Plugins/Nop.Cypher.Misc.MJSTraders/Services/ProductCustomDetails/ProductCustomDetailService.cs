using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.ProductCustomDetails
{
    public partial class ProductCustomDetailService : IProductCustomDetailService
    {
        #region Fields

        private readonly IRepository<ProductCustomDetail> _productCustomDetailRepository;

        #endregion

        #region Ctor

        public ProductCustomDetailService(IRepository<ProductCustomDetail> productCustomDetailRepository)
        {
            _productCustomDetailRepository = productCustomDetailRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the product custom detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// ProductCustomDetail
        /// </returns>
        public virtual async Task<ProductCustomDetail> GetProductCustomDetailByIdAsync(int id)
        {
            if (id == 0)
                return null;

            return await _productCustomDetailRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Gets the product custom detail by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>
        /// ProductCustomDetail
        /// </returns>
        public virtual async Task<ProductCustomDetail> GetProductCustomDetailByProductIdAsync(int productId)
        {
            if (productId == 0)
                return null;

            var query = _productCustomDetailRepository.Table;

            query = query.Where(x => x.ProductId == productId);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts the product custom detail.
        /// </summary>
        /// <param name="productCustomDetail">The product custom detail.</param>
        public virtual async Task InsertProductCustomDetailAsync(ProductCustomDetail productCustomDetail)
        {
            if (productCustomDetail == null)
                throw new ArgumentNullException();

            await _productCustomDetailRepository.InsertAsync(productCustomDetail);
        }

        /// <summary>
        /// Updates the product custom detail.
        /// </summary>
        /// <param name="productCustomDetail">The product custom detail.</param>
        public virtual async Task UpdateProductCustomDetailAsync(ProductCustomDetail productCustomDetail)
        {
            if (productCustomDetail == null)
                throw new ArgumentNullException();

            await _productCustomDetailRepository.UpdateAsync(productCustomDetail);
        }

        /// <summary>
        /// Deletes the product custom detail.
        /// </summary>
        /// <param name="productCustomDetail">The product custom detail.</param>
        public virtual async Task DeleteProductCustomDetailAsync(ProductCustomDetail productCustomDetail)
        {
            if (productCustomDetail == null)
                throw new ArgumentNullException();

            await _productCustomDetailRepository.DeleteAsync(productCustomDetail);
        }

        #endregion
    }
}