using Nop.Core.Configuration;

namespace Nop.Cypher.Misc.MJSTraders
{
    public class MJSTradersSettings : ISettings
    {
        /// <summary>
        /// Gets or Sets Plugin in Enable 
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Gets or Sets po customer identification
        /// </summary>
        public int POCustomerId { get; set; }

        /// <summary>
        /// Gets or Sets the page size for PO number to account page
        /// </summary>
        public int PONumberPageSizeOnAccountPage { get; set; }

        /// <summary>
        /// Gets or Sets cart offer customer role
        /// </summary>
        public int CartOfferCustomerRoleId { get; set; }

        /// <summary>
        /// Gets or Sets abandoned message title content
        /// </summary>
        public string AbandonedMessageTitleContent { get; set; }

        /// <summary>
        /// Gets or Sets offer time
        /// </summary>
        public int OfferTime { get; set; }

        /// <summary>
        /// Gets or Sets MJS Traders logo
        /// </summary>
        public int MJSTradersLogoId { get; set; }

        /// <summary>
        /// Gets or Sets MJS Traders NTN number
        /// </summary>
        public string MJSTradersNTNNumber { get; set; }

        /// <summary>
        /// Gets or sets MJS Traders STRN no
        /// </summary>
        public string MJSTradersSTRNNo { get; set; }

        /// <summary>
        /// Gets or Sets MJS Enterprice logo
        /// </summary>
        public int MJSEnterpriceLogoId { get; set; }

        /// <summary>
        /// Gets or Sets MJS Enterprice NTN number
        /// </summary>
        public string MJSEnterpriceNTNNumber { get; set; }

        /// <summary>
        /// Gets or sets MJS Enterprice STRN no
        /// </summary>
        public string MJSEnterpriceSTRNNo { get; set; }
        
        public string LearnAboutText { get; set; }

        public string ChooseYourBusinessText { get; set; }

    }
}
