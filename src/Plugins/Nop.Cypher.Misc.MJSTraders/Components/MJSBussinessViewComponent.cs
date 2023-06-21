using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Extensions;
using Nop.Cypher.Misc.MJSTraders.Factories.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Model.MJSBussiness;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Components
{
    [ViewComponent(Name = "MJSBussiness")]
    public class MJSBussinessViewComponent : NopViewComponent
    {
        #region Fields
     
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public MJSBussinessViewComponent(ISettingService settingService,
            IStoreContext storeContext,
        IWorkContext workContext)
        {
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var _MJSTradersSetting = await _settingService.LoadSettingAsync<MJSTradersSettings>(storeScope);

            var model = new MJSBussinessModel();

            model.ChooseYourBusinessText = _MJSTradersSetting.ChooseYourBusinessText;
            model.LearnAboutText = _MJSTradersSetting.LearnAboutText;

            return View(model);
        }

        #endregion
    }
}
