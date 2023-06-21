using Nop.Core;
using Nop.Cypher.Misc.MJSTraders.Model.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Models.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Factories.PONumbers
{
    public partial class PONumberModelFactory : IPONumberModelFactory
    {
        #region Fields

        private readonly IPONumberService _poNumberService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public PONumberModelFactory(IPONumberService poNumberService,
            ISettingService settingService,
            IStoreContext storeContext,
            ILocalizationService localizationService)
        {
            _poNumberService = poNumberService;
            _settingService = settingService;
            _storeContext = storeContext;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get PONumber for Customers
        /// </summary>
        /// <param name="customerId">Customer identification</param>
        /// <param name="page">Number of items page; pass null to load the first page</param>
        /// <returns>PO number list</returns>
        public virtual async Task<PONumbersModel> PreparePONumbersModelAsync(int customerId, int? page)
        {
            var _mjsTradersSettings = await _settingService.LoadSettingAsync<MJSTradersSettings>((await _storeContext.GetCurrentStoreAsync()).Id);
            var pageSize = _mjsTradersSettings.PONumberPageSizeOnAccountPage != 0 ? _mjsTradersSettings.PONumberPageSizeOnAccountPage : 10;
            var pageIndex = 0;

            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }

            //prepare model
            var model = new PONumbersModel();

            if (customerId > 0)
            {
                var poNumbers = await _poNumberService.GetPONumbersAsync(customerId: customerId,
                    pageIndex: pageIndex,
                    pageSize: pageSize);

                var pagerModel = new PagerModel(_localizationService)
                {
                    PageSize = poNumbers.PageSize,
                    TotalRecords = poNumbers.TotalCount,
                    PageIndex = poNumbers.PageIndex,
                    ShowTotalSummary = false,
                    RouteActionName = "Nop.Cypher.Misc.MJSTraders.MyAccount.MJSTraders.CustomerPONumbersPaged",
                    UseRouteLinks = true,
                    RouteValues = new PONumbersModel.CustomerPONumberRouteValues { PageNumber = pageIndex }
                };

                model.PagerModel = pagerModel;
                model.CustomerId = customerId;
                model.PONumbers = poNumbers.Select(x =>
                {
                    return new PONumberModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        CustomerId = x.CustomerId,
                        IsApprove = x.IsApproved,
                        DownloadId = x.DownloadId,
                        ApproveOnUtc = x.ApproveOnUtc,
                        CreateOnUtc = x.CreateOnUtc,
                    };
                }).ToList();
            }
            return model;
        }

        #endregion
    }
}