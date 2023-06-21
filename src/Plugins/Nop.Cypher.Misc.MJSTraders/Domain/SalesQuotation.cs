using Nop.Core;
using System;

namespace Nop.Cypher.Misc.MJSTraders.Domain
{
    public class SalesQuotation : BaseEntity
    {
        /// <summary>
        /// Gets or Sets Created by
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets reference number
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets sales quotation customer identification
        /// </summary>
        public int SalesQuotationCustomerId { get; set; }

        /// <summary>
        /// Gets or Sets title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets email
        /// </summary>
        public string EmailCC { get; set; }

        /// <summary>
        /// Gets or Sets comapny
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Gets or Sets inquiry date
        /// </summary>
        public DateTime InquiryDate { get; set; }

        /// <summary>
        /// Gets or Sets generate date
        /// </summary>
        public DateTime GenerateDate { get; set; }

        /// <summary>
        /// Gets or sets price is inclusive tax or not
        /// </summary>
        public bool IsTaxInclusive { get; set; }

        /// <summary>
        /// Gets or Sets delivery terms
        /// </summary>
        public string DeliveryTerms { get; set; }

        /// <summary>
        /// Gets or Sets delivery charges
        /// </summary>
        public decimal DeliveryCharges { get; set; }

        /// <summary>
        /// Gets or Sets payment terms
        /// </summary>
        public string PaymentTerms { get; set; }

        /// <summary>
        /// Gets or Sets valid until
        /// </summary>
        public DateTime? ValidUntilUtc { get; set; }

        /// <summary>
        /// Gets or set sales quotation additional note
        /// </summary>
        public string SalesQuotationNote { get; set; }

        /// <summary>
        /// Gets or Sets send from entity
        /// </summary>
        public string SendFromEntity { get; set; }
    }
}
