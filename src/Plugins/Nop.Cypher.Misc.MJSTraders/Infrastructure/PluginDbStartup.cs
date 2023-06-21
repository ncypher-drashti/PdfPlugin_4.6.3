using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Validators;

namespace Nop.Cypher.Misc.MJSTraders.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring plugin DB context on application startup
    /// </summary>
    public class PluginDbStartup : INopStartup
    {
        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 111;

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {

        }

        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            /// <summary>
            /// Represents object for the configuring plugin DB context on application startup
            /// </summary>
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new MJSTradersViewLocationExpander());
            });

            //public validator
            services.AddTransient<IValidator<Model.PONumbers.PONumbersModel>, Validators.PODocumentValidator>();

            // admin validator
            services.AddTransient<IValidator<Areas.Admin.Model.SalesQuotations.SalesQuotationModel>, SalesQuotationValidator>();
        }
    }
}