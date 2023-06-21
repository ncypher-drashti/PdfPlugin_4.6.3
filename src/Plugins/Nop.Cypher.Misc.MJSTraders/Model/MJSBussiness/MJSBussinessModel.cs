using Nop.Web.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Cypher.Misc.MJSTraders.Model.MJSBussiness
{
    public partial record MJSBussinessModel : BaseNopEntityModel
    {
        public string ChooseYourBusinessText { get; set; }    
        
        public string LearnAboutText { get; set; }        
    }
}
