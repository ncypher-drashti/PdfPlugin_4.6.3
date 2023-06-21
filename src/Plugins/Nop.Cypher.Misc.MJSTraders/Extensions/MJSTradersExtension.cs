using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Extensions
{
    public static class MJSTradersExtension
    {
        public static async Task<bool> IsPOCustomerAsync(this Customer customer)
        {
            var _settingService = EngineContext.Current.Resolve<ISettingService>();
            var _customerService = EngineContext.Current.Resolve<ICustomerService>();
            int poCustomerRoleId = await _settingService.GetSettingByKeyAsync<int>("MJSTradersSettings.POCustomerId");
            if (poCustomerRoleId == 0)
                return false;

            if ((await _customerService.GetCustomerRolesAsync(customer)).Any(x => x.Id == poCustomerRoleId))
                return true;

            return false;
        }
    }
}
