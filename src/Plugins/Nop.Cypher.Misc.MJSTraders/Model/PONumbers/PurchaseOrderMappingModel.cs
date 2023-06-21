using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Cypher.Misc.MJSTraders.Model.PONumbers
{
    public partial class PurchaseOrderMappingModel
    {
        public PurchaseOrderMappingModel()
        {
            this.PurchaseOrders = new List<SelectListItem>();
        }

        public bool IsSelected { get; set; }

        public string PurchaseOrderName { get; set; }

        public int DownloadId { get; set; }
        public int PurchaseOrderId { get; set; }

        public IList<SelectListItem> PurchaseOrders { get; set; }
    }
}
