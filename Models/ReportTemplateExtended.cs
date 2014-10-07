using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ModelsLayer.ReportTemplateMetadata))]
    public partial class ReportTemplate  
    {
        
 
    }

    class ReportTemplateMetadata
    {
        [Display(Name = "Order Direction")]
        [UIHint("OrderDirectionEditor")]
        public String OrderDirection { get; set; }
         
    }

}