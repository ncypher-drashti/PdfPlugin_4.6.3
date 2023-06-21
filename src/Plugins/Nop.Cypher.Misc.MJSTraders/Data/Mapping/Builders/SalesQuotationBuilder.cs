using FluentMigrator.Builders.Create.Table;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Mapping.Builders;
using Nop.Data.Extensions;
using System.Data;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial class SalesQuotationBuilder : NopEntityBuilder<SalesQuotation>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
               .WithColumn(nameof(SalesQuotation.ReferenceNumber)).AsString(10).NotNullable()
                .WithColumn(nameof(SalesQuotation.SalesQuotationCustomerId)).AsInt32().ForeignKey<SalesQuotationCustomer>(onDelete: Rule.None);
        }

        #endregion
    }
}
