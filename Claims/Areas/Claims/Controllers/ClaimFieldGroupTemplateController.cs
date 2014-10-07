using System;
using System.Web.Mvc;
using Factories;
using ModelsLayer;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class ClaimFieldGroupTemplateController : Controller
    {
        private readonly IClaimFieldGroupTemplateFactory _claimFieldGroupTemplateFactory;
        //
        // GET: /Claims/ClaimField/

        public ClaimFieldGroupTemplateController(IClaimFieldGroupTemplateFactory claimFieldGroupTemplateFactory)
        {
            _claimFieldGroupTemplateFactory = claimFieldGroupTemplateFactory;
        }

        //
        // GET: /Claims/ClaimField/Details/5



        //
        // GET: /Claims/ClaimField/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /Claims/ClaimField/Create

        [HttpPost]
        public ActionResult Create(ClaimFieldGroupTemplate claimFieldGroupTemplate)
        {
            if (!ModelState.IsValid) return View(claimFieldGroupTemplate);
            _claimFieldGroupTemplateFactory.CreateClaimFieldGroupTemplate(claimFieldGroupTemplate);
            return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimFieldGroupTemplate.ClaimTemplateID });
        }

        //
        // GET: /Claims/ClaimField/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var claimFieldGroupTemplate = _claimFieldGroupTemplateFactory.GetClaimFieldGroupTemplate(id);
            return View(claimFieldGroupTemplate);
        }

        //
        // POST: /Claims/ClaimField/Edit/5

        [HttpPost]
        public ActionResult Edit(ClaimFieldGroupTemplate claimFieldGroupTemplate)
        {
            if (!ModelState.IsValid) return View(claimFieldGroupTemplate);
            _claimFieldGroupTemplateFactory.UpdateClaimFieldGroupTemplate(claimFieldGroupTemplate);
            if (HttpContext.Request.QueryString["mode"] == "grid" ||
                HttpContext.Request.QueryString["mode"] == "fromTemplate")
                return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimFieldGroupTemplate.ClaimTemplateID });

            return View(claimFieldGroupTemplate);
        }

        //
        // GET: /Claims/ClaimField/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var claimFieldGroupTemplate = _claimFieldGroupTemplateFactory.GetClaimFieldGroupTemplate(id);
            if (claimFieldGroupTemplate == null)
            {
                return HttpNotFound();
            }
            return View(claimFieldGroupTemplate);
        }

        //
        // POST: /Claims/ClaimField/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var claimFieldGroupTemplate = _claimFieldGroupTemplateFactory.GetClaimFieldGroupTemplate(id);
            _claimFieldGroupTemplateFactory.DeleteClaimFieldGroupTemplate(claimFieldGroupTemplate);
            var claimTemplateId = Convert.ToInt32(HttpContext.Request.QueryString["ClaimTemplateID"]);
            return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimTemplateId });
        }

        protected override void Dispose(bool disposing)
        {
            _claimFieldGroupTemplateFactory.Dispose(disposing);
        }
    }
}