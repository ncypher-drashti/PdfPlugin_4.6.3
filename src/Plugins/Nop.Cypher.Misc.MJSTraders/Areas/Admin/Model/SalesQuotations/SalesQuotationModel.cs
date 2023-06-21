using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations
{
    public record SalesQuotationModel : BaseNopEntityModel
    {
        public SalesQuotationModel()
        {
            SalesQuotationLineSearchModel = new SalesQuotationLineSearchModel();
            salesQuotationNewLineModel = new SalesQuotationLineModel();
            SalesQuotationDocumentModel = new SalesQuotationDocumentModel();
            SalesQuotationDocumentSearchModel = new SalesQuotationDocumentSearchModel();
            AvailableEntities = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.CreatedBy")]
        public string CreatedBy { get; set; }

        public int SalesQuotationCustomerId { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.ReferenceNumber")]
        public string ReferenceNumber { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Title")]
        public string QuotationTitle { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.EmailCC")]
        public string EmailCC { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Company")]
        public string Company { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Designation")]
        public string Designation { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.InquiryDate")]
        [UIHint("Date")]
        public DateTime InquiryDate { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.GenerateDate")]
        [UIHint("Date")]
        public DateTime GenerateDate { get; set; }

        public SalesQuotationLineSearchModel SalesQuotationLineSearchModel { get; set; }

        public SalesQuotationLineModel salesQuotationNewLineModel { get; set; }

        public SalesQuotationDocumentModel SalesQuotationDocumentModel { get; set; }

        public SalesQuotationDocumentSearchModel SalesQuotationDocumentSearchModel { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.IsTaxInclusive")]
        public bool IsTaxInclusive { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.DeliveryTerms")]
        public string DeliveryTerms { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.DeliveryCharges")]
        public decimal DeliveryCharges { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.PaymentTerms")]
        public string PaymentTerms { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.ValidUntil")]
        [UIHint("Date")]
        public DateTime? ValidUntil { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.SalesQuotationNote")]
        public string SalesQuotationNote { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.SendFromEntity")]
        public string SendFromEntity { get; set; }

        public IList<SelectListItem> AvailableEntities { get; set; }
    }
}
