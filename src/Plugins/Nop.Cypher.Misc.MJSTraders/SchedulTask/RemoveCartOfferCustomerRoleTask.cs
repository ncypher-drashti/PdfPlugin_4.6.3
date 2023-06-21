using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Data;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.ScheduleTasks;
using System;
using System.Linq;

namespace Nop.Cypher.Misc.MJSTraders.SchedulTask
{
    public class RemoveCartOfferCustomerRoleTask : IScheduleTask
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IRepository<GenericAttribute> _genericAttributeRepository;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public RemoveCartOfferCustomerRoleTask(ISettingService settingService,
            IStoreContext storeContext,
            IGenericAttributeService genericAttributeService,
            IRepository<GenericAttribute> genericAttributeRepository,
            ICustomerService customerService)
        {
            _settingService = settingService;
            _storeContext = storeContext;
            _genericAttributeService = genericAttributeService;
            _genericAttributeRepository = genericAttributeRepository;
            _customerService = customerService;
        }

        #endregion

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            //Get setting
            var _mjsTradersSettings = await _settingService.LoadSettingAsync<MJSTradersSettings>((await _storeContext.GetCurrentStoreAsync()).Id);

            if (_mjsTradersSettings.CartOfferCustomerRoleId == 0)
                return;

            var genericAttributes = _genericAttributeRepository.Table.Where(g => g.KeyGroup == "Customer" && g.Key == MJSTradersUtilities.CartOfferRoleAssignedTime && g.StoreId == _storeContext.GetCurrentStore().Id).ToList();
            foreach (var genericAttribute in genericAttributes)
            {
                var customerdate = Convert.ToDateTime(genericAttribute.Value).AddHours(_mjsTradersSettings.OfferTime);
                if (customerdate <= DateTime.UtcNow)
                {
                    var customer = await _customerService.GetCustomerByIdAsync(genericAttribute.EntityId);

                    if ((await _customerService.GetCustomerRolesAsync(customer)).Any(x => x.Id == _mjsTradersSettings.CartOfferCustomerRoleId))
                    {
                        await _customerService.RemoveCustomerRoleMappingAsync(customer, (await _customerService.GetCustomerRolesAsync(customer)).FirstOrDefault(mapping => mapping.Id == _mjsTradersSettings.CartOfferCustomerRoleId));
                        await _customerService.UpdateCustomerAsync(customer);
                        await _genericAttributeService.DeleteAttributeAsync(genericAttribute);
                    }
                }
            }
        }
    }
}