using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public interface IClaimFieldFactory
    {
        void Initialize();
        ClaimField GetClaimField(int claimFieldId);
        List<ClaimField> GetClaimFields();
        List<ClaimField> GetClaimFields(int claimId);
        bool CreateClaimField(ClaimField claimField);
        bool UpdateClaimField(ClaimField claimField);
        bool DeleteClaimField(ClaimField claimField);
        void Dispose(bool disposing);
    }

    public class ClaimFieldFactory : IClaimFieldFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public ClaimField GetClaimField(int claimFieldId)
        {
            return _db.ClaimFields.Include("ClaimFieldFieldGroups").Single(m => m.ClaimFieldID == claimFieldId);
        }

        public List<ClaimField> GetClaimFields()
        {
            return _db.ClaimFields.ToList();
        }

        public bool CreateClaimField(ClaimField claimField)
        {
            _db.ClaimFields.Add(claimField);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClaimField(ClaimField claimField)
        {
            _db.Entry(claimField).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClaimField(ClaimField claimField)
        {
            _db.ClaimFields.Remove(claimField);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public List<ClaimField> GetClaimFields(int claimId)
        {
            return _db.ClaimFields.Where(m => m.ClaimFieldGroup.ClaimID == claimId).ToList();
        }
    }
}