using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{

    [MetadataType(typeof(ClaimFieldGroupTemplateMetadata))]
    public partial class ClaimFieldGroupTemplate
    {

    }



    class ClaimFieldGroupTemplateMetadata
    {
        [Required]
        public String Name { get; set; }

        
     
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }
    }
}