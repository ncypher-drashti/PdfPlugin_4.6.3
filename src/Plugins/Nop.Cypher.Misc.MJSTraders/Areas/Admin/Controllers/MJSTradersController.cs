using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Services.ProductCustomDetails;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Filters;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    public class MJSTradersController : BaseAdminController
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IProductCustomDetailService _productCustomDetailService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public MJSTradersController(IWorkContext workContext,
            IStoreContext storeContext,
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService,
            IBaseAdminModelFactory baseAdminModelFactory,
            IProductCustomDetailService productCustomDetailService,
            ICustomerService customerService)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _productCustomDetailService = productCustomDetailService;
            _customerService = customerService;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        public virtual async Task<IActionResult> Configure()
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var _MJSTradersSetting = await _settingService.LoadSettingAsync<MJSTradersSettings>(storeScope);

            var model = new ConfigurationModel
            {
                Enable = _MJSTradersSetting.Enable,
                POCustomerId = _MJSTradersSetting.POCustomerId,
                PONumberPageSizeOnAccountPage = _MJSTradersSetting.PONumberPageSizeOnAccountPage,
                AbandonedMessageTitleContent = _MJSTradersSetting.AbandonedMessageTitleContent,
                CartOfferCustomerRoleId = _MJSTradersSetting.CartOfferCustomerRoleId,
                OfferTime = _MJSTradersSetting.OfferTime,

                MJSTradersLogoId = _MJSTradersSetting.MJSTradersLogoId,
                MJSTradersNTNNumber = _MJSTradersSetting.MJSTradersNTNNumber,
                MJSTradersSTRNNo = _MJSTradersSetting.MJSTradersSTRNNo,
                MJSEnterpriceLogoId = _MJSTradersSetting.MJSEnterpriceLogoId,
                MJSEnterpriceNTNNumber = _MJSTradersSetting.MJSEnterpriceNTNNumber,
                MJSEnterpriceSTRNNo = _MJSTradersSetting.MJSEnterpriceSTRNNo,
                ChooseYourBusinessText = _MJSTradersSetting.ChooseYourBusinessText,
                LearnAboutText = _MJSTradersSetting.LearnAboutText,

                ActiveStoreScopeConfiguration = storeScope
            };

            //prepare available customer role
            await _baseAdminModelFactory.PrepareCustomerRolesAsync(model.AvailableCustomer, true,
                await _localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.CustomerRole.SelectAny"));

            if (storeScope <= 0)
                return View(model);

            model.Enable_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.Enable, storeScope);
            model.POCustomerId_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.POCustomerId, storeScope);
            model.PONumberPageSizeOnAccountPage_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.PONumberPageSizeOnAccountPage, storeScope);
            model.AbandonedMessageTitleContent_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.AbandonedMessageTitleContent, storeScope);
            model.CartOfferCustomerRoleId_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.CartOfferCustomerRoleId, storeScope);
            model.OfferTime_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.OfferTime, storeScope);

            model.MJSTradersLogoId_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.MJSTradersLogoId, storeScope);
            model.MJSTradersNTNNumber_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.MJSTradersNTNNumber, storeScope);
            model.MJSTradersSTRNNo_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.MJSTradersSTRNNo, storeScope);
            model.MJSEnterpriceLogoId_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.MJSEnterpriceLogoId, storeScope);
            model.MJSEnterpriceNTNNumber_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.MJSEnterpriceNTNNumber, storeScope);
            model.MJSEnterpriceSTRNNo_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.MJSEnterpriceSTRNNo, storeScope);
            model.ChooseYourBusinessText_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.ChooseYourBusinessText, storeScope);
            model.LearnAboutText_OverrideForStore = await _settingService.SettingExistsAsync(_MJSTradersSetting, x => x.LearnAboutText, storeScope);

            return View(model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public virtual async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var _MJSTradersSetting = await _settingService.LoadSettingAsync<MJSTradersSettings>(storeScope);

            //save settings
            _MJSTradersSetting.Enable = model.Enable;
            _MJSTradersSetting.POCustomerId = model.POCustomerId;
            _MJSTradersSetting.PONumberPageSizeOnAccountPage = model.PONumberPageSizeOnAccountPage;
            _MJSTradersSetting.AbandonedMessageTitleContent = model.AbandonedMessageTitleContent;
            _MJSTradersSetting.CartOfferCustomerRoleId = model.CartOfferCustomerRoleId;
            _MJSTradersSetting.OfferTime = model.OfferTime;

            _MJSTradersSetting.MJSTradersLogoId = model.MJSTradersLogoId;
            _MJSTradersSetting.MJSTradersNTNNumber = model.MJSTradersNTNNumber;
            _MJSTradersSetting.MJSTradersSTRNNo = model.MJSTradersSTRNNo;
            _MJSTradersSetting.MJSEnterpriceLogoId = model.MJSEnterpriceLogoId;
            _MJSTradersSetting.MJSEnterpriceNTNNumber = model.MJSEnterpriceNTNNumber;
            _MJSTradersSetting.MJSEnterpriceSTRNNo = model.MJSEnterpriceSTRNNo;
            _MJSTradersSetting.ChooseYourBusinessText = model.ChooseYourBusinessText;
            _MJSTradersSetting.LearnAboutText = model.LearnAboutText;

            //_settingService.SaveSetting(_shopSettings);

            /* We do not clear cache after each setting update.
           * This behavior can increase performance because cached settings will not be cleared 
           * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.Enable, model.Enable_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.POCustomerId, model.POCustomerId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.PONumberPageSizeOnAccountPage, model.PONumberPageSizeOnAccountPage_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.AbandonedMessageTitleContent, model.AbandonedMessageTitleContent_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.CartOfferCustomerRoleId, model.CartOfferCustomerRoleId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.OfferTime, model.OfferTime_OverrideForStore, storeScope, false);

            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.MJSTradersLogoId, model.MJSTradersLogoId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.MJSTradersNTNNumber, model.MJSTradersNTNNumber_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.MJSTradersSTRNNo, model.MJSTradersSTRNNo_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.MJSEnterpriceLogoId, model.MJSEnterpriceLogoId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.MJSEnterpriceNTNNumber, model.MJSEnterpriceNTNNumber_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.MJSEnterpriceSTRNNo, model.MJSEnterpriceSTRNNo_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.ChooseYourBusinessText, model.ChooseYourBusinessText_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(_MJSTradersSetting, x => x.LearnAboutText, model.LearnAboutText_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion

        #region Product detail

        [HttpPost]
        public virtual async Task<IActionResult> SaveProductDetails(ProductDetailCustomModel model)
        {
            if (!await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return AccessDeniedView();

            if (model != null)
            {
                var gstTAXProductDetailExists = await _productCustomDetailService.GetProductCustomDetailByProductIdAsync(model.ProductId);

                if (gstTAXProductDetailExists != null)
                {
                    gstTAXProductDetailExists.ProductId = model.ProductId;
                    gstTAXProductDetailExists.ProductUnit = model.ProductUnit;
                    gstTAXProductDetailExists.ProductPiecePerUnit = model.ProductPiecePerUnit;

                    //Update PO number
                    await _productCustomDetailService.UpdateProductCustomDetailAsync(gstTAXProductDetailExists);
                }
                else
                {
                    var gstTAXProductDetail = new ProductCustomDetail
                    {
                        ProductId = model.ProductId,
                        ProductUnit = model.ProductUnit,
                        ProductPiecePerUnit = model.ProductPiecePerUnit
                    };

                    //Insert PO number
                    await _productCustomDetailService.InsertProductCustomDetailAsync(gstTAXProductDetail);
                }

                return Json(new
                {
                    Result = true,
                });
            }

            return Json(new
            {
                Result = true,
            });
        }

        #endregion
    }
}
