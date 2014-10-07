using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using ModelsLayer;
using Factories;

namespace ClaimsPoC.Clients.Controllers
{



    public class ClientController : Controller
    {
        private readonly IClientFactory clientFactory;

        public ClientController(IClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        //
        // GET: /Contracts/Client/
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View(clientFactory.GetClients());
        }

        //
        // GET: /Contracts/Client/Details/5
        public ActionResult Details(int id = 0)
        {
            Client client = clientFactory.GetClient(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        //
        // GET: /Contracts/Client/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contracts/Client/Create

        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                clientFactory.CreateClient(client);

                //WillisAssociate/WillisAssociate/Wizard/
                if (Request.QueryString["source"] != null && Request.QueryString["source"] == "wizard")
                {
                    return RedirectToAction("Wizard", "WillisAssociate", new { area = "WillisAssociate", ClientID = client.ClientID });
                }
                return RedirectToAction("Index");
            }

            return View(client);
        }

        //
        // GET: /Contracts/Client/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Client client = clientFactory.GetClient(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //
        // POST: /Contracts/Client/Edit/5

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                clientFactory.UpdateClient(client);
                return RedirectToAction("Index");
            }
            return View(client);
        }

        //
        // GET: /Contracts/Client/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Client client = clientFactory.GetClient(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //
        // POST: /Contracts/Client/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = clientFactory.GetClient(id);
            clientFactory.DeleteClient(client);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            clientFactory.Dispose(disposing);
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Client_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<Client> Clients = clientFactory.GetClients();
            DataSourceResult result = Clients.ToDataSourceResult(request);
            return Json(result);
        }


        //public List<ClaimTemplate> GetTemplate(int ClientID)
        //{
        //    var clientTemplates =
        //        from clientClaimTemplates in db.ClientClaimTemplates
        //        join clients in db.Clients on clientClaimTemplates.ClientID equals clients.ClientID
        //        where clients.ClientID == ClientID
        //        select new { ClaimTemplateID = clientClaimTemplates.ClaimTemplateID };

        //    List<int> clientTemplateIDList = new List<int>();

        //    foreach (var clientTemplate in clientTemplates)
        //    {
        //        clientTemplateIDList.Add(clientTemplate.ClaimTemplateID);
        //    }

        //    Claims.Controllers.ClaimTemplateController claimController = new Claims.Controllers.ClaimTemplateController();

        //    List<ClaimTemplate> result = new List<ClaimTemplate>();

        //    foreach (int claimTemplateID in clientTemplateIDList)
        //    {
        //        result.Add(claimController.GetClaimTemplate(claimTemplateID));
        //    }
        //    return result;
        //}

        public void RegisterClient()
        {

        }

    }
}