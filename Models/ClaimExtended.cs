using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ModelsLayer.ClaimMetadata))]
    public partial class Claim : BrokingPlatformIntegrationBase.Interfaces.IClaim
    {


        public override string ToString()
        {
            return this.Name;
        }



        public DateTime ClaimDate
        {
            get
            {
                if (this.DateCreated != null)
                    return this.DateCreated.Value;
                else
                    return DateTime.Now;
            }
            set
            {

            }
        }

        public string ClientName
        {
            get
            {
                return this.Name;
            }
            set
            {
            }
        }

        List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup> BrokingPlatformIntegrationBase.Interfaces.IClaim.ClaimFieldGroups
        {
            get
            {
                return this.ClaimFieldGroups.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup>();
            }
            set
            {

            }
        }



        public String Source { get { return "CFT"; } }

        public String Status
        {
            get
            {
                if (this.ClaimStatu != null)
                    return this.ClaimStatu.Name;
                else
                    return "-";
            }
            set
            {

            }
        }



        public string CountryName
        {
            get
            {
                if (this.Country != null)
                    return this.Country.Name;
                else
                    return String.Empty;
            }
        }

        public String ClaimTempalteName { get {
            if (this.ClaimTemplate != null)
                return this.ClaimTemplate.Name;
            else
                return String.Empty;
        } }
    }

    class ClaimMetadata
    {
        [Display(Name = "Claim Template ID")]
        [Required]
        public Nullable<int> ClaimTemplateID { get; set; }

        [Display(Name = "Client User ID")]
        [Required]
        public Nullable<int> UserID { get; set; }

        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public Nullable<System.DateTime> DateCreated { get; set; }


        [Display(Name = "Willis Employee ID")]
        public Nullable<int> WillisEmployeeID { get; set; }
    }

}