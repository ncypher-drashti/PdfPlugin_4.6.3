using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.ScheduleTasks;
using Nop.Core.Infrastructure;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components;
using Nop.Cypher.Misc.MJSTraders.Components;
using Nop.Data;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;
using Nop.Services.Security;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nop.Cypher.Misc.MJSTraders
{
    public class MJSTradersPlugins : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IRepository<Language> _languagerepository;
        private readonly INopFileProvider _fileProvider;
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly WidgetSettings _widgetSettings;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public MJSTradersPlugins(ILocalizationService LocalizationService,
            IPermissionService PermissionService,
            IRepository<Language> languagerepository,
            INopFileProvider fileProvider,
            IWebHelper WebHelper,
            ISettingService settingService,
            IMessageTemplateService messageTemplateService,
            WidgetSettings widgetSettings,
            IScheduleTaskService scheduleTaskService)
        {
            _localizationService = LocalizationService;
            _permissionService = PermissionService;
            _languagerepository = languagerepository;
            _fileProvider = fileProvider;
            _webHelper = WebHelper;
            _settingService = settingService;
            _messageTemplateService = messageTemplateService;
            _widgetSettings = widgetSettings;
            _scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Utilities

        public async Task<bool> AuthenticateAsync()
        {
            return await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins);
        }

        /// <summary>
        /// Install resource string
        /// </summary>
        protected virtual async Task InstallLocaleResourcesAsync()
        {
            //'English' language
            var enLanguage = _languagerepository.Table.FirstOrDefault(l => l.LanguageCulture.ToLower() == "en-us" & l.Published);

            //save resources
            if (enLanguage != null)
            {
                foreach (var filePath in Directory.EnumerateFiles(_fileProvider.MapPath("~/Plugins/Cypher.MJSTraders/Localization/ResourceString"),
                "ResourceString_EN.xml", SearchOption.TopDirectoryOnly))
                {
                    //var localesXml = File.ReadAllText(filePath);
                    //localizationService.ImportResourcesFromXml(enLanguage, localesXml);
                    using (var streamReader = new StreamReader(filePath))
                    {
                        await _localizationService.ImportResourcesFromXmlAsync(enLanguage, streamReader);
                    }
                }
            }
        }

        ///<summry>
        ///Delete Resource String
        ///</summry>
        protected virtual async Task DeleteLocalResourcesAsync()
        {
            var file = Path.Combine(_fileProvider.MapPath("~/Plugins/Cypher.MJSTraders/Localization/ResourceString"), "ResourceString_EN.xml");
            var languageResourceNames = from name in XDocument.Load(file).Document.Descendants("LocaleResource")
                                        select name.Attribute("Name").Value;

            foreach (var item in languageResourceNames)
            {
                await _localizationService.DeleteLocaleResourceAsync(item);
            }
        }

        #endregion

        #region Methods

        #region In/Uninstall

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/MJSTraders/Configure";
        }

        public override async Task InstallAsync()
        {
            // mark widget as active
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(this.PluginDescriptor.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(this.PluginDescriptor.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            // Insert or Install settings
            var settings = new MJSTradersSettings
            {
                Enable = true,
                PONumberPageSizeOnAccountPage = 10,
            };
            await _settingService.SaveSettingAsync(settings);

            #region Create Email/message template

            if (!(await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.UploadPONumberAdminNotification).Any())
            {
                await _messageTemplateService.InsertMessageTemplateAsync(new MessageTemplate
                {
                    Name = MJSTradersEmailTemplates.UploadPONumberAdminNotification,
                    Subject = "PO Received",
                    Body = "<p><img src=\"https://www.mjstraders.com/images/thumbs/0000439.png\" alt=\"%Store.Name%\" style=\"height:65px;\"/>" +
                      "<br/>" +
                      "Dear Store Owner" +
                      "<br/> " +
                      "A Purchase order %PONumber.Id% has been uploaded by %PONumber.CustomerName% from %PONumber.CompanyName%." +
                      "<br/> " +
                      "You’re Requested to review the purchase order and approve it through your admin panel." +
                      "<br/></p>",
                    IsActive = true,
                    DelayBeforeSend = 0,
                    DelayPeriodId = 0,
                    AttachedDownloadId = 0,
                    EmailAccountId = 1,
                    LimitedToStores = true
                });
            }

            if (!(await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.UploadPONumberCustomerNotification).Any())
            {
                await _messageTemplateService.InsertMessageTemplateAsync(new MessageTemplate
                {
                    Name = MJSTradersEmailTemplates.UploadPONumberCustomerNotification,
                    Subject = "Upload PO Number - %MJST.Title%",
                    Body = "<p>An upload po number %MJST.Id% - %MJST.Title%" +
                    "<br/> " +
                    "Create: %MJST.CreateOnUtc%" +
                    "<br/></p>",
                    IsActive = false,
                    DelayBeforeSend = 0,
                    DelayPeriodId = 0,
                    AttachedDownloadId = 0,
                    EmailAccountId = 1,
                    LimitedToStores = true,
                });
            }

            if (!(await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.ApprovePONumberCustomerNotification).Any())
            {
                await _messageTemplateService.InsertMessageTemplateAsync(new MessageTemplate
                {
                    Name = MJSTradersEmailTemplates.ApprovePONumberCustomerNotification,
                    Subject = "PO Approved",
                    Body = "<p><img src=\"https://www.mjstraders.com/images/thumbs/0000439.png\" alt=\"%Store.Name%\" style=\"height:65px;\"/>" +
                     "<br/> " +
                     "Dear %PONumber.CustomerName%" +
                     "<br/> " +
                     "We are humbled to have been chosen by %PONumber.CompanyName% as a vendor" +
                     "<br/> " +
                     "This email is to acknowledge that we have reviewed your purchase order along with its terms and conditions and thankfully accept." +
                     "<br/> " +
                     "The purchase order is now in its processing mode and will be delivered well within the time frame allotted." +
                     "<br/> " +
                     "Our team will keep you notified related to this order, or feel free to call us anytime if you have any queries." +
                     "<br/> " +
                     "Many thanks" +
                     "<br/> " +
                     "MJS Team" +
                     "<br/></p>",
                    IsActive = true,
                    DelayBeforeSend = 0,
                    DelayPeriodId = 0,
                    AttachedDownloadId = 0,
                    EmailAccountId = 1,
                    LimitedToStores = true,
                });
            }

            if (!(await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.DeletePONumberCustomerNotification).Any())
            {
                await _messageTemplateService.InsertMessageTemplateAsync(new MessageTemplate
                {
                    Name = MJSTradersEmailTemplates.DeletePONumberCustomerNotification,
                    Subject = "Deleted your PO Number - %MJST.Title%",
                    Body = "<p>A delete your #%MJST.Id% : %MJST.Title%" +
                     "<br/> " +
                     "Delete: %MJST.DeleteOnUtc%" +
                     "<br/></p>",
                    IsActive = false,
                    DelayBeforeSend = 0,
                    DelayPeriodId = 0,
                    AttachedDownloadId = 0,
                    EmailAccountId = 1,
                    LimitedToStores = true,
                });
            }

            if (!(await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.SendSalesQuotationCustomerNotification).Any())
            {
                await _messageTemplateService.InsertMessageTemplateAsync(new MessageTemplate
                {
                    Name = MJSTradersEmailTemplates.SendSalesQuotationCustomerNotification,
                    Subject = "SALES QUOTATION - %MJST.Title%",
                    Body = "<p><img src=\"https://www.mjstraders.com/content/images/thumbs/0000439.png\" alt=\"%Store.Name%\" style=\"height:65px;\"/>" +
                    "<br/>" +
                     "Dear %SalesQuotation.CustomerName%" +
                    "<br/>" +
                    "Thanks for your recent Inquiry at MJS Traders." +
                    "<br/>" +
                    "We have attached the quotation in this email for your convenience." +
                     "<br/>" +
                    "Please note that MJS Traders provides a price beat guarantee towards all products. So do not stress if the attached quotation is not per your expectations." +
                     "<br/>" +
                    "All you need to do is communicate with us the lowest price you have been quoted and we will beat that." +
                    "<br/>" +
                    "Thanks again for trusting MJS Traders and stay safe." +
                    "<br/>" +
                    "Best Regards" +
                    "MJS Team" +
                    "<br/></p>",
                    IsActive = true,
                    DelayBeforeSend = 0,
                    DelayPeriodId = 0,
                    AttachedDownloadId = 0,
                    EmailAccountId = 1,
                    LimitedToStores = true,
                });
            }

            #endregion

            #region Schedual Task

            //install synchronization task
            if (await _scheduleTaskService.GetTaskByTypeAsync("Nop.Cypher.Misc.MJSTraders.SchedulTask.RemoveCartOfferCustomerRoleTask, Nop.Cypher.Misc.MJSTraders") == null)
            {
                await _scheduleTaskService.InsertTaskAsync(new ScheduleTask
                {
                    Enabled = true,
                    Seconds = 3600,
                    Name = "MJSTraders - Remove Cart Offer Customer Role",
                    Type = "Nop.Cypher.Misc.MJSTraders.SchedulTask.RemoveCartOfferCustomerRoleTask, Nop.Cypher.Misc.MJSTraders"
                });
            }

            #endregion

            //Insert resource string
            await InstallLocaleResourcesAsync();

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            // remove widget as active
            if (_widgetSettings.ActiveWidgetSystemNames.Contains(this.PluginDescriptor.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(this.PluginDescriptor.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            // Delete Or Uninstall Settings
            await _settingService.DeleteSettingAsync<MJSTradersSettings>();

            #region  Email/message template

            //Delete message/Email template
            var messageTemplate = (await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.UploadPONumberAdminNotification).FirstOrDefault();
            if (messageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplate);

            messageTemplate = (await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.UploadPONumberCustomerNotification).FirstOrDefault();
            if (messageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplate);

            messageTemplate = (await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.ApprovePONumberCustomerNotification).FirstOrDefault();
            if (messageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplate);

            messageTemplate = (await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.DeletePONumberCustomerNotification).FirstOrDefault();
            if (messageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplate);

            messageTemplate = (await _messageTemplateService.GetAllMessageTemplatesAsync(0)).Where(x => x.Name == MJSTradersEmailTemplates.SendSalesQuotationCustomerNotification).FirstOrDefault();
            if (messageTemplate != null)
                await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplate);

            #endregion

            #region Schedual Task

            //uninstall synchronization task
            var removeCartOfferCustomerRole = await _scheduleTaskService.GetTaskByTypeAsync("Nop.Cypher.Misc.MJSTraders.SchedulTask.RemoveCartOfferCustomerRoleTask, Nop.Cypher.Misc.MJSTraders");
            if (removeCartOfferCustomerRole != null)
                await _scheduleTaskService.DeleteTaskAsync(removeCartOfferCustomerRole);

            #endregion

            //Delete resource string
            await DeleteLocalResourcesAsync();

            await base.UninstallAsync();
        }

        #endregion

        #region Widget

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                // admin area widgets
                AdminWidgetZones.OrderDetailsBlock,

                // admin after payment status
                "admin_orderdetail_after_paymentstatus",

                // admin order shipment edit page buttons
                AdminWidgetZones.OrderShipmentDetailsButtons,

                // admin product edit page additional product detail
                AdminWidgetZones.ProductDetailsBlock,

                // admin customer edit page 
                AdminWidgetZones.CustomerDetailsButtons,

                // public area widgets
                PublicWidgetZones.AccountNavigationAfter,
                PublicWidgetZones.OrderDetailsPageBeforeproducts,

                // cart page for select po order mapping
                PublicWidgetZones.OrderSummaryContentDeals,

                // order list page - before order status line
                "orderlist_page_before_orderstatus_line",

                // Home page MJSBussiness
                PublicWidgetZones.HomepageBeforeProducts
            });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            // admin area widgets
            if (widgetZone == AdminWidgetZones.OrderDetailsBlock)
                return typeof(OrderEditViewComponent);

            // admin after payment status
            if (widgetZone == "admin_orderdetail_after_paymentstatus")
            return typeof(OrderCheckNumberViewComponent);

            // admin order shipment edit page buttons
            if (widgetZone == AdminWidgetZones.OrderShipmentDetailsButtons)
                return typeof(ShipmentButtonsViewComponent);

            // admin product edit page additional product detail
            if (widgetZone == AdminWidgetZones.ProductDetailsBlock)
                return typeof(ProductEditDetailsBlockViewComponent);

            if (widgetZone == AdminWidgetZones.CustomerDetailsButtons)
                return typeof(CustomerEditViewComponent);

            // public area widgets
            if (widgetZone == PublicWidgetZones.OrderDetailsPageBeforeproducts)
                return typeof(PODocumentOrderDetailViewComponent);

            // public area widgets
            if (widgetZone == PublicWidgetZones.OrderSummaryContentDeals)
                return typeof(PurchaseOrderMappingViewComponent);

            // Home page MJSBussiness
            if (widgetZone == PublicWidgetZones.HomepageBeforeProducts)
                return typeof(MJSBussinessViewComponent);

            if (widgetZone == "orderlist_page_before_orderstatus_line")
                return typeof(OrderListOrderPurchaseOrderNumberViewComponent);

            else
                return typeof(MyAccountMJSTradersViewComponent);

        }

        #endregion

        #region Site map - plugin menu

        /// <summary>
        /// Manage plugin menu in sidebar menu at admin side
        /// </summary>
        /// <param name="rootNode"></param>
        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            // sales menu
            var salesNode = rootNode.ChildNodes.Where(x => x.SystemName == "Sales").FirstOrDefault();
            var poNumberNode = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Nop.Cypher.Sitemap.MJSTraders.PONumber"),
                ControllerName = "PONumber",
                ActionName = "List",
                Visible = await AuthenticateAsync(),
                SystemName = "Cypher.MJSTraders.PONumber",
                IconClass = "far fa-dot-circle"
            };
            salesNode.ChildNodes.Add(poNumberNode);

            var salesQuotation = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Nop.Cypher.Sitemap.MJSTraders.SalesQuotation"),
                ControllerName = "SalesQuotation",
                ActionName = "CustomerList",
                Visible = await AuthenticateAsync(),
                SystemName = "Cypher.MJSTraders.SalesQuotation",
                IconClass = "far fa-dot-circle"
            };
            salesNode.ChildNodes.Add(salesQuotation);

            // configuration
            var configurationNode = rootNode.ChildNodes.Where(x => x.SystemName == "Configuration").FirstOrDefault();

            var configureNode = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Nop.Cypher.Sitemap.MJSTraders.Configuration"),
                ControllerName = "MJSTraders",
                ActionName = "Configure",
                Visible = await AuthenticateAsync(),
                SystemName = "Cypher.MJSTraders.Configure",
                IconClass = "far fa-dot-circle"
            };
            configurationNode.ChildNodes.Add(configureNode);
        }

        #endregion

        #endregion

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;
    }
}
