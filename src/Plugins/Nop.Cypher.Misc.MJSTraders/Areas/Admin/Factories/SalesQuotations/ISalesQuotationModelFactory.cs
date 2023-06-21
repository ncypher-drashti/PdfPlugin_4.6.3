using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations;
using Nop.Cypher.Misc.MJSTraders.Domain;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.SalesQuotations
{
    public partial interface ISalesQuotationModelFactory
    {
        #region Sales quotation Customer

        /// <summary>
        /// Prepare sales quotaion list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion list model</param>
        /// <returns>Sales quotation list model</returns>
        Task<SalesQuotationCustomerListModel> PrepareSalesQuotationCustomerListModelAsync(SalesQuotationCustomerSearchModel searchModel);

        /// <summary>
        /// Prepare sales quotation search model
        /// </summary>
        /// <param name="searchModel">Sales quotation search model</param>
        /// <returns>Sales quotation search model</returns>
        SalesQuotationCustomerSearchModel PrepareSalesQuotationCustomerSearchModel(SalesQuotationCustomerSearchModel searchModel);

        /// <summary>
        /// Prepare sales quotation model
        /// </summary>
        /// <param name="model">Sales quotation model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation model</returns>
        SalesQuotationCustomerModel PrepareSalesQuotationCustomerModel(SalesQuotationCustomerModel model, SalesQuotationCustomer salesQuotation);

        #endregion

        #region Sales quotation

        /// <summary>
        /// Prepare sales quotaion list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion list model</param>
        /// <returns>Sales quotation list model</returns>
        Task<SalesQuotationListModel> PrepareSalesQuotationListModelAsync(SalesQuotationSearchModel searchModel);

        /// <summary>
        /// Prepare sales quotation search model
        /// </summary>
        /// <param name="searchModel">Sales quotation search model</param>
        /// <returns>Sales quotation search model</returns>
        SalesQuotationSearchModel PrepareSalesQuotationSearchModel(SalesQuotationSearchModel searchModel);

        /// <summary>
        /// Prepare sales quotation model
        /// </summary>
        /// <param name="salesQuotationCustomer">Sales quotation customer</param>
        /// <param name="model">Sales quotation model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation model</returns>
        Task<SalesQuotationModel> PrepareSalesQuotationModelAsync(SalesQuotationCustomer salesQuotationCustomer, SalesQuotationModel model, SalesQuotation salesQuotation);

        /// <summary>
        /// Send 'send sales quotation' notifications 
        /// </summary>
        /// <param name="salesQuotation">Sales quotation</param>
        Task SendNotificationAsync(SalesQuotation salesQuotation);

        #endregion

        #region Sales quotation line

        /// <summary>
        /// Prepare sales quotaion line list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion line list model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation line list model</returns>
        Task<SalesQuotationLineListModel> PrepareSalesQuotationLineListModelAsync(SalesQuotationLineSearchModel searchModel, SalesQuotation salesQuotation);

        /// <summary>
        /// Prepare sales quotation line search model
        /// </summary>
        /// <param name="searchModel">Sales quotation line search model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation search line model</returns>
        SalesQuotationLineSearchModel PrepareSalesQuotationLineSearchModel(SalesQuotationLineSearchModel searchModel, SalesQuotation salesQuotation);

        /// <summary>
        /// Prepare sales quotation line model
        /// </summary>
        /// <param name="model">Sales quotation line model</param>
        /// <param name="salesQuotationLine">Sales quotation line</param>
        /// <returns>Sales quotation line model</returns>
        SalesQuotationLineModel PrepareSalesQuotationLineModel(SalesQuotationLineModel model, SalesQuotationLine salesQuotationLine);

        #endregion

        #region Sale quotation documnet

        /// <summary>
        /// Prepare sales quotaion document list model
        /// </summary>
        /// <param name="searchModel">Sales quotaion document list model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation document list model</returns>
        Task<SalesQuotationDocumentListModel> PrepareSalesQuotationDocumentListModelAsync(SalesQuotationDocumentSearchModel searchModel, SalesQuotation salesQuotation);

        /// <summary>
        /// Prepare sales quotation line search model
        /// </summary>
        /// <param name="searchModel">Sales quotation document search model</param>
        /// <param name="salesQuotation">Sales quotaion</param>
        /// <returns>Sales quotation search document model</returns>
        SalesQuotationDocumentSearchModel PrepareSalesQuotationDocumentSearchModel(SalesQuotationDocumentSearchModel searchModel, SalesQuotation salesQuotation);

        #endregion
    }
}
