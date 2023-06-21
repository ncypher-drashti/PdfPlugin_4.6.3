using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Controllers;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Factories.SalesQuotations;
using Nop.Cypher.Misc.MJSTraders.Controllers;
using Nop.Cypher.Misc.MJSTraders.Services.Catalog;
using Nop.Cypher.Misc.MJSTraders.Services.Messages;
using Nop.Cypher.Misc.MJSTraders.Services.Orders;
using Nop.Cypher.Misc.MJSTraders.Services.PDF;
using Nop.Cypher.Misc.MJSTraders.Services.PODocuments;
using Nop.Cypher.Misc.MJSTraders.Services.PONumbers;
using Nop.Cypher.Misc.MJSTraders.Services.ProductCustomDetails;
using Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations;
using Nop.Services.Common;
using Nop.Services.Orders;
using Nop.Web.Controllers;
using Nop.Web.Factories;

namespace Nop.Cypher.Misc.MJSTraders.Infrastructure
{
    public class NopStartup : INopStartup
    {

        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register service

            //services
            services.AddScoped<IPONumberService, PONumberService>();
            services.AddScoped<IMJSTradersWorkFlowMessageService, MJSTradersWorkFlowMessageService>();
            services.AddScoped<IPODocumentService, PODocumentService>();
            services.AddScoped<ISalesQuotationService, SalesQuotationService>();
            services.AddScoped<ICustomOrderService, CustomOrderService>();
            services.AddScoped<IProductCustomDetailService, ProductCustomDetailService>();
            services.AddScoped<IOrderProcessingService, CustomOrderProcessingService>();

            //admin factories
            services.AddScoped<IPONumberModelFactory, PONumberModelFactory>();
            services.AddScoped<IPODocumentModelFactory, PODocumentModelFactory>();
            services.AddScoped<ISalesQuotationModelFactory, SalesQuotationModelFactory>();

            //factories
            services.AddScoped<Factories.PONumbers.IPONumberModelFactory, Factories.PONumbers.PONumberModelFactory>();
            services.AddScoped<Factories.PODocuments.IPODocumentModelFactory, Factories.PODocuments.PODocumentModelFactory>();

            //Override 
            services.AddScoped<IMJSTPdfService, MJSTPdfService>();
            services.AddScoped<IPdfService, MJSTPdfService>();
            services.AddScoped<IProductModelFactory, Factories.Products.MJSTradersProductModelFactory>();
            services.AddScoped<ShoppingCartController, MJSTradersShoppingCartController>();
            services.AddScoped<IShoppingCartService, MJSTradersShoppingCartService>();
            services.AddScoped<ShoppingCartService, MJSTPriceCalculationService>();

            //Controllers
            services.AddScoped<Nop.Web.Areas.Admin.Controllers.OrderController, CustomOrderController>();
            services.AddScoped<CatalogController, MJSTradersCatalogController>();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 2001;
    }
}
