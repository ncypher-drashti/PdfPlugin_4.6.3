using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Components
{
    [ViewComponent(Name = "MyAccountMJSTraders")]
    public class MyAccountMJSTradersViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public MyAccountMJSTradersViewComponent(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return Content("");

            return View();
        }

        #endregion
    }
}
