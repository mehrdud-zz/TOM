using ModelsLayer; 
using System.Collections.Generic;
using System.Data;
using System.Linq; 

namespace Factories
{
    public interface IClaimFieldTemplateFactory
    {
        void Initialize();
        ClaimFieldTemplate GetClaimFieldTemplate(int claimFieldTemplateId);
        List<ClaimFieldTemplate> GetClaimFieldTemplates();
        bool CreateClaimFieldTemplate(ClaimFieldTemplate claimFieldTemplate);
        bool UpdateClaimFieldTemplate(ClaimFieldTemplate claimFieldTemplate);
        bool DeleteClaimFieldTemplate(ClaimFieldTemplate claimFieldTemplate);
        void Dispose(bool disposing);

        List<ClaimFieldTemplate> GetClaimFieldTemplates(int claimTemplateId);
    }

    public class ClaimFieldTemplateFactory : IClaimFieldTemplateFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public ClaimFieldTemplate GetClaimFieldTemplate(int claimFieldTemplateId)
        {
            return _db.ClaimFieldTemplates.Find(claimFieldTemplateId); 
        }

        public List<ClaimFieldTemplate> GetClaimFieldTemplates()
        { 
            return _db.ClaimFieldTemplates.ToList(); 
        }

        public bool CreateClaimFieldTemplate(ClaimFieldTemplate claimFieldTemplate)
        {
            _db.ClaimFieldTemplates.Add(claimFieldTemplate);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClaimFieldTemplate(ClaimFieldTemplate claimFieldTemplate)
        {
            _db.Entry(claimFieldTemplate).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClaimFieldTemplate(ClaimFieldTemplate claimFieldTemplate)
        {
            _db.ClaimFieldTemplates.Remove(claimFieldTemplate);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }



        public List<ClaimFieldTemplate> GetClaimFieldTemplates(int claimTemplateId)
        {
            return _db.ClaimFieldTemplates.Where(m => m.ClaimFieldGroupTemplate.ClaimTemplateID == claimTemplateId).ToList(); 
        }
    }
}