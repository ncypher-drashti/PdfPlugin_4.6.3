using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.EventConsumer
{
    public partial class CustomerCreateEventConsumer : IConsumer<EntityInsertedEvent<Customer>>
    {
        #region Field

        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISalesQuotationService _salesQuotationService;
        #endregion

        #region Ctor

        public CustomerCreateEventConsumer(ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ISalesQuotationService salesQuotationService)
        {
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _salesQuotationService = salesQuotationService;
        }

        #endregion

        #region Methods

        public async Task HandleEventAsync(EntityInsertedEvent<Customer> entityInsertedEvent)
        {
            var customer = entityInsertedEvent.Entity;
            if (customer == null)
                return;

            if (string.IsNullOrEmpty(customer.Email))
                return;

            var salesQuotationCustomer = new SalesQuotationCustomer
            {
                Name = await _customerService.GetCustomerFullNameAsync(customer),
                Email = customer.Email,
                Company = await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CompanyAttribute)
            };

            //Insert sales quotation customer
            await _salesQuotationService.InsertSalesQuotationCustomerAsync(salesQuotationCustomer);

            return;
        }

        #endregion
    }
}
