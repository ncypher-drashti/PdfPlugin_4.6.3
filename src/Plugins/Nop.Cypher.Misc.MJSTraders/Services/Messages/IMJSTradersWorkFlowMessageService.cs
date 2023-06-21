using Nop.Cypher.Misc.MJSTraders.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.Messages
{
    public partial interface IMJSTradersWorkFlowMessageService
    {
        /// <summary>
        /// Sends a approve PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<IList<int>> SendCustomerApprovePoNumberNotificationAsync(PONumber poNumber, int languageId);

        /// <summary>
        /// Sends a delete PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<IList<int>> SendCustomerDeletePoNumberNotificationAsync(PONumber poNumber, int languageId);

        /// <summary>
        /// Sends a submit PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<IList<int>> SendCustomerUploadPoNumberNotificationAsync(PONumber poNumber, int languageId);

        /// <summary>
        /// Sends a submit PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        Task<IList<int>> SendAdminUploadPoNumberNotificationAsync(PONumber poNumber, int languageId);

        /// <summary>
        /// Sends a sales quotation notification
        /// </summary>
        /// <param name="salesQuotation">Sales quotation</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <returns>Queued email identifier</returns>
        Task<IList<int>> SendSalesQuotationCustomerNotificationAsync(SalesQuotation salesQuotation, int languageId, string attachmentFilePath = null, string attachmentFileName = null);
    }
}
