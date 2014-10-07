using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ModelsLayer;
using Factories;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{



    public class ClaimTemplateController : Controller
    {
        private readonly IClaimTemplateFactory _claimTemplateFactory;

        public ClaimTemplateController() { }


        public ClaimTemplateController(IClaimTemplateFactory claimTemplateFactory)
        {
            _claimTemplateFactory = claimTemplateFactory;
        }
        //
        // GET: /Administration/ClaimTemplate/

        public ActionResult Index()
        {
            return View(_claimTemplateFactory.GetClaimTemplates());
        }

        //
        // GET: /Administration/ClaimTemplate/Details/5

        public ActionResult Details(int id = 0)
        {
            ViewBag.ClaimTemplateID = id;
            ClaimTemplate claimtemplate = _claimTemplateFactory.GetClaimTemplate(id);
            if (claimtemplate == null)
            {
                return HttpNotFound();
            }
            return View(claimtemplate);
        }

        //
        // GET: /Administration/ClaimTemplate/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Administration/ClaimTemplate/Create

        [HttpPost]
        public ActionResult Create(ClaimTemplate claimtemplate)
        {
            if (!ModelState.IsValid) return View();
            _claimTemplateFactory.CreateClaimTemplate(claimtemplate);
            return RedirectToAction("Edit", "ClaimTemplate", new { area = "Claims", @id = claimtemplate.ClaimTemplateID });
        }

        //
        // GET: /Administration/ClaimTemplate/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.ClaimTemplateID = id;
            var claimtemplate = _claimTemplateFactory.GetClaimTemplate(id);
            if (claimtemplate == null)
            {
                return HttpNotFound();
            }
            return View(claimtemplate);
        }

        //
        // POST: /Administration/ClaimTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(ClaimTemplate claimtemplate)
        {
            if (!ModelState.IsValid) return View(claimtemplate);
            _claimTemplateFactory.UpdateClaimTemplate(claimtemplate);
            return RedirectToAction("Index");
        }

        //
        // GET: /Administration/ClaimTemplate/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var claimtemplate = _claimTemplateFactory.GetClaimTemplate(id);
            if (claimtemplate == null)
            {
                return HttpNotFound();
            }
            return View(claimtemplate);
        }

        //
        // POST: /Administration/ClaimTemplate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimTemplate claimtemplate = _claimTemplateFactory.GetClaimTemplate(id);
            _claimTemplateFactory.DeleteClaimTemplate(claimtemplate);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _claimTemplateFactory.Dispose(disposing);
            base.Dispose(disposing);
        }



        public List<ClaimTemplate> GetClientClaimTemplatesByUsername(string username)
        {
            return _claimTemplateFactory.GetClientClaimTemplates(username);
        }


        //public ActionResult GetClientClaimTemplatesByUsernameJson(string username)
        //{
        //    List<ClaimTemplate> clientClaimTemplates = _claimTemplateFactory.GetClientClaimTemplates(username);
        //    return Json(
        //        new
        //        {
        //            Result = (from obj in clientClaimTemplates select new { ClaimTemplateID = obj.ClaimTemplateID, Name = obj.Name })
        //        }
        //        , JsonRequestBehavior.AllowGet);
        //}



        public List<ClaimTemplate> GetClientClaimTemplates(int clientId)
        {

            return _claimTemplateFactory.GetClientClaimTemplates(clientId);
        }


        public ClaimTemplate GetClaimTemplate(int claimTemplateId)
        {
            return _claimTemplateFactory.GetClaimTemplate(claimTemplateId);
        }





        public ActionResult AddToClient(int clientId)
        {
            ViewBag.ClaimTemplateID = new SelectList(_claimTemplateFactory.GetClaimTemplates(), "ClaimTemplateID", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult AddToClient(int clientId, int claimTemplateId)
        {
            var claimsEntities = new ClaimsEntities();
            var claimTemplate = claimsEntities.ClaimTemplates.Single(ct => ct.ClaimTemplateID == claimTemplateId);
            claimsEntities.Clients.Single(c => c.ClientID == clientId).ClaimTemplates.Add(claimTemplate);
            claimsEntities.SaveChanges();
            return RedirectToAction("Edit", "Client", new { @area = "Clients", @id = clientId });
        }

        public ActionResult  DeleteClientClaimTemplate(int clientId, int claimTemplateId)
        {
            _claimTemplateFactory.DeleteClientClaimTemplate(clientId,claimTemplateId);
            return RedirectToAction("Edit", "Client", new { @area = "Clients", @id = clientId });
        }
        

        public ActionResult GetClaimTemplates()
        {
            var claimTemplates = _claimTemplateFactory.GetClaimTemplates();
            return Json(claimTemplates.Select(role => new { Value = role.ClaimTemplateID, Title = role.Name }), JsonRequestBehavior.AllowGet);
        }


    }
}