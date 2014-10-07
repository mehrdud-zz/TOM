using System;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer
{
    [MetadataType(typeof(ClaimTemplateMetadata))]
    public partial class ClaimTemplate
    {
        public override string ToString()
        {
            return Name;
        }
    }


    class ClaimTemplateMetadata
    {
        [Required]
        public string Name { get; set; }

        [UIHint("SourceEditor")]
        public string Source { get; set; }

    }
}