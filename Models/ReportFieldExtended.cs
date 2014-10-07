using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ModelsLayer.ReportFieldMetadata))]
    public partial class ReportField
    {
        public string ClaimTemplateName { get; set; }

    }

    class ReportFieldMetadata
    {
        [Display(Name = "Function")]
        [UIHint("FunctionEditor")]
        public String Func { get; set; }

        [Display(Name = "Field")]
        [Required]
        [UIHint("FieldEditor")]
        public Nullable<int> FieldId { get; set; }



        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }



    }

}