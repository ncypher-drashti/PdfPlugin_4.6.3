using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using System;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.EventConsumer
{
    public class OrderPlaceEventConsumer : IConsumer<OrderPlacedEvent>
    {
        #region Field

        private readonly ICustomerService _customerService;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPONumberService _poNumberService;
        private readonly IPODocumentService _poDocumentService;

        #endregion

        #region Ctor

        public OrderPlaceEventConsumer(ICustomerService customerService,
            IStoreContext storeContext,
            IGenericAttributeService genericAttributeService,
            IPONumberService poNumberService,
            IPODocumentService poDocumentService)
        {
            _customerService = customerService;
            _storeContext = storeContext;
            _storeContext = storeContext;
            _genericAttributeService = genericAttributeService;
            _poNumberService = poNumberService;
            _poDocumentService = poDocumentService;
        }

        #endregion

        #region Methods

        public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;

            var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);

            // get last selected purchase order
            int lastPurchaseOrder = await _genericAttributeService.GetAttributeAsync<int>(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, (await _storeContext.GetCurrentStoreAsync()).Id);
            if (lastPurchaseOrder == 0)
                return;

            var po = await _poNumberService.GetPONumberByIdAsync(lastPurchaseOrder);
            if (po == null)
                return;

            var poDocument = await _poDocumentService.GetPODocumentByOrderIdAsync(order.Id);
            if (poDocument == null)
            {
                poDocument = new PODocument()
                {
                    Title1 = po.Title,
                    DownloadId1 = po.DownloadId,
                    IsActive = true,
                    OrderId = order.Id,
                    CreateOnUtc = DateTime.UtcNow,
                    UpdateOnUtc = DateTime.UtcNow
                };

                await _poDocumentService.InsertPODocumentAsync(poDocument);

                // delete po
                await _poNumberService.DeletePONumberAsync(po);

                // set 0 to last selected po
                await _genericAttributeService.SaveAttributeAsync(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, 0, (await _storeContext.GetCurrentStoreAsync()).Id);
            }
            else
            {
                if (poDocument.DownloadId1 == 0)
                {
                    poDocument.Title1 = po.Title;
                    poDocument.DownloadId1 = po.DownloadId;
                }
                else if (poDocument.DownloadId2 == 0)
                {
                    poDocument.Title2 = po.Title;
                    poDocument.DownloadId2 = po.DownloadId;
                }
                else if (poDocument.DownloadId3 == 0)
                {
                    poDocument.Title3 = po.Title;
                    poDocument.DownloadId3 = po.DownloadId;
                }
                else if (poDocument.DownloadId4 == 0)
                {
                    poDocument.Title4 = po.Title;
                    poDocument.DownloadId4 = po.DownloadId;
                }
                else if (poDocument.DownloadId5 == 0)
                {
                    poDocument.Title5 = po.Title;
                    poDocument.DownloadId5 = po.DownloadId;
                }

                poDocument.IsActive = true;
                poDocument.OrderId = order.Id;
                poDocument.UpdateOnUtc = DateTime.UtcNow;

                await _poDocumentService.UpdatePODocumentAsync(poDocument);

                // delete po
                await _poNumberService.DeletePONumberAsync(po);

                // set 0 to last selected po
                await _genericAttributeService.SaveAttributeAsync(customer, MJSTradersUtilities.LastSelectedPurchaseOrderId, 0, (await _storeContext.GetCurrentStoreAsync()).Id);
            }

            return;
        }

        #endregion
    }
}