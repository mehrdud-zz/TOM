using ModelsLayer; 
using System.Collections.Generic;
using System.Data;
using System.Linq; 

namespace Factories
{
    public interface IClaimFieldGroupTemplateFactory
    {
        void Initialize();
        ClaimFieldGroupTemplate GetClaimFieldGroupTemplate(int claimFieldGroupTemplateId);
        List<ClaimFieldGroupTemplate> GetClaimFieldGroupTemplates();
        bool CreateClaimFieldGroupTemplate(ClaimFieldGroupTemplate claimFieldGroupTemplate);
        bool UpdateClaimFieldGroupTemplate(ClaimFieldGroupTemplate claimFieldGroupTemplate);
        bool DeleteClaimFieldGroupTemplate(ClaimFieldGroupTemplate claimFieldGroupTemplate);
        void Dispose(bool disposing);
    }

    public class ClaimFieldGroupTemplateFactory : IClaimFieldGroupTemplateFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public ClaimFieldGroupTemplate GetClaimFieldGroupTemplate(int claimFieldGroupTemplateId)
        {
            return _db.ClaimFieldGroupTemplates.Find(claimFieldGroupTemplateId);
        }

        public List<ClaimFieldGroupTemplate> GetClaimFieldGroupTemplates()
        {
            return _db.ClaimFieldGroupTemplates.ToList(); 
        }

        public bool CreateClaimFieldGroupTemplate(ClaimFieldGroupTemplate claimFieldGroupTemplate)
        {
            _db.ClaimFieldGroupTemplates.Add(claimFieldGroupTemplate);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClaimFieldGroupTemplate(ClaimFieldGroupTemplate claimFieldGroupTemplate)
        {
            _db.Entry(claimFieldGroupTemplate).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClaimFieldGroupTemplate(ClaimFieldGroupTemplate claimFieldGroupTemplate)
        {
            _db.ClaimFieldGroupTemplates.Remove(claimFieldGroupTemplate);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        } 
    }
}