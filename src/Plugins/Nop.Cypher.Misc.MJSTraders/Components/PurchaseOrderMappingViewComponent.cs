using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Model.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Components
{
    [ViewComponent(Name = "PurchaseOrderMapping")]
    public class PurchaseOrderMappingViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IPONumberService _poNumberService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public PurchaseOrderMappingViewComponent(IWorkContext workContext,
            IStoreContext storeContext,
            IPONumberService poNumberService,
            ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _poNumberService = poNumberService;
            _localizationService = localizationService;
            _genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (_workContext.OriginalCustomerIfImpersonated == null)
            {
                // set 0 to last selected po
                await _genericAttributeService.SaveAttributeAsync(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, 0, (await _storeContext.GetCurrentStoreAsync()).Id);
                return Content("");
            }

            if (!await (await _workContext.GetCurrentCustomerAsync()).IsPOCustomerAsync())
                return Content("");

            var purchaseOrders = await _poNumberService.GetPONumbersAsync(customerId: customer.Id);

            if (!purchaseOrders.Any(x => x.IsApproved))
                return Content("");

            int lastPurchaseOrder = await _genericAttributeService.GetAttributeAsync<int>(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, (await _storeContext.GetCurrentStoreAsync()).Id);

            var model = new PurchaseOrderMappingModel();
            if (lastPurchaseOrder > 0)
            {
                var po = await _poNumberService.GetPONumberByIdAsync(lastPurchaseOrder);

                if (po != null)
                {
                    if (!po.IsApproved)
                    {
                        // set 0 to last selected po
                        await _genericAttributeService.SaveAttributeAsync(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, 0, (await _storeContext.GetCurrentStoreAsync()).Id);
                        lastPurchaseOrder = 0;
                    }
                    else
                    {
                        model.IsSelected = true;
                        model.PurchaseOrderName = po.Title;
                        model.PurchaseOrderId = po.Id;
                        model.DownloadId = po.DownloadId;
                    }
                }
                else
                {

                    // set 0 to last selected po
                    await _genericAttributeService.SaveAttributeAsync(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, 0, (await _storeContext.GetCurrentStoreAsync()).Id);
                    lastPurchaseOrder = 0;
                }
            }

            model.PurchaseOrders.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.OrderMapping.PurchaseOrder.Select")
            });

            foreach (var po in purchaseOrders.Where(x => x.IsApproved))
            {
                model.PurchaseOrders.Add(new SelectListItem
                {
                    Value = po.Id.ToString(),
                    Text = po.Title,
                    Selected = po.Id == lastPurchaseOrder
                });
            }

            return View(model);
        }

        #endregion
    }
}