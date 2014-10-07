//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ModelsLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        public Client()
        {
            this.ClaimTemplates = new HashSet<ClaimTemplate>();
            this.PageSetups = new HashSet<PageSetup>();
        }
    
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundColour { get; set; }
        public string TextColour { get; set; }
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }
        public string Heading3 { get; set; }
        public string Heading4 { get; set; }
        public string Colour1 { get; set; }
        public string Colour2 { get; set; }
        public string Colour3 { get; set; }
        public string Colour4 { get; set; }
        public string Colour5 { get; set; }
        public string Colour6 { get; set; }
        public string Colour7 { get; set; }
        public string Colour8 { get; set; }
        public string Colour9 { get; set; }
        public string Colour10 { get; set; }
    
        public virtual ICollection<ClaimTemplate> ClaimTemplates { get; set; }
        public virtual ICollection<PageSetup> PageSetups { get; set; }
    }
}
