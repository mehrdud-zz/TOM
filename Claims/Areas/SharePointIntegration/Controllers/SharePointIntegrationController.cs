using Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimsPoC.Areas.SharePointIntegration.Controllers
{
    public class SharePointIntegrationController : Controller
    {
        private readonly IReportFactory _reportFactory;

        public SharePointIntegrationController(IReportFactory reportFactory)
        {
            _reportFactory = reportFactory;
        }

        //
        // GET: /SharePointIntegration/SharePointIntegration/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SharePointDashboard(int reportId)
        {
            var report = _reportFactory.GetReportTemplate(reportId);            
            return View(report);
        }

        public ActionResult SharePointDashboardII(int dashboardId)
        {
            ViewBag.DashboardId = dashboardId;
            return View();

        }


        public ActionResult SharePointFrame()
        {
            return View();
        }


      
    }
}
