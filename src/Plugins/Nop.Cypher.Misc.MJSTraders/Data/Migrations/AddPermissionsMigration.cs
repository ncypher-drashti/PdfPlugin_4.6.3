using FluentMigrator;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Data;
using Nop.Data.Migrations;
using System;
using System.Linq;

namespace Nop.Cypher.Misc.MJSTraders.Data.Migrations
{
    [NopMigration("2022/07/07 09:52:16:2551771", "Add Permissions")]
    public class AddPermissionsMigration : AutoReversingMigration
    {
        private readonly INopDataProvider _dataProvider;

        public AddPermissionsMigration(INopDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public override void Up()
        {
            if (!_dataProvider.GetTable<PermissionRecord>().Any(pr => string.Compare(pr.SystemName, "SalesQuotation", StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                var salesQuotationPermission = _dataProvider.InsertEntity(
                    new PermissionRecord
                    {
                        Name = "Sales Quotation. Quotation",
                        SystemName = "SalesQuotation",
                        Category = "Sales"
                    }
                );

                //add it to the register role by default
                var adminRole = _dataProvider
                    .GetTable<CustomerRole>()
                    .FirstOrDefault(x => x.IsSystemRole && x.SystemName == NopCustomerDefaults.AdministratorsRoleName);

                _dataProvider.InsertEntity(
                    new PermissionRecordCustomerRoleMapping
                    {
                        CustomerRoleId = adminRole.Id,
                        PermissionRecordId = salesQuotationPermission.Id
                    }
                );
            }
        }
    }
}
