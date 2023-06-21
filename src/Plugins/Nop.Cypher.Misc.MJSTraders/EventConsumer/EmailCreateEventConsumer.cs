using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Events;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.EventConsumer
{
    public class EmailCreateEventConsumer : IConsumer<EntityInsertedEvent<QueuedEmail>>
    {
        #region Field

        private readonly ICustomerService _customerService;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public EmailCreateEventConsumer(ICustomerService customerService,
            IStoreContext storeContext,
            IGenericAttributeService genericAttributeService,
            ISettingService settingService)
        {
            _customerService = customerService;
            _storeContext = storeContext;
            _storeContext = storeContext;
            _genericAttributeService = genericAttributeService;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        public async Task HandleEventAsync(EntityInsertedEvent<QueuedEmail> entityInsertedEvent)
        {
            var queuedEmail = entityInsertedEvent.Entity;
            var _mjsTradersSettings = await _settingService.LoadSettingAsync<MJSTradersSettings>((await _storeContext.GetCurrentStoreAsync()).Id);

            if (!string.IsNullOrWhiteSpace(_mjsTradersSettings.AbandonedMessageTitleContent) && _mjsTradersSettings.CartOfferCustomerRoleId > 0)
            {
                if (queuedEmail.Subject.Contains(_mjsTradersSettings.AbandonedMessageTitleContent))
                {
                    var customer = await _customerService.GetCustomerByEmailAsync(queuedEmail.To);

                    if (!(await _customerService.GetCustomerRolesAsync(customer)).Any(x => x.Id == _mjsTradersSettings.CartOfferCustomerRoleId))
                    {
                        await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping { CustomerId = customer.Id, CustomerRoleId = (await _customerService.GetCustomerRoleByIdAsync(_mjsTradersSettings.CartOfferCustomerRoleId)).Id });
                        await _customerService.UpdateCustomerAsync(customer);
                    }
                    await _genericAttributeService.SaveAttributeAsync(customer, MJSTradersUtilities.CartOfferRoleAssignedTime, DateTime.UtcNow.ToString("G"), (await _storeContext.GetCurrentStoreAsync()).Id);
                }
            }

            return;
        }

        #endregion
    }
}