using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ContractMetadata))]
    public partial class Contract
    {

    }
     
        class ContractMetadata
        {
            [Display(Name = "Start Date")]
            [Required]
            public Nullable<System.DateTime> StartDate { get; set; }

            [Display(Name = "End Date")]
            public Nullable<System.DateTime> EndDate { get; set; }
        }
 

}