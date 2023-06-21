using Nop.Cypher.Misc.MJSTraders.Model.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Domain;

namespace Nop.Cypher.Misc.MJSTraders.Factories.PODocuments
{
    public partial interface IPODocumentModelFactory
    {
        /// <summary>
        /// Prepare PO document model
        /// </summary>
        /// <param name="model">PO document model</param>
        /// <param name="poDocument">PO document</param>
        /// <returns>PO document model</returns>
        PODocumentModel PreparePODocumentModel(PODocumentModel model, PODocument poDocument);
    }
}
