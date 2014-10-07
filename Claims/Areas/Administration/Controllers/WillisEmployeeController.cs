using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ModelsLayer;


// ReSharper disable once CheckNamespace
namespace ClaimsPoC.Administration.Controllers
{
    public class WillisEmployeeController : Controller
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public ClaimsEntities Db
        {
            get { 
                return _db;
            }
        }

        public ActionResult Home()
        {
            return View();
        }


        public ActionResult Index()
        {
            var willisEmployees = _db.WillisEmployees.ToList<WillisEmployee>();
            return View(willisEmployees);
        }

        //
        // GET: /Administration/WillisEmployee/Details/5

        public ActionResult Details(int id = 0)
        {
            WillisEmployee willisemployee = _db.WillisEmployees.Find(id);
            if (willisemployee == null)
            {
                return HttpNotFound();
            }
            return View(willisemployee);
        }

        //
        // GET: /Administration/WillisEmployee/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administration/WillisEmployee/Create

        [HttpPost]
        public ActionResult Create(WillisEmployee willisemployee)
        {
            if (ModelState.IsValid)
            {
                _db.WillisEmployees.Add(willisemployee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(willisemployee);
        }

        //
        // GET: /Administration/WillisEmployee/Edit/5

        public ActionResult Edit(int id = 0)
        {
            WillisEmployee willisemployee = _db.WillisEmployees.Find(id);
            if (willisemployee == null)
            {
                return HttpNotFound();
            }
            return View(willisemployee);
        }

        //
        // POST: /Administration/WillisEmployee/Edit/5

        [HttpPost]
        public ActionResult Edit(WillisEmployee willisemployee)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(willisemployee).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(willisemployee);
        }

        //
        // GET: /Administration/WillisEmployee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            WillisEmployee willisemployee = _db.WillisEmployees.Find(id);
            if (willisemployee == null)
            {
                return HttpNotFound();
            }
            return View(willisemployee);
        }

        //
        // POST: /Administration/WillisEmployee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            WillisEmployee willisemployee = _db.WillisEmployees.Find(id);
            _db.WillisEmployees.Remove(willisemployee);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        public bool IsWillisEmployeeInRole(string username, string role)
        {

            bool result = false;
            using (var context = new ClaimsEntities())
            {
                var userList =
                    from we in context.WillisEmployees
                    where we.EmployeeUserID == username
                    select we.EmployeeUserID;

                if (userList.Count() == 1)
                {
                    result = true;
                }
            }
            return result;
        }

        public WillisEmployee GetWillisEmployee(string username)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException("Username cannot be null");

            var willisEmployee =
                from we in _db.WillisEmployees
                where we.EmployeeUserID == username
                select we;

            if (willisEmployee.Count() == 1)
            {
                return willisEmployee.First<WillisEmployee>();
            }
            else
            {
                throw new NullReferenceException("User cannot be found!", new Exception("User could not be found in the database: " + username));
            }
        }

    }
}