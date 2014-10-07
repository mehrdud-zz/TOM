using BrokingPlatformIntegrationBase.Interfaces;
using Eclipse.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Eclipse.Factories
{
    public interface IEclipseClaimFactory
    {
        List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetClaims();
        BrokingPlatformIntegrationBase.Interfaces.IClaim GetClaim(int ClaimID);

        string GetClaimStatus(int ClaimID);

        List<ClientElement> GetClientList();
    }

    public class EclipseClaimFactory : BrokingPlatformIntegrationBase.Controllers.IClaimController, IEclipseClaimFactory
    {
        private Acturis.EclipseODSEntities db = new Acturis.EclipseODSEntities();

        public EclipseClaimFactory()
        {
        }
        public List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetClaims()
        {

            var eclipseClaims = db.tblClaims.Take(1000);

            var eclipseClaimsList = new List<BrokingPlatformIntegrationBase.Interfaces.IClaim>();
            foreach (var eclipseClaim in eclipseClaims)
            {
                eclipseClaimsList.Add(
                    new EclipseClaim { ClaimID = eclipseClaim.ClaimId, Reference = eclipseClaim.ClaimReference });

            }
             
            return eclipseClaimsList.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaim>();
        }


        public BrokingPlatformIntegrationBase.Interfaces.IClaim GetClaim(int ClaimID)
        {
            var eclipseClaimItem = new EclipseClaim();
            eclipseClaimItem.ClaimFieldGroups = new List<IClaimFieldGroup>();
            var eclipseClaimFieldGroup = new EclipseClaimFieldGroup("", "");

            eclipseClaimFieldGroup.ClaimFields = new List<IClaimField>();

            var eclipseClaim =
                db.tblClaims.Single(m => m.ClaimId == ClaimID);

            var claimId = new Acturis.Data.ActurisClaimField();
            claimId.Name = "ClaimId";
            claimId.ShortTextValue = eclipseClaim.ClaimId.ToString();
            claimId.TemplateName = "ShortText";
            eclipseClaimFieldGroup.ClaimFields.Add(claimId);


            var claimName = new Acturis.Data.ActurisClaimField();
            claimName.Name = "Name";
            claimName.ShortTextValue = eclipseClaim.ClaimId.ToString();
            claimName.TemplateName = "ShortText";
            eclipseClaimFieldGroup.ClaimFields.Add(claimName);


            var fields = new string[] {"ClaimStatus","ClaimReference","LossRegisterId","SumInsured","SumInsuredCurrencyISO",
                "LossName","LossDateFrom","LossDateTo","LossLocation","ClaimDescription","VesselAircraftConvey","Claimant","ContentiousLossIndicator",
                "UniqueClaimReference","Interest","InsuredId","InsuredName","ClientId","ClientName","ReinsuredId","ReinsuredName","PrimaryClaimHandlerId",
                "PrimaryClaimHandlerName","PrimaryClaimHandlerTeamId","PrimaryClaimHandlerTeamName","Deleted","CreatedDate","LastUpdateDate"};

            foreach (var field in fields)
            {
                var claimRef = new Acturis.Data.ActurisClaimField();
                claimRef.Name = field;
                var object1 = eclipseClaim.GetType().GetProperty(field).GetValue(eclipseClaim, null);
                claimRef.ShortTextValue = (object1 != null) ? object1.ToString() : String.Empty;
                claimRef.TemplateName = "ShortText";
                eclipseClaimFieldGroup.ClaimFields.Add(claimRef);
            }


            eclipseClaimItem.ClaimID = ClaimID;

            eclipseClaimItem.ClaimFieldGroups.Add(eclipseClaimFieldGroup);
            return eclipseClaimItem;
        }


        public string GetClaimStatus(int ClaimID)
        {
            return String.Empty;
        }




        public List<ClientElement> GetClientList()
        {
            var clientElementList =
                new List<ClientElement>();



            string query =
            @"SELECT TOP 20 [ClientName], [ClientId] COUNT([ClaimId]) AS [Counter] FROM [tblClaim] GROUP BY [ClientName] ORDER BY [Counter] DESC ";

            var clientList = db.tblClaims.SqlQuery(query).ToList();


            foreach (var client in clientList)
            {
                clientElementList.Add(
                    new ClientElement()
                    {
                        Name = client.ClientName,
                        ClientID = client.ClientId.Value,
                        Source = "Eclipse"
                    });
            }

            return clientElementList;
        }
    }


}
