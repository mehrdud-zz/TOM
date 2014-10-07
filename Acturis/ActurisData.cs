using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Acturis.Data
{
    public class ActurisClaim : BrokingPlatformIntegrationBase.Interfaces.IClaim
    {
        private ClaimCore _ClaimCore;
        private List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup> _ActurisClaimFieldGroupList;

        public ActurisClaim(ClaimCore ClaimCore)
        {
            if (ClaimCore != null)
            {
                this._ClaimCore = ClaimCore;

                if (ClaimCore.ClaimRef != null)
                    this.Name = ClaimCore.ClaimRef.Value.ToString();

                if (ClaimCore.LossDateFrom != null)
                    this.ClaimDate = ClaimCore.LossDateFrom.Value;


                this.ClientName = ClaimCore.ClaimId.ToString();


                if (ClaimCore.ClaimStatus != null)
                {
                    this._Status = ClaimCore.ClaimStatus.Value.ToString();
                }
            }
        }

        private string _Name;
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }

        }

        private DateTime _ClaimDate;
        public DateTime ClaimDate
        {
            get
            {

                return _ClaimDate;
            }
            set
            {
                _ClaimDate = value;

            }
        }


        private string _ClientName;
        public string ClientName
        {
            get
            {
                return _ClientName;
            }
            set
            {
                this._ClientName = value;
            }
        }

        public List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup> ClaimFieldGroups
        {
            get
            {
                return
                    this._ActurisClaimFieldGroupList.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup>();
            }
            set
            {
                this._ActurisClaimFieldGroupList = value;
            }
        }




        public string Source
        {
            get { return "Acturis"; }
        }

        public int ClaimID
        {
            get
            {
                return _ClaimCore.ClaimId;
            }
        }


        private string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }


        public int? ClaimTemplateID
        {
            get { return null; }
        }

        public Nullable<DateTime> DateCreated
        {
            get { return DateTime.Now; }
        }

        public string Description
        {
            get { return String.Empty; }
        }

        public int? CountryID
        {
            get { return null; }
        }

        public string CountryName
        {
            get { return String.Empty; }
        }


        public String ClaimTempalteName
        {
            get
            {
                return "Acturis";
            }
        }

        public String ClaimCause
        {
            get;
            set;
        }
    }
    public partial class ActurisClaimFieldGroup : BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup
    {
        public ActurisClaimFieldGroup(String Name, String Description)
        {
            this._Name = Name;
            this._Description = Description;
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        private string _Name;

        public string Description
        {
            get { return this._Description; }
            set
            {
                this._Description = value;
            }
        }
        private string _Description;

        public List<BrokingPlatformIntegrationBase.Interfaces.IClaimField> ClaimFields
        {
            get
            {
                return _ClaimFields;
            }
            set
            {
                this._ClaimFields = value;
            }
        }
        private List<BrokingPlatformIntegrationBase.Interfaces.IClaimField> _ClaimFields;


        public String Source { get { return "Acturis"; } }
    }

    public class ActurisClaimField : BrokingPlatformIntegrationBase.Interfaces.IClaimField
    {
        public ActurisClaimField()
        {

        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this._Name = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { this._Description = value; }
        }

        private String _Value;
        public String Value
        {
            get { return _Value; }
            set { this._Value = value; }
        }


        private bool _Mandatory;
        public bool Mandatory
        {
            get { return _Mandatory; }
            set { this._Mandatory = value; }
        }



        private String _TemplateName;
        public String TemplateName
        {
            get
            {
                return _TemplateName;
            }
            set
            {
                this._TemplateName = value;
            }
        }




        public string ClaimFieldTemplate
        {
            get;
            set;
        }






        public int ClaimFieldID
        {
            get;
            set;
        }

        public int? ClaimFieldGroupID
        {
            get;
            set;
        }

        public int? ClaimFieldTemplateID
        {
            get;
            set;
        }

        public string ShortTextValue
        {
            get;
            set;
        }

        public string LongTextValue
        {
            get;
            set;
        }

        public DateTime? DateTimeValue
        {
            get;
            set;
        }

        public decimal? CurrecncyValue
        {
            get;
            set;
        }

        public byte[] FileValue
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string CountryValue
        {
            get;
            set;
        }

        public string CountryValueName
        {
            get;
            set;
        }

        public string DropDownValue
        {
            get;
            set;
        }

        public string MultiChoiceValue
        {
            get;
            set;
        }

        public DateTime? DateValue
        {
            get;
            set;
        }

        public int? IntegerValue
        {
            get;
            set;
        }

        public double? FloatValue
        {
            get;
            set;
        }

        public double? RangeValue
        {
            get;
            set;
        }

        public string DropDownLevel2Value
        {
            get;
            set;
        }



        public string CurrecncySign
        {
            get;
            set;
        }
    }
}


