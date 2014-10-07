//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Acturis
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClaimCore
    {
        public int ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public Nullable<int> ClaimRef { get; set; }
        public Nullable<int> ClaimStatus { get; set; }
        public Nullable<int> ClaimCause { get; set; }
        public Nullable<int> ClaimHandlerOffice { get; set; }
        public Nullable<int> ClaimHandler { get; set; }
        public Nullable<System.DateTime> LossDateFrom { get; set; }
        public Nullable<System.DateTime> NotificationDate { get; set; }
        public Nullable<int> DelegatedArrangement { get; set; }
        public Nullable<System.DateTime> SettlementDate { get; set; }
        public Nullable<int> NatureOfInjury { get; set; }
        public Nullable<int> LossInvolving { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public Nullable<int> Country { get; set; }
        public string Note { get; set; }
        public Nullable<int> ClaimType { get; set; }
        public Nullable<int> PolicyVersionRef { get; set; }
        public string Description { get; set; }
        public string ClientClaimRef { get; set; }
        public Nullable<int> DataLoadId { get; set; }
        public bool Loaded { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
        public Nullable<int> NCBCompromised { get; set; }
        public string PolicyExcess { get; set; }
        public Nullable<int> PolicySection { get; set; }
        public Nullable<int> ClientAtFault { get; set; }
        public string OldClaimNo { get; set; }
        public Nullable<System.DateTime> LossDateTo { get; set; }
        public Nullable<System.DateTime> ADWLastUpdate { get; set; }
    }
}
