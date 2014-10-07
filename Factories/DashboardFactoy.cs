using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public interface IDashboardFactory
    {

        void Initialize();    
        void Initialize(ClaimsEntities claimsEntities);        
        Dashboard GetDashboard(int pageSetupId);
        List<Dashboard> GetDashboards();
        bool CreateDashboard(Dashboard dashboard);
        bool UpdateDashboard(Dashboard dashboard);
        bool DeleteDashboard(Dashboard dashboard);
        void Dispose(bool disposing);
    }

    public class DashboardFactory : IDashboardFactory
    {
        private  ClaimsEntities _db = new ClaimsEntities();


         
        public void Initialize()
        {

        }

        public void Initialize(ClaimsEntities claimsEntities) {
            _db = claimsEntities;
        }

        public Dashboard GetDashboard(int dashboardId)
        {
            var dashboard = _db.PageSetups.Single(m => m.PageSetupID == dashboardId);
            return (Dashboard)dashboard;
        }

        public List<Dashboard> GetDashboards()
        {
            var dashboardList = _db.PageSetups.ToList();
            var result = new List<Dashboard>();
            foreach (var dashboard in dashboardList)
                result.Add((Dashboard)dashboard);

            return result;
        }

        public bool CreateDashboard(Dashboard dashboard)
        {
            _db.PageSetups.Add(dashboard);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateDashboard(Dashboard dashboard)
        {
            _db.Entry(dashboard).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteDashboard(Dashboard dashboard)
        {
            _db.PageSetups.Remove(dashboard);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }



    }
}