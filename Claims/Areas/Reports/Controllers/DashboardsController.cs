using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimsPoC.Areas.Reports.Views.Dashboards
{
    public class DashboardsController : Controller
    {
        //
        // GET: /Reports/Dashboards/

        private readonly Factories.IPageSetupFactory _dashboardFactory;
        private readonly Factories.IPageElementFactory _pageElementFactory;

        public DashboardsController(Factories.IPageSetupFactory dashboardFactory, Factories.IPageElementFactory pageElementFactory)
        {
            _dashboardFactory = dashboardFactory;
            _pageElementFactory = pageElementFactory;
        }
        //
        // GET: /Dashboard/Dashboard/

        public ActionResult Index()
        {
            return View(_dashboardFactory.GetPageSetups());
        }

        //
        // GET: /Dashboard/Dashboard/Details/5

        public ActionResult Details(int DashboardId = 0)
        {
            ViewBag.DashboardId = DashboardId;
            return View();
        }

         


        public ActionResult SharePointDashboard(int id = 0)
        {
            return View(_dashboardFactory.GetPageSetup(id));
        }


        //
        // GET: /Dashboard/Dashboard/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Dashboard/Dashboard/Create

        [HttpPost]
        public ActionResult Create(ModelsLayer.PageSetup dashboard)
        {
            _dashboardFactory.CreatePageSetup(dashboard);
            return View(dashboard);
        }

        //
        // GET: /Dashboard/Dashboard/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var claimstatu = _dashboardFactory.GetPageSetup(id);
            return View(claimstatu);
        }

        //
        // POST: /Dashboard/Dashboard/Edit/5

        [HttpPost]
        public ActionResult Edit(ModelsLayer.PageSetup dashboard)
        {
            _dashboardFactory.UpdatePageSetup(dashboard);
            return View(dashboard);
        }

        //
        // GET: /Dashboard/Dashboard/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var dashboard = _dashboardFactory.GetPageSetup(id);
            return View(dashboard);
        }

        //
        // POST: /Dashboard/Dashboard/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var dashboard = _dashboardFactory.GetPageSetup(id);
            _dashboardFactory.DeletePageSetup(dashboard);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _dashboardFactory.Dispose(disposing);
        }



        [HttpPost]
        public ActionResult SaveDashboard(string jsonOfLog)
        {
            var pageElementDetailList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Factories.PageElementDetailList>(jsonOfLog);

            _pageElementFactory.SavePage(pageElementDetailList, User.Identity.Name);

            var result = pageElementDetailList.PageElementDetails.Count().ToString(System.Globalization.CultureInfo.InvariantCulture);
            return Content(result);
        }

    }
}
