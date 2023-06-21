using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using System.Data;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping.Builders
{
    public partial class ProductCustomDetailBuilder : NopEntityBuilder<ProductCustomDetail>
    {
        #region Methods

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ProductCustomDetail.ProductId)).AsInt32().ForeignKey<Product>(onDelete: Rule.None);
        }

        #endregion
    }
}
