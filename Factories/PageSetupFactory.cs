using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public interface IPageSetupFactory
    {
       

        void Initialize();

        void Initialize(IClaimsEntities db);
        PageSetup GetPageSetup(int pageSetupId);
        List<PageSetup> GetPageSetups();
        bool CreatePageSetup(PageSetup pageSetup);
        bool UpdatePageSetup(PageSetup pageSetup);
        bool DeletePageSetup(PageSetup pageSetup);
        void Dispose(bool disposing);
    }

    public class PageSetupFactory : IPageSetupFactory
    {
        public PageSetupFactory() {
            this.Initialize();
        }

        private IClaimsEntities _db;

        public void Initialize()
        {
            _db = new ClaimsEntities();
        }
        public void Initialize(IClaimsEntities db)
        {
            _db = db;
        }

        public PageSetup GetPageSetup(int pageSetupId)
        {
            return _db.PageSetups.Find(pageSetupId);
        }

        public List<PageSetup> GetPageSetups()
        {
            return _db.PageSetups.ToList();
        }

        public bool CreatePageSetup(PageSetup pageSetup)
        {
            _db.PageSetups.Add(pageSetup);
            _db.SaveChanges();
            return true;
        }

        public bool UpdatePageSetup(PageSetup pageSetup)
        {
            _db.Entry(pageSetup).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeletePageSetup(PageSetup pageSetup)
        {
            _db.PageSetups.Remove(pageSetup);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
          //  _db.Dispose();
        }
    }
}