using FluentMigrator.Builders.Create.Table;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using System.Data;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial  class SalesQuotationDocumentBuilder : NopEntityBuilder<SalesQuotationDocument>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(SalesQuotationDocument.SaleQuotationId)).AsInt32().ForeignKey<SalesQuotation>(onDelete: Rule.None);
        }

        #endregion
    }
}
