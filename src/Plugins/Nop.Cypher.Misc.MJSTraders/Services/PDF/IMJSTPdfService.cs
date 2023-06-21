using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HarfBuzzSharp;
using Nop.Core.Domain.Vendors;
using Nop.Cypher.Misc.MJSTraders.Domain;

namespace Nop.Cypher.Misc.MJSTraders.Services.PDF
{
    public partial interface IMJSTPdfService 
    {
        /// <summary>
        /// Print an sales quotation to PDF
        /// </summary>
        /// <param name="salesQuotation">Sales quotation</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        /// <param name="vendorId">Vendor identifier to limit products; 0 to print all products. If specified, then totals won't be printed</param>
        /// <returns>A path of generated file</returns>
        Task PrintSalesQuotationToPdfAsync(Stream stream, SalesQuotation salesQuotation, Language language = null, Vendor vendor = null);

        /// <summary>
        /// Print sales quotations to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="salesQuotations">Sales quotations</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        /// <param name="vendorId">Vendor identifier to limit products; 0 to print all products. If specified, then totals won't be printed</param>
        Task PrintSalesQuotationsToPdfAsync(Stream stream, IList<SalesQuotation> salesQuotations, Language language = null, Vendor vendor = null);

        ///// <summary>
        ///// print cash memo pdf
        ///// </summary>
        ///// <param name="stream">Stream</param>
        ///// <param name="shipment">shipment for print cash memo</param>
        ///// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        //Task PrintCashMemoPDFAsync(Stream stream, Shipment shipment, int languageId = 0);

    }
}
