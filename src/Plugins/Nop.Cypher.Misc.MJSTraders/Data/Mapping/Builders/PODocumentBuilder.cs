using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Orders;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using System.Data;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial class PODocumentBuilder : NopEntityBuilder<PODocument>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PODocument.OrderId)).AsInt32().ForeignKey<Order>(onDelete: Rule.None);
        }

        #endregion
    }
}
