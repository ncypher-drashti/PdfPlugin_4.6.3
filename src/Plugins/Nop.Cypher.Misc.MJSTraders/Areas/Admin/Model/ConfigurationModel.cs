using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model
{
    public record ConfigurationModel : BaseNopModel
    {
        public ConfigurationModel()
        {
            AvailableCustomer = new List<SelectListItem>();
        }

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.Enable")]
        public bool Enable { get; set; }
        public bool Enable_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.POCustomer")]
        public int POCustomerId { get; set; }
        public bool POCustomerId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.PONumberPageSizeOnAccountPage")]
        public int PONumberPageSizeOnAccountPage { get; set; }
        public bool PONumberPageSizeOnAccountPage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.CartOfferCustomerRole")]
        public int CartOfferCustomerRoleId { get; set; }
        public bool CartOfferCustomerRoleId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.AbandonedMessageTitleContent")]
        public string AbandonedMessageTitleContent { get; set; }
        public bool AbandonedMessageTitleContent_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.OfferTime")]
        public int OfferTime { get; set; }
        public bool OfferTime_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.MJSTradersLogoId")]
        [UIHint("Picture")]
        public int MJSTradersLogoId { get; set; }
        public bool MJSTradersLogoId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.MJSTradersNTNNumber")]
        public string MJSTradersNTNNumber { get; set; }
        public bool MJSTradersNTNNumber_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.MJSTradersSTRNNo")]
        public string MJSTradersSTRNNo { get; set; }
        public bool MJSTradersSTRNNo_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.MJSEnterpriceLogoId")]
        [UIHint("Picture")]
        public int MJSEnterpriceLogoId { get; set; }
        public bool MJSEnterpriceLogoId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.MJSEnterpriceNTNNumber")]
        public string MJSEnterpriceNTNNumber { get; set; }
        public bool MJSEnterpriceNTNNumber_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.MJSEnterpriceSTRNNo")]
        public string MJSEnterpriceSTRNNo { get; set; }
        public bool MJSEnterpriceSTRNNo_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.LearnAboutText")]
        public string LearnAboutText { get; set; }
        public bool LearnAboutText_OverrideForStore { get; set; }

        [NopResourceDisplayName("Nop.Cypher.Misc.MJSTraders.Admin.Configuration.ChooseYourBusinessText")]
        public string ChooseYourBusinessText { get; set; }
        public bool ChooseYourBusinessText_OverrideForStore { get; set; }

        public IList<SelectListItem> AvailableCustomer { get; set; }
    }
}
