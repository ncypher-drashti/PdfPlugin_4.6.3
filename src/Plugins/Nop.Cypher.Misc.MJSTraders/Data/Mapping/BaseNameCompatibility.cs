using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Data.Mapping;
using System;
using System.Collections.Generic;

namespace Nop.Cypher.Misc.MJSTraders.Data.Mapping
{
    /// <summary>
    /// Base instance of backward compatibility of table naming
    /// </summary>
    public partial class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>
        {
            { typeof(PODocument), "NC_MJST_PO_Document" },
            { typeof(PONumber), "NC_MJST_PO_Number" },
            { typeof(ProductCustomDetail), "NC_MJST_ProductCustoDetail" },
            { typeof(SalesQuotationCustomer), "NC_MJST_SalesQuotationCustomer" },
            { typeof(SalesQuotationDocument), "NC_MJST_SalesQuotationDocument" },
            { typeof(SalesQuotationLine), "NC_MJST_SalesQuotationLine" },
            { typeof(SalesQuotation), "NC_MJST_SalesQuotation" },
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>
        {
        };
    }
}
