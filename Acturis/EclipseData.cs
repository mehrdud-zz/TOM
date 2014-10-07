using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Eclipse.Data
{
    public class EclipseClaim : BrokingPlatformIntegrationBase.Interfaces.IClaim
    {
        private Acturis.ClaimCore _ClaimCore;
        private List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup> _EclipseClaimFieldGroupList;

        public EclipseClaim()
        {
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
                    this._EclipseClaimFieldGroupList;
            }
            set
            {
                this._EclipseClaimFieldGroupList = value;
            }
        }




        public string Source
        {
            get { return "Eclipse"; }
        }

        public int ClaimID
        {
            get;
            set;
        }

        public string Reference { get; set; }


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
                return "Eclipse";
            }
        }

        public String ClaimCause
        {
            get;
            set;
        }
    }
    public class EclipsePolicy : BrokingPlatformIntegrationBase.Interfaces.IClaim
    {
        private Acturis.ClaimCore _ClaimCore;
        private List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup> _EclipseClaimFieldGroupList;

        public EclipsePolicy()
        {
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
                    this._EclipseClaimFieldGroupList;
            }
            set
            {
                this._EclipseClaimFieldGroupList = value;
            }
        }




        public string Source
        {
            get { return "Eclipse Policy"; }
        }

        public int ClaimID
        {
            get;
            set;
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
                return "Eclipse";
            }
        }

        public String ClaimCause
        {
            get;
            set;
        }
    }
    public partial class EclipseClaimFieldGroup : BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup
    {
        public EclipseClaimFieldGroup(String Name, String Description)
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


        public String Source { get { return "Eclipse"; } }
    }

    public class EclipseClaimField : BrokingPlatformIntegrationBase.Interfaces.IClaimField
    {
        public EclipseClaimField()
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


