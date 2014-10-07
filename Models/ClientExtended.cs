using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ClientMetadata))]
    public partial class Client
    {


        

    }

    class ClientMetadata
    {
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name="Field Group Background Colour")]
        public string Colour1 { get; set; }

    }
}