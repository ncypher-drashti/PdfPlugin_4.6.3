using FluentMigrator;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Extensions;
using Nop.Data.Mapping;
using Nop.Data.Migrations;

namespace Nop.Cypher.Misc.MJSTraders.Data.Migrations
{
    [NopMigration("2021/09/27 11:24:16:2551771", "Create Table")]
    public class SchemaMigration : AutoReversingMigration
    {

        /// <summary>
        /// Collect the UP migration expressions
        /// <remarks>
        /// We use an explicit table creation order instead of an automatic one
        /// due to problems creating relationships between tables
        /// </remarks>
        /// </summary>
        public override void Up()
        {
            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(PODocument))).Exists())
                Create.TableFor<PODocument>();

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(PONumber))).Exists())
                Create.TableFor<PONumber>();

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SalesQuotationCustomer))).Exists())
                Create.TableFor<SalesQuotationCustomer>();

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(ProductCustomDetail))).Exists())
                Create.TableFor<ProductCustomDetail>();

            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SalesQuotation))).Exists())
                Create.TableFor<SalesQuotation>();
            
            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SalesQuotationDocument))).Exists())
                Create.TableFor<SalesQuotationDocument>();
            
            if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(SalesQuotationLine))).Exists())
                Create.TableFor<SalesQuotationLine>();
        }
    }
}
