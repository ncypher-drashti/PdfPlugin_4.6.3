using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Mapping.Builders;
using Nop.Data.Extensions;
using System.Data;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial class PONumberBuilder : NopEntityBuilder<PONumber>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PONumber.CustomerId)).AsInt32().ForeignKey<Customer>(onDelete: Rule.None);
        }

        #endregion
    }
}
