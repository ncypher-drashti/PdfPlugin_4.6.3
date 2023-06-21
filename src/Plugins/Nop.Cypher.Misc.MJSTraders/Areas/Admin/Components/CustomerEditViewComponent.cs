using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Framework.Components;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components
{
    [ViewComponent(Name = "CustomerEdit")]
    public partial class CustomerEditViewComponent : NopViewComponent
    {
        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var model = (CustomerModel)additionalData;

            if (model == null)
                return Content("");

            return View(model);
        }

        #endregion
    }
}
