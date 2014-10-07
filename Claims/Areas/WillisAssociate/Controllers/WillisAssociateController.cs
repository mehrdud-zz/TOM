using Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimsPoC.Areas.WillisAssociate.Controllers
{
    public class ClientClaimTemplateWizardModel
    {
        public int ClientID { get; set; }
        public int ClaimTemplateID { get; set; }
    }
    public class WillisAssociateController : Controller
    {
        private IClientFactory clientFactory;
        private IClaimTemplateFactory claimTemplateFactory;
        private IReportFactory reportFactory;
        //
        // GET: /WillisAssociate/WillisAssociate/



        public WillisAssociateController(IClientFactory clientFactory, IClaimTemplateFactory claimTemplateFactory, IReportFactory reportFactory)
        {
            this.clientFactory = clientFactory;
            this.claimTemplateFactory = claimTemplateFactory;
            this.reportFactory = reportFactory;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Wizard(int? ClientID)
        {
            List<SelectListItem> clientList = new List<SelectListItem>();
            foreach (ModelsLayer.Client client in clientFactory.GetClients())
            {
                clientList.Add(new SelectListItem()
                {
                    Text = client.Name,
                    Value = client.ClientID.ToString(),
                    Selected = (ClientID != null && client.ClientID == (int)ClientID) ? true : false
                });
            }


            List<SelectListItem> claimTemplateList = new List<SelectListItem>();
            foreach (ModelsLayer.ClaimTemplate claimTemplate in claimTemplateFactory.GetClaimTemplates())
            {
                claimTemplateList.Add(new SelectListItem()
                {
                    Text = claimTemplate.Name,
                    Value = claimTemplate.ClaimTemplateID.ToString()
                });
            }


            ViewBag.ClientsSelectList = clientList;
            ViewBag.ClaimTemplatesSelectList = claimTemplateList;

            string username = User.Identity.Name;
            ViewBag.ClaimTemplatesList = claimTemplateFactory.GetClaimTemplatesClientDoesntHave(username);
            return View();
        }

        [HttpPost]
        public ActionResult Wizard(ClientClaimTemplateWizardModel modelItem)
        {
            bool result = clientFactory.AddClaimTemplateToClient(modelItem.ClientID, modelItem.ClaimTemplateID);
            if (result)
            {
                return RedirectToAction("Details", "Client", new { area = "Clients", id = modelItem.ClientID });
            }
            return View();
        }

        public List<SelectListItem> GetSelectList(List<object> list, string name, string key)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            foreach (object item in list)
            {
                itemList.Add(new SelectListItem()
                {
                    Text = item.GetType().GetProperty(name).GetValue(item, null).ToString(),
                    Value = item.GetType().GetProperty(key).GetValue(key, null).ToString()
                });
            }

            return itemList;
        }


        public ActionResult Reports()
        {
            ClaimsStatusReport claimsStatusReport = reportFactory.GetClaimStatusReport();
            return View(claimsStatusReport);
        }

    }
}
