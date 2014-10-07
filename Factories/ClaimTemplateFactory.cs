using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public interface IClaimTemplateFactory
    {
        void Initialize();
        ClaimTemplate GetClaimTemplate(int claimTemplateId);
        List<ClaimTemplate> GetClaimTemplates();
        bool CreateClaimTemplate(ClaimTemplate claimTemplate);
        bool UpdateClaimTemplate(ClaimTemplate claimTemplate);
        bool DeleteClaimTemplate(ClaimTemplate claimTemplate);
        void Dispose(bool disposing);
        List<ClaimTemplate> GetClientClaimTemplates(string username);
        List<ClaimTemplate> GetClientClaimTemplates(int clientId);
        List<ClaimTemplate> GetClaimTemplatesClientDoesntHave(string username);

        ClaimTemplate GetClientClaimTemplate(int clientId, int claimTemplateId);

        void DeleteClientClaimTemplate(int clientId, int claimTemplateId);
    }

    public class ClaimTemplateFactory : IClaimTemplateFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();
        private readonly IUserFactory _userFactory;

        public ClaimTemplateFactory()
        {
        }

        public ClaimTemplateFactory(IUserFactory userFactory)
        {
            _userFactory = userFactory;
        }



        public void Initialize()
        {

        }

        public ClaimTemplate GetClaimTemplate(int claimTemplateId)
        {
            return _db.ClaimTemplates.Find(claimTemplateId);
        }

        public List<ClaimTemplate> GetClaimTemplates()
        {
            return _db.ClaimTemplates.ToList();
        }

        public bool CreateClaimTemplate(ClaimTemplate claimTemplate)
        {
            _db.ClaimTemplates.Add(claimTemplate);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClaimTemplate(ClaimTemplate claimTemplate)
        {
            _db.Entry(claimTemplate).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClaimTemplate(ClaimTemplate claimTemplate)
        {
            var ctg = from c in _db.ClaimFieldGroupTemplates
                      where c.ClaimTemplateID == claimTemplate.ClaimTemplateID
                      select c;

            foreach (var ctg1 in ctg)
                _db.ClaimFieldGroupTemplates.Remove(ctg1);

            _db.SaveChanges();
            _db.ClaimTemplates.Remove(claimTemplate);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }


        public List<ClaimTemplate> GetClientClaimTemplates(string username)
        {
            User user = _userFactory.GetUser(username);
            var clientId = (user.ClientID != null) ? user.ClientID.Value : 0;


            var clientClaimTemplates =
                from ct in _db.ClaimTemplates
                where ct.Clients.Any(c => c.ClientID == clientId)
                select ct;

            return clientClaimTemplates.ToList();
        }



        public List<ClaimTemplate> GetClientClaimTemplates(int clientId)
        {
            return _db.Clients.Where(m => m.ClientID == clientId).SelectMany(m => m.ClaimTemplates).ToList();
        }

        public List<ClaimTemplate> GetClaimTemplatesClientDoesntHave(string username)
        {
            //
            //    UserFactory userfactory = new UserFactory();
            //    ModelsLayer.User user = userfactory.GetUser(Username);

            //    List<int> userClientContractsList = contractFactory.GetContractsByClientID((int)user.ClientID);

            //    var claimTemplatesList =
            //    from claimTemplates in db.ClaimTemplates
            //    where !userClientContractsList.Contains(claimTemplates.ClaimTemplateID)
            //    select claimTemplates;

            //    List<ClaimTemplate> casd =
            //        claimTemplatesList.ToList<ClaimTemplate>();

            return new List<ClaimTemplate>();
        }

        public ClaimTemplate GetClientClaimTemplate(int clientId, int claimTemplateId)
        {
            Client client = _db.Clients.SingleOrDefault(m => m.ClientID == clientId);

            var ct =
                from claimTemplate in _db.ClaimTemplates
                where claimTemplate.ClaimTemplateID == claimTemplateId && claimTemplate.Clients.Contains(client)
                select claimTemplate;

            return ct.Single();
        }

        public void DeleteClientClaimTemplate(int clientId, int claimTemplateId)
        {
            var client = _db.Clients.SingleOrDefault(m => m.ClientID == clientId);

            if (client != null)
            {
                var claimTemplate =
                    client.ClaimTemplates.Single(m => m.ClaimTemplateID == claimTemplateId);

                if (claimTemplate != null)
                    client.ClaimTemplates.Remove(claimTemplate);
            }

            _db.SaveChanges();
        }
    }
}