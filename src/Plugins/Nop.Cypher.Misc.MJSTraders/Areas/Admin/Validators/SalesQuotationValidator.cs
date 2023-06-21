using FluentValidation;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model.SalesQuotations;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Validators
{
    /// <summary>
    /// Represents Po number validator
    /// </summary>
    public class SalesQuotationValidator : BaseNopValidator<SalesQuotationModel>
    {
        #region Ctor

        public SalesQuotationValidator(ILocalizationService localizationService)
        {
            RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Validator.Name.Required"));

            RuleFor(model => model.CreatedBy)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Validator.CreatedBy.Required"));

            RuleFor(model => model.QuotationTitle)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Validator.Title.Required"));


            RuleFor(model => model.Email)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Validator.Email.Required"));


            RuleFor(model => model.GenerateDate)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Validator.GenerateDate.Required"));

            RuleFor(model => model.InquiryDate)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.Admin.SalesQuotation.Validator.InquiryDate.Required"));

        }

        #endregion
    }
}
