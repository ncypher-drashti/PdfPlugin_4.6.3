using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.PONumbers;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.PONumbers
{
    public partial interface IPONumberModelFactory
    {
        /// <summary>
        /// Prepare PO number list model
        /// </summary>
        /// <param name="searchModel">PO number list model</param>
        /// <returns>PO number list model</returns>
        Task<PONumberListModel> PreparePONumberListModelAsync(PONumberSearchModel searchModel);

        /// <summary>
        /// Prepare PO number search model
        /// </summary>
        /// <param name="searchModel">PO number search model</param>
        /// <returns>PO number search model</returns>
        Task<PONumberSearchModel> PreparePONumberSearchModelAsync(PONumberSearchModel searchModel);
    }
}
