using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Nop.Cypher.Misc.MJSTraders.Infrastructure
{
    public partial class RouteProvider : BaseRouteProvider, IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //get language pattern
            //it's not needed to use language pattern in AJAX requests and for actions returning the result directly (e.g. file to download),
            //use it only for URLs of pages that the user can go to
            var lang = GetLanguageRoutePattern();

            //My account - MJSTraders PO Number
            endpointRouteBuilder.MapControllerRoute("Nop.Cypher.Misc.MJSTraders.MyAccount.MJSTraders.CustomerPONumbers", $"{lang}/customer/ponumbers",
            new { controller = "PONumber", action = "CustomerPONumbers" });

            endpointRouteBuilder.MapControllerRoute("Nop.Cypher.Misc.MJSTraders.MyAccount.MJSTraders.CustomerPONumbersPaged", $"{lang}/customer/ponumbers/page/{{pageNumber:min(0)}}",
            new { controller = "PONumber", action = "CustomerPONumbers" });

            endpointRouteBuilder.MapControllerRoute("Nop.Cypher.Misc.MJSTraders.OrderDetail.Download.PODocumentDownload", $"{lang}/Download/PODocumentDownload/{{downloadId:min(0)}}",
            new { controller = "Download", action = "PODocumentDownload" });

            endpointRouteBuilder.MapControllerRoute("Nop.Cypher.Misc.MJSTraders.MyAccount.MJSTraders.CreatePONumber", $"{lang}/PONumber/CreatePONumber",
            new { controller = "PONumber", action = "CreatePONumber" });

            endpointRouteBuilder.MapControllerRoute("Nop.Cypher.Misc.MJSTraders.SelectPurchaseOrder", $"{lang}/SelectPurchaseOrder/{{purchaseOrderId:min(0)}}",
            new { controller = "PONumber", action = "SelectPurchaseOrder" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => -1;
    }
}