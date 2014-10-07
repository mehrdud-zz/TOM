using BrokingPlatformIntegrationBase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acturis.Factories
{
    public interface IActurisClaimFactory
    {
        List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetClaims();
        BrokingPlatformIntegrationBase.Interfaces.IClaim GetClaim(int ClaimID);

        string GetClaimStatus(int ClaimID);

        List<ClientElement> GetClientList();
    }

    public class ActurisClaimFactory : BrokingPlatformIntegrationBase.Controllers.IClaimController, IActurisClaimFactory
    {
        private ACWEntities db = new ACWEntities();

        public ActurisClaimFactory()
        {
        }
        public List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetClaims()
        {
            List<Acturis.Data.ActurisClaim> acturisClaimsList = new List<Acturis.Data.ActurisClaim>();

            var claimCores = db.ClaimCores.Take(200).OrderByDescending(m => m.ADWLastUpdate);

            foreach (ClaimCore claimCore in claimCores)
            {
                Acturis.Data.ActurisClaim acturisClaim =
                    new Data.ActurisClaim(claimCore);
                 
                acturisClaimsList.Add(acturisClaim);
            }

            return acturisClaimsList.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaim>();
        }


        public BrokingPlatformIntegrationBase.Interfaces.IClaim GetClaim(int ClaimID)
        {
            List<Acturis.Data.ActurisClaim> acturisClaimsList = new List<Acturis.Data.ActurisClaim>();

            var claimCore = db.ClaimCores.Single(m => m.ClaimId == ClaimID);


            Acturis.Data.ActurisClaim acturisClaim =
                new Data.ActurisClaim(claimCore);

            acturisClaim.ClaimFieldGroups =
                new List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup>();

            Acturis.Data.ActurisClaimFieldGroup acturisClaimFieldGroup =
                new Acturis.Data.ActurisClaimFieldGroup(claimCore.Description, String.Empty);

            List<Acturis.Data.ActurisClaimField> acturisClaimFieldList =
                new List<Data.ActurisClaimField>();


            Acturis.Data.ActurisClaimField clientClaimRef = new Data.ActurisClaimField();
            clientClaimRef.Name = "Client Claim Reference";
            clientClaimRef.ShortTextValue = claimCore.PolicyExcess;
            clientClaimRef.TemplateName = "ShortText";
            acturisClaimFieldList.Add(clientClaimRef);


            Acturis.Data.ActurisClaimField natureofInjury = new Data.ActurisClaimField();
            natureofInjury.Name = "Nature of Injury";
            if (claimCore.NatureOfInjury != null)
            {
                natureofInjury.ShortTextValue =
                    db.NatureOfInjuries.Single(m => m.NatureOfInjuryId == claimCore.NatureOfInjury.Value).Description;
            }
            else
                natureofInjury.ShortTextValue = "-";
            natureofInjury.TemplateName = "ShortText";
            acturisClaimFieldList.Add(natureofInjury);


            Acturis.Data.ActurisClaimField lossInvolving = new Data.ActurisClaimField();
            lossInvolving.Name = "Loss Involving";
            if (claimCore.LossInvolving != null)
            {
                lossInvolving.ShortTextValue =
                    db.LossInvolvings.Single(m => m.LossInvolvingId == claimCore.LossInvolving.Value).Description;
            }
            else
                lossInvolving.ShortTextValue = "-";
            lossInvolving.TemplateName = "ShortText";
            acturisClaimFieldList.Add(lossInvolving);


            Acturis.Data.ActurisClaimField claimCause = new Data.ActurisClaimField();
            claimCause.Name = "Claim Cause";
            if (claimCore.ClaimCause != null)
            {
                claimCause.ShortTextValue =
                    db.ClaimCauseTypes.Single(m => m.ClaimCauseRef == claimCore.ClaimCause.Value).ClaimCause;
            }
            else
                claimCause.ShortTextValue = "-";
            claimCause.TemplateName = "ShortText";
            acturisClaimFieldList.Add(claimCause);




            Acturis.Data.ActurisClaimField policyExcess = new Data.ActurisClaimField();
            policyExcess.Name = "Policy Excess";
            policyExcess.ShortTextValue = claimCore.PolicyExcess;
            policyExcess.TemplateName = "ShortText";
            acturisClaimFieldList.Add(policyExcess);


            Acturis.Data.ActurisClaimField lossToDate = new Data.ActurisClaimField();
            lossToDate.Name = "Loss Date to";
            if (claimCore.LossDateTo.HasValue)
                lossToDate.ShortTextValue = claimCore.LossDateTo.Value.ToShortDateString();
            else
                lossToDate.ShortTextValue = "-";

            lossToDate.TemplateName = "ShortText";
            lossToDate.Description = "This field shows date from which we had a loss!";
            acturisClaimFieldList.Add(lossToDate);



            acturisClaimFieldGroup.ClaimFields = acturisClaimFieldList.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaimField>();

            List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup> temp = new List<BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup>();
            temp.Add((BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup)acturisClaimFieldGroup);

            acturisClaim.ClaimFieldGroups = temp;

            acturisClaim.Status = GetClaimStatus(ClaimID);

            return acturisClaim;
        }


        public string GetClaimStatus(int ClaimID)
        {
            var claimStatusName =
                from claimCore in db.ClaimCores
                join claimStatus in db.ClaimStatus on claimCore.ClaimStatus equals claimStatus.ClaimStatusId
                where claimCore.ClaimId == ClaimID
                select new { status = claimStatus.Description };

            return claimStatusName.First().status;
        }


        public List<ClientElement> GetClientList()
        {
            List<ClientElement> clientElementList =
                new List<ClientElement>();



            string query =
            @"SELECT 
            Client.* from Client 
            inner join (
                SELECT TOP 30
                    Client.ClientId,
                    COUNT(PolicyVersionRef)  AS PolicyCount  FROM ClaimCore     
                    INNER JOIN Policy on VersionRef =  PolicyVersionRef
                    INNER JOIN Client on Policy.ClientId = Client.ClientId  GROUP BY Client.ClientId,Client.Name  ORDER BY PolicyCount Desc
                ) AS t1
             on Client.ClientId = t1.ClientId  ";

            var clientList = db.Clients.SqlQuery(query).ToList();


            foreach (var client in clientList)
            {
                clientElementList.Add(
                    new ClientElement()
                {
                    Name = client.Name,
                    ClientID = client.ClientId,
                    Source = "Acturis"
                });
            }

            return clientElementList;
        }
    }



}
