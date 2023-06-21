using FluentValidation;
using Nop.Cypher.Misc.MJSTraders.Model.PONumbers;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Cypher.Misc.MJSTraders.Validators
{
    /// <summary>
    /// Represents Po number validator
    /// </summary>
    public class PODocumentValidator : BaseNopValidator<PONumbersModel>
    {
        #region Ctor

        public PODocumentValidator(ILocalizationService localizationService)
        {
            //RuleFor(model => model.Title)
            // .NotEmpty()
            // .WithMessage(localizationService.GetResource("Nop.Cypher.Misc.MJSTraders.PONumber.Validator.Title"));

            RuleFor(model => model.DownloadId)
            .NotEqual(0)
            .WithMessageAwait(localizationService.GetResourceAsync("Nop.Cypher.Misc.MJSTraders.PONumber.Validator.Download"));
        }

        #endregion
    }
}