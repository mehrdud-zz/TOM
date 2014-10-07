using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokingPlatformIntegrationBase.Interfaces
{
    public class ClientElement
    {
        public int ClientID;
        public String Name;
        public String Source;
    }

    public interface IClaim
    {
        String Name { get; set; }
        DateTime ClaimDate { get; set; }
        String ClientName { get; set; }
        List<IClaimFieldGroup> ClaimFieldGroups { get; set; }

        String Status { get; set; }

        String Source { get; }

        int ClaimID { get; }

        Nullable<int> ClaimTemplateID { get; }
        Nullable<DateTime> DateCreated { get; }
        String Description { get; }
        Nullable<int> CountryID { get; }
        String CountryName { get; }


        String ClaimTempalteName { get; }
    }

    public interface IClaimFieldGroup
    {
        String Name { get; set; }
        String Description { get; set; }
        List<IClaimField> ClaimFields { get; set; }
    }

    public interface IClaimField
    {
        String Name { get; set; }
        String Description { get; set; }
        String Value { get; set; }

        String ClaimFieldTemplate { set; get; }

        bool Mandatory { get; }

        String TemplateName { set; get; }



         int ClaimFieldID { get; set; }
         Nullable<int> ClaimFieldGroupID { get; set; }
         Nullable<int> ClaimFieldTemplateID { get; set; }
         string ShortTextValue { get; set; }
         string LongTextValue { get; set; }
         Nullable<System.DateTime> DateTimeValue { get; set; }
         Nullable<decimal> CurrecncyValue { get; set; }
         String CurrecncySign { get; set; }
         byte[] FileValue { get; set; }
       
         string Code { get; set; }
         string CountryValue { get; set; }
         string CountryValueName { get; set; }

         string DropDownValue { get; set; }
         string MultiChoiceValue { get; set; }
         Nullable<System.DateTime> DateValue { get; set; }
         Nullable<int> IntegerValue { get; set; }
         Nullable<double> FloatValue { get; set; }
         Nullable<double> RangeValue { get; set; }
         string DropDownLevel2Value { get; set; }
    

    }

 
}


namespace BrokingPlatformIntegrationBase.Controllers
{
    public interface IClaimController
    {
        List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetClaims();
    }
}
