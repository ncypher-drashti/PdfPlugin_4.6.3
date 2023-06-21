using FluentMigrator.Builders.Create.Table;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Mapping.Builders;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial  class SalesQuotationCustomerBuilder : NopEntityBuilder<SalesQuotationCustomer>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
               .WithColumn(nameof(SalesQuotationCustomer.Name)).AsString(100).Nullable();
        }

        #endregion
    }
}
