using BrokingPlatformIntegrationBase.Interfaces;
using Eclipse.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Eclipse.Factories
{

    public interface IEclipsePolicyFactory
    {
        List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetPolicies();
        BrokingPlatformIntegrationBase.Interfaces.IClaim GetPolicy(int polictyId);


        List<ClientElement> GetClientList();
    }


    public class EclipsePolicyFactory : IEclipsePolicyFactory
    {
        private Acturis.EclipseODSEntities db = new Acturis.EclipseODSEntities();

        public EclipsePolicyFactory()
        {
        }
        public List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetPolicies()
        {
            List<Eclipse.Data.EclipseClaim> eclipseClaimsList = new List<Eclipse.Data.EclipseClaim>();



            return eclipseClaimsList.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaim>();
        }


        public BrokingPlatformIntegrationBase.Interfaces.IClaim GetPolicy(int polictyId)
        {
            var eclipsePolicy = db.tblPolicies.Single(m => m.PolicyId == polictyId);

            var eclipsePolicyItem = new Data.EclipsePolicy();
            eclipsePolicyItem.ClaimFieldGroups = new List<IClaimFieldGroup>();
            var eclipseClaimFieldGroup = new EclipseClaimFieldGroup("", "");
          
            eclipseClaimFieldGroup.ClaimFields = new List<IClaimField>();
          

            var policyRef = new Acturis.Data.ActurisClaimField();
            policyRef.Name = "Policy Reference";
            policyRef.ShortTextValue = eclipsePolicy.PolicyReference;
            policyRef.TemplateName = "ShortText";
            eclipseClaimFieldGroup.ClaimFields.Add(policyRef);


            var fields = new string[] {
                "PolicyId","PolicyReference","ProgrammeReference","ProductClassId","ProductClass","Product","OwnershipTeamId","OwnerId","BusinessType","PolicyStatus","PolicyStatusCode",
                "AuthorizedDate","CancelledDate","InceptionDate","ExpiryDate","Backloaded","BackloadDescription","RenewedFromId","RenewedToId","PolicyExpired","PolicyType","MasterBinder",
                "Deleted","PrimaryHandlerId","PrimaryHandlerName","PrimaryHandlerTeamId","PrimaryHandlerTeam","PrimaryProducerId","PrimaryProducerName","PrimaryProducerTeamId","PrimaryProducerTeam",
                "FirstClientId","FirstClientName","FirstClientCountry","FirstInsuredId","FirstInsuredName","FirstInsuredCountry","FirstReinsuredId","FirstReinsuredName","FirstReinsuredCountry",
                "FirstBusinessClassId","FirstBusinessClass","BusinessClassList","CreatedDate","LastUpdateDate"};

            foreach (var field in fields)
            {
                var claimRef = new Acturis.Data.ActurisClaimField();
                claimRef.Name = field;
                var object1 = eclipsePolicy.GetType().GetProperty(field).GetValue(eclipsePolicy, null);
                claimRef.ShortTextValue = (object1 != null) ? object1.ToString() : String.Empty;
                claimRef.TemplateName = "ShortText";
                eclipseClaimFieldGroup.ClaimFields.Add(claimRef);
            }          

            eclipsePolicyItem.ClaimID = eclipsePolicy.PolicyId;
            eclipsePolicyItem.ClaimFieldGroups.Add(eclipseClaimFieldGroup);
            return eclipsePolicyItem;
        }



        public List<ClientElement> GetClientList()
        {
            List<ClientElement> clientElementList =
                new List<ClientElement>();



            return clientElementList;
        }
    }



}
