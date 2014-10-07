using ModelsLayer; 
using System.Collections.Generic;
using System.Data;
using System.Linq; 

namespace Factories
{
    public interface IClaimStatusFactory
    {
        void Initialize();
        ClaimStatu GetClaimStatu(int claimStatuId);
        List<ClaimStatu> GetClaimStatus();
        bool CreateClaimStatu(ClaimStatu claimStatus);
        bool UpdateClaimStatu(ClaimStatu claimStatus);
        bool DeleteClaimStatu(ClaimStatu claimStatus);
        void Dispose(bool disposing);
    }

    public class ClaimStatusFactory : IClaimStatusFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public ClaimStatu GetClaimStatu(int claimStatuId)
        {
            return _db.ClaimStatus.Find(claimStatuId); 
        }

        public List<ClaimStatu> GetClaimStatus()
        {
            return _db.ClaimStatus.ToList(); 
        }

        public bool CreateClaimStatu(ClaimStatu claimStatus)
        {
            _db.ClaimStatus.Add(claimStatus);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClaimStatu(ClaimStatu claimStatus)
        {
            _db.Entry(claimStatus).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClaimStatu(ClaimStatu claimStatus)
        {
            _db.ClaimStatus.Remove(claimStatus);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        } 
    }
}