using ClaimsPoC.ClientHome.Models;
using ClaimsPoC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelsLayer;
using Factories;

namespace ClaimsPoC.ClientHome.Controllers
{

    public class ClaimStatus
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class ClientHomeModel
    {
        public List<ClaimStatusAggregate> ClaimStatusList { get; set; }
        public List<CountyCounter> CountryCounterList { get; set; }
    }

    public class ClientHomeController : Controller
    {
        private IUserFactory userFactory;
        private IClaimFactory claimFactory;
        private IClaimStatusFactory claimStatusFactory;
        private IClaimTemplateFactory claimTemplateFactory;
        private IFieldTypeFactory fieldTypeFactory;
        private ICountryFactory countryFactory;
        private IReportFactory reportFactory;


        public ClientHomeController()
        {
            this.userFactory = new UserFactory();
            this.claimFactory = new ClaimFactory();
            this.claimStatusFactory = new ClaimStatusFactory();
            this.fieldTypeFactory = new FieldTypeFactory();
            this.countryFactory = new CountryFactory();
            this.reportFactory = new ReportFactory();
        }

        public ClientHomeController(IUserFactory userFactory, IClaimFactory claimFactory2, IClaimStatusFactory claimStatusFactory, IClaimTemplateFactory claimTemplateFactory, IFieldTypeFactory fieldTypeFactory, ICountryFactory countryFactory, IReportFactory reportFactory)
        {
            this.userFactory = userFactory;
            this.claimFactory = claimFactory2;
            this.claimStatusFactory = claimStatusFactory;
            this.claimTemplateFactory = claimTemplateFactory;
            this.fieldTypeFactory = fieldTypeFactory;
            this.countryFactory = countryFactory;
            this.reportFactory = reportFactory;
        }

        public ActionResult GetClientStyles()
        {
            string username = HttpContext.User.Identity.Name;

            ClaimsEntities ClaimsEntities = new ClaimsEntities();

            var clientObj =
                from client in ClaimsEntities.Clients
                join users in ClaimsEntities.Users on client.ClientID equals users.ClientID
                select client;

            Client clientENt = clientObj.ToList()[0];

            String clientStyles =
                "<style type='text/css'>" +
                "body{ background-color: " + clientENt.BackgroundColour + "!important;}" +
                "h1{ color: " + clientENt.Heading1 + "!important;}" +
                "h2{ color: " + clientENt.Heading2 + "!important;} " +
                "h3{ color: " + clientENt.Heading3 + "!important;} " +
                ".fieldGroup{ background-color: " + clientENt.Colour1 + "!important;} " +
                "</style>";

            ViewBag.ClientStyles = clientStyles;

            Response.Write(clientStyles);
            HttpContext.ApplicationInstance.CompleteRequest();

            return View();
        }

        //
        // GET: /ClientHome/ClientHome/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClaimSubmittedSuccessfully()
        {
            return View();
        }

        public ActionResult ClaimForm(int claimTemplateId)
        {
            var claimTemplateController = new Claims.Controllers.ClaimTemplateController();
            var claimTemplate = claimTemplateController.GetClaimTemplate(claimTemplateId);
            return View(claimTemplate);
        }

        public ActionResult ClaimFormDetails(int claimId)
        {
            var claimController = new Claims.Controllers.ClaimController();
            var claim = claimController.GetClaim(claimId);
            return View(claim);
        }

        public ActionResult ClaimFormEdit(int claimId)
        {
            var claim = claimFactory.GetClaim(claimId);
            if (claim == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClaimStatusID = new SelectList(claimStatusFactory.GetClaimStatus(), "ClaimStatusID", "Name", claim.ClaimStatusID);
            ViewBag.CountryID = new SelectList(countryFactory.GetCountries(), "CountryID", "Name", claim.CountryID);
            return View(claim);
        }

        [HttpPost]
        public ActionResult ClaimFormEdit(Claim clientClaim, FormCollection formCollection)
        {
            var claimController = new Claims.Controllers.ClaimController();
            claimController.UpdateFromTemplate(clientClaim, formCollection);
            return RedirectToAction("ClaimSubmittedSuccessfully");
        }


        [HttpPost]
        public ActionResult ClaimForm(Claim clientClaim, FormCollection formCollection)
        {
            var claimController = new Claims.Controllers.ClaimController();
            claimController.CreateFromTemplate(clientClaim.ClaimTemplateID.Value, formCollection);
            return RedirectToAction("ClaimSubmittedSuccessfully");
        }





        public ActionResult ClaimSection(IEnumerable<ClaimField> ClaimFieldArray)
        {
            return View(ClaimFieldArray);
        }

        [HttpPost]
        public ActionResult ClaimSection(ClientHome.Models.ClientClaim ClientClaim)
        {
            return RedirectToAction("Index", "ClientHome");
        }


        public ActionResult ListClientClaims()
        {
            User currentUser = userFactory.GetCurrentUser();

            ModelsLayer.ClaimsEntities ClaimsEntities = new ModelsLayer.ClaimsEntities();

            var clientClaims =
                from claims in ClaimsEntities.Claims
                where claims.UserID == currentUser.UserId
                select claims;

            List<Claim> claimsList = clientClaims.ToList();

            return View(claimsList);
        }

        public ActionResult ListClientClaimTemplates(int ClientID)
        {
            List<ClaimTemplate> clientClaimTemplates =
                claimTemplateFactory.GetClientClaimTemplates(ClientID);



            return View(clientClaimTemplates);
        }






        public ActionResult GoogleDonutChart(String StatusName)
        {
            ModelsLayer.User clientUser = GetUserID();

            ModelsLayer.ClaimsEntities ClaimsEntities = new ModelsLayer.ClaimsEntities();

            var c =
                ClaimsEntities.Claims
                .GroupBy(n => n.ClaimStatu.Name)
                .Select(n => new ClaimStatusAggregate
                {
                    Status = n.Key,
                    Counter = n.Count()
                }
                )
                .OrderBy(n => n.Status);


            return View(c.ToList());
        }


        public ActionResult ClientReports()
        {
            ClientHomeModel clientHomeModel = new ClientHomeModel();

            ModelsLayer.User cu = GetUserID();

            //if (cu != null)
            //{
            //    Guid UserID = cu.UserId;

            //    ModelsLayer.ClaimsEntities claimsModel =
            //        new ModelsLayer.ClaimsEntities();

            //    var countyCounter =
            //     from claims in claimsModel.Claims
            //     where claims.UserID == UserID
            //     group claims by new { country = claims.Country.Name, status = claims.ClaimStatu.Name } into grp
            //     let counter = grp.Count()
            //     select new CountyCounter() { Country = grp.Key.country, Counter = counter, Status = grp.Key.status };




            //    var c =
            //        claimsModel.Claims
            //        .GroupBy(n => n.ClaimStatu.Name)
            //        .Select(n => new ClaimStatusAggregate
            //        {
            //            Status = n.Key,
            //            Counter = n.Count()
            //        }
            //        )
            //        .OrderBy(n => n.Status);

            //    clientHomeModel.ClaimStatusList = c.ToList();
            //    clientHomeModel.CountryCounterList = countyCounter.ToList();



            //    return View(clientHomeModel);
            //}
            //else

            return View();
        }


        public ModelsLayer.User GetUserID()
        {
            ModelsLayer.User clientUser = null;

            if (HttpContext.User != null && HttpContext.User.Identity != null &&
                !String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                string clientUsername = HttpContext.User.Identity.Name;

                ClaimsEntities ClaimsEntities = new ClaimsEntities();
                var clientUser2 =
                    from clientUsers in ClaimsEntities.Users
                    where clientUsers.UserName == clientUsername
                    select clientUsers;

                if (clientUser2 != null && clientUser2.Count() > 0)
                    clientUser = clientUser2.Single<ModelsLayer.User>();

            }

            return clientUser;

        }


        public ActionResult GetFieldTemplate(ClaimFieldTemplate ClaimFieldTemplate, String FieldMode)
        {
            //string claimFieldTemplateName = ClaimFieldTemplate.FieldType.TemplateName;

            //switch (claimFieldTemplateName)
            //{
            //    case "ShortText":
            //        break;
            //    case "LongText":
            //        break;
            //    case "Integer":
            //        break;
            //    case "Float":
            //        break;
            //    case "Date":
            //        break;
            //    case "DateTime":
            //        break;
            //    case "DropDown":
            //        break;
            //    case "MultiChoice":
            //        //return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimTempalteID });

            //        return PartialView("MultiSelectTemplate", new { claimField = ClaimFieldTemplate, FieldMode = FieldMode });
            //        break;
            //    case "File":
            //        break;
            //    case "Money":
            //        break;
            //    case "Country":
            //        break;
            //    case "Range":
            //        break;
            //    default:
            //        break;
            //}
            ViewBag.ClaimFieldTemplateName = ClaimFieldTemplate.FieldType.TemplateName;
            FieldTemplate FieldTemplate = new FieldTemplate() { ClaimFieldTemplate = ClaimFieldTemplate, FieldMode = FieldMode };
            return View(FieldTemplate);
        }



    }
}
