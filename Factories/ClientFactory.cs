using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public interface IClientFactory
    {
        void Initialize();
        Client GetClient(string clientName);
        Client GetClient(int clientId);
        List<Client> GetClients();
        bool CreateClient(Client client);
        bool UpdateClient(Client client);
        bool DeleteClient(Client client);
        void Dispose(bool disposing);
        bool AddClaimTemplateToClient(int clientId, int claimTemplateId);
    }

    public class ClientFactory : IClientFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {
        }

        public Client GetClient(string clientName)
        {
            return _db.Clients.Single(cl => cl.Name == clientName);
        }

        public Client GetClient(int clientId)
        {
            return _db.Clients.Find(clientId);
        }

        public List<Client> GetClients()
        {
            var clients = _db.Clients;
            List<Client> clientList = clients.ToList();
            return clientList;
        }

        public bool CreateClient(Client client)
        {
            _db.Clients.Add(client);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClient(Client client)
        {
            _db.Entry(client).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClient(Client client)
        {
            _db.Clients.Remove(client);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }


        public List<ClaimTemplate> GetTemplate(int clientId)
        {
            return new List<ClaimTemplate>(); 
        }


        //public List<ClaimTemplate> GetTemplate(int ClientID)
        //{
        //    var clientTemplates =
        //        from clientClaimTemplates in db.ClientClaimTemplates
        //        join clients in db.Clients on clientClaimTemplates.ClientID equals clients.ClientID
        //        where clients.ClientID == ClientID
        //        select new { ClaimTemplateID = clientClaimTemplates.ClaimTemplateID };

        //    List<int> clientTemplateIDList = new List<int>();

        //    foreach (var clientTemplate in clientTemplates)
        //    {
        //        clientTemplateIDList.Add(clientTemplate.ClaimTemplateID);
        //    }

        //    Claims.Controllers.ClaimTemplateController claimController = new Claims.Controllers.ClaimTemplateController();

        //    List<ClaimTemplate> result = new List<ClaimTemplate>();

        //    foreach (int claimTemplateID in clientTemplateIDList)
        //    {
        //        result.Add(claimController.GetClaimTemplate(claimTemplateID));
        //    }
        //    return result;
        //}

        public bool AddClaimTemplateToClient(int clientId, int claimTemplateId)
        {
            _db.Clients.Find(clientId).ClaimTemplates.Add(
                _db.ClaimTemplates.Find(claimTemplateId)
                );
            _db.SaveChanges();
            return true;
        }
    }

}