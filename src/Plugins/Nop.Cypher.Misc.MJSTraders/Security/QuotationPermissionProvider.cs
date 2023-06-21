using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Services.Security;
using System;
using System.Collections.Generic;

namespace Nop.Cypher.Misc.MJSTraders.Security
{
    /// <summary>
    /// Quotation permission provider
    /// </summary>
    public class QuotationPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessSalesQuotation = new PermissionRecord { Name = "Sales Quotation. Quotation", SystemName = "SalesQuotation", Category = "Sales" };

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessSalesQuotation,
            };
        }

        public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    NopCustomerDefaults.AdministratorsRoleName,
                    new[]
                    {
                        AccessSalesQuotation,
                    }
                ),
            };
        }
    }
}
