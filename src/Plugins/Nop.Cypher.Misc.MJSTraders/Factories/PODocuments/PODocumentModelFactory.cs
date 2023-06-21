using Nop.Cypher.Misc.MJSTraders.Model.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Domain;

namespace Nop.Cypher.Misc.MJSTraders.Factories.PODocuments
{
    public partial class PODocumentModelFactory : IPODocumentModelFactory
    {
        /// <summary>
        /// Prepare PO document model
        /// </summary>
        /// <param name="model">PO document model</param>
        /// <param name="poDocument">PO document</param>
        /// <returns>PO document model</returns>
        public PODocumentModel PreparePODocumentModel(PODocumentModel model, PODocument poDocument)
        {
            if (poDocument != null)
            {
                model = model ?? new PODocumentModel
                {
                    Id = poDocument.Id
                };

                model.OrderId = poDocument.OrderId;
                model.Title1 = poDocument.Title1;
                model.DownloadId1 = poDocument.DownloadId1;
                model.Title2 = poDocument.Title2;
                model.DownloadId2 = poDocument.DownloadId2;
                model.Title3 = poDocument.Title3;
                model.DownloadId3 = poDocument.DownloadId3;
                model.Title4 = poDocument.Title4;
                model.DownloadId4 = poDocument.DownloadId4;
                model.Title5 = poDocument.Title5;
                model.DownloadId5 = poDocument.DownloadId5;
                model.IsActive = poDocument.IsActive;
                model.CreateOnUtc = poDocument.CreateOnUtc;
                model.UpdateOnUtc = poDocument.UpdateOnUtc;
            }

            return model;
        }
    }
}
