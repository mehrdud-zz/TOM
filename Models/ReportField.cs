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
    
    public partial class ReportField
    {
        public int ReportFieldID { get; set; }
        public Nullable<int> ReportID { get; set; }
        public Nullable<int> FieldId { get; set; }
        public string DisplayName { get; set; }
        public string Func { get; set; }
        public Nullable<bool> Filter { get; set; }
        public string Direction { get; set; }
        public Nullable<int> FieldNumber { get; set; }
        public string TableName { get; set; }
    
        public virtual ReportTemplate ReportTemplate { get; set; }
        public virtual ClaimFieldTemplate ClaimFieldTemplate { get; set; }
    }
}