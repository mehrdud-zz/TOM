using System.Web.Routing;
using ModelsLayer;
using Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ClaimsPoC.Clients.Controllers
{
    public class ClientClaimTemplateController : Controller
    {

        private readonly IClaimTemplateFactory _claimTemplateFactory;

        // GET: /Claims/ClientClaimTemplate/
        public ClientClaimTemplateController(IClaimTemplateFactory claimTemplateFactory)
        {
            _claimTemplateFactory = claimTemplateFactory;
        }

        public ActionResult Index(int clientId, String viewMode)
        {
            var claimTemplateList = _claimTemplateFactory.GetClientClaimTemplates(clientId);
            ViewBag.ViewMode = viewMode;
            ViewBag.ClientID = clientId;
            return View(claimTemplateList.ToList());
        }


        public ActionResult Delete(int clientId, int claimTemplateId)
        {

            //var clientClaimTemplate = _claimTemplateFactory.GetClientClaimTemplate(clientId, claimTemplateId);

            //if (clientClaimTemplate == null)
            //{
            //    return HttpNotFound();
            //}

            //return View(clientClaimTemplate);

            _claimTemplateFactory.GetClientClaimTemplate(clientId, claimTemplateId);
            return RedirectToAction("Edit", "Client", new { @id = clientId, area = "Clients" });
        }

        //
        // POST: /Contracts/ClientUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int clientId, int claimTemplateId)
        {
            _claimTemplateFactory.GetClientClaimTemplate(clientId, claimTemplateId);
            return RedirectToAction("Edit", "Client", new { @id = clientId, area = "Clients" });
        }
    }
}
