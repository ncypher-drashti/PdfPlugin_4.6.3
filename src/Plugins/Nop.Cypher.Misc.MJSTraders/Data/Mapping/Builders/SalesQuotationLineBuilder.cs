using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Mapping.Builders;
using Nop.Data.Extensions;
using System.Data;
using FluentMigrator.Builders.Create.Table;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial class SalesQuotationLineBuilder : NopEntityBuilder<SalesQuotationLine>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
               .WithColumn(nameof(SalesQuotationLine.Name)).AsString(100).Nullable()
                .WithColumn(nameof(SalesQuotationLine.SaleQuotationId)).AsInt32().ForeignKey<SalesQuotation>(onDelete: Rule.None);
        }

        #endregion
    }
}
