using Nop.Cypher.Misc.MJSTraders.Domain;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.ProductCustomDetails
{
    public partial interface IProductCustomDetailService
    {
        /// <summary>
        /// Gets the product custom detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ProductCustomDetail</returns>
        Task<ProductCustomDetail> GetProductCustomDetailByIdAsync(int id);

        /// <summary>
        /// Gets the product custom detail by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>ProductCustomDetail</returns>
        Task<ProductCustomDetail> GetProductCustomDetailByProductIdAsync(int productId);

        /// <summary>
        /// Inserts the product custom detail.
        /// </summary>
        /// <param name="productCustomDetail">The product custom detail.</param>
        Task InsertProductCustomDetailAsync(ProductCustomDetail productCustomDetail);

        /// <summary>
        /// Updates the product custom detail.
        /// </summary>
        /// <param name="productCustomDetail">The product custom detail.</param>
        Task UpdateProductCustomDetailAsync(ProductCustomDetail productCustomDetail);

        /// <summary>
        /// Deletes the product custom detail.
        /// </summary>
        /// <param name="productCustomDetail">The product custom detail.</param>
        Task DeleteProductCustomDetailAsync(ProductCustomDetail productCustomDetail);
    }
}