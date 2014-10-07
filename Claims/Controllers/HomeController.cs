using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimsPoC.Claims.Controllers;
using ModelsLayer;

namespace ClaimsPoC.Controllers
{
    public class CountyCounter
    {
        public string Country { get; set; }
        public string Status { get; set; }
        public int Counter { get; set; }
    }


    public class ClaimStatusAggregate
    {
        public string Status { get; set; }
        public int Counter { get; set; }
    }

    public class HomeController : WillisController
    {
        //
        // GET: /Willis/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }



        public ActionResult Navigation()
        {
            return PartialView();
        }


        public void GetClaimsBasedOnCountry()
        {

        }

        //public ActionResult Notifications()
        //{
            
        //    ClaimController controller =
        //         new ClaimController();

        //    int claimCount = controller.GetClaimsCount();
        //    ViewBag.ClaimCount = claimCount;
        //    return View(claimCount);
        //}

      

        public ActionResult Articles()
        {
            var claimsModel =
                new ClaimsEntities();

            var claim1 =
             from claimStatus in claimsModel.ClaimStatus
             select claimStatus;

            return View(claim1.ToList());
        }


        public ActionResult AdminTasks()
        {
            return View();
        }
    }
}
