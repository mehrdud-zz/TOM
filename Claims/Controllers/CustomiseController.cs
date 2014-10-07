using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ModelsLayer;

namespace ClaimsPoC.Controllers
{

    public class CustomiseController : Controller
    {
        //
        // GET: /Customise/

        private readonly Factories.IPageElementFactory _pageElementFactory;
        private readonly Factories.IReportFactory _reportFactory;
        private readonly Acturis.Factories.IActurisClaimFactory _acturisFactory;

        public CustomiseController(Factories.IPageElementFactory pageElementFactory, Factories.IReportFactory reportFactory, Acturis.Factories.IActurisClaimFactory acturisFactory)
        {
            this._pageElementFactory = pageElementFactory;
            this._reportFactory = reportFactory;
            this._acturisFactory = acturisFactory;
        }

        [HttpPost]
        public ActionResult SavePage(string jsonOfLog)
        {
            var pageElementDetailList = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Factories.PageElementDetailList>(jsonOfLog);

            _pageElementFactory.SavePage(pageElementDetailList, User.Identity.Name);

            var result = pageElementDetailList.PageElementDetails.Count().ToString(CultureInfo.InvariantCulture);
            return Content(result);
        }


        public ActionResult DisplayPageElements(string username)
        {
            var pageElementList = _pageElementFactory.GetPageElementListByUsername(username);

            return View(pageElementList);
        }

        public JsonResult GetPageElementListByUsernameandPageUrl(string pageUrl, int reportId = 0)
        {
            var username = User.Identity.Name;

            var pageElementList = _pageElementFactory.GetPageElementListByUsernameandPageUrl(username, pageUrl);

            //if (pageElementList.Any())
            //{
            //    pageElementList = new List<PageElement>
            //    {
            //        new PageElement
            //        {
            //            Width = 640,
            //            Height = 480,
            //            ElementTop = 100,
            //            ElementLeft = 100,
            //            Title = "",
            //            ReportID = reportId,
            //            FrameID = "Frame" + Guid.NewGuid().ToString()

            //        }
            //    };
            //}


            //  var pet = (from m in pageElementList select new PageElementToken(m)).ToList<PageElementToken>();

            var pet = new List<PageElementToken>();
            foreach (var pageElement in pageElementList)
            {
                pet.Add(new PageElementToken(pageElement));
            }

            return Json(
             new
             {
                 Result = pet
             }
             , JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetPageElementListByDashboardId(int dashboardId = 0, int reportId = 0)
        {
            if (dashboardId > 0)
            {
                var pageElementList = _pageElementFactory.GetPageElementListByDashboardId(dashboardId);

                var pet = new List<PageElementToken>();
                foreach (var pageElement in pageElementList)
                {
                    pet.Add(new PageElementToken(pageElement));
                }


                var clientList = _acturisFactory.GetClientList();
                return Json(
                 new
                 {
                     Result = pet,
                     ClientList = clientList
                 }
                 , JsonRequestBehavior.AllowGet);
            }
            else
            {

                var report = _reportFactory.GetReportTemplate(reportId);
                var pet = new List<PageElementToken>();
                var pageElementToken = new PageElementToken();

                pageElementToken.Width = 600;
                pageElementToken.Height = 400;
                pageElementToken.Left = 0;
                pageElementToken.Top = 0;
                pageElementToken.PageElementId = 0;
                pageElementToken.ReportId = reportId; 
                pageElementToken.Title = report.Name;
                pageElementToken.Filters = new ModelsLayer.Filter[0];



                foreach (ReportField field in report.ReportFields)
                {
                    if (field.Filter != null && field.Filter.Value == true)
                    {
                        Array.Resize(ref pageElementToken.Filters, pageElementToken.Filters.Length + 1);
                        pageElementToken.Filters[pageElementToken.Filters.Length - 1] = new ModelsLayer.Filter() { FieldId = field.FieldId.Value, Name = field.DisplayName };
                    }
                }
                pet.Add(pageElementToken);
                var clientList = _acturisFactory.GetClientList();
                return Json(
            new
            {
                Result = pet,
                ClientList = clientList
            }
            , JsonRequestBehavior.AllowGet);

            }
            return Json(
               new
               {
         
               }
               , JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetReportsList()
        {
            var reportsList = _reportFactory;
            return Json(
                new
                {
                    Result = reportsList
                }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
