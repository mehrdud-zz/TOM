using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Factories
{
    public interface IWillisEmployeeFactory
    {
        void Initialize();
        WillisEmployee GetWillisEmployee(int WillisEmployeeID);
        List<WillisEmployee> GetWillisEmployees();
        bool CreateWillisEmployee(WillisEmployee WillisEmployee);
        bool UpdateWillisEmployee(WillisEmployee WillisEmployee);
        bool DeleteWillisEmployee(WillisEmployee WillisEmployee);
        void Dispose(bool Disposing);
    }

    public class WillisEmployeeFactory : IWillisEmployeeFactory
    {
        private ModelsLayer.ClaimsEntities db = new ModelsLayer.ClaimsEntities();

        public void Initialize()
        {

        }

        public WillisEmployee GetWillisEmployee(int WillisEmployeeID)
        {
            WillisEmployee WillisEmployee = db.WillisEmployees.Find(WillisEmployeeID);
            return WillisEmployee;
        }

        public List<WillisEmployee> GetWillisEmployees()
        {
            var WillisEmployees = db.WillisEmployees;
            List<WillisEmployee> WillisEmployeeList = WillisEmployees.ToList();
            return WillisEmployeeList;
        }

        public bool CreateWillisEmployee(WillisEmployee WillisEmployee)
        {
            db.WillisEmployees.Add(WillisEmployee);
            db.SaveChanges();
            return true;
        }

        public bool UpdateWillisEmployee(WillisEmployee WillisEmployee)
        {
            db.Entry(WillisEmployee).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool DeleteWillisEmployee(WillisEmployee WillisEmployee)
        {
            db.WillisEmployees.Remove(WillisEmployee);
            db.SaveChanges();
            return true;
        }

        public void Dispose(bool Disposing)
        {
            db.Dispose();
        }



    }
}