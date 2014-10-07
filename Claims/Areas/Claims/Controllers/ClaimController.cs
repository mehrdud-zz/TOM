using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ModelsLayer;
using Factories;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class ClaimController : Controller
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();
        private readonly IUserFactory _userFactory;
        private readonly IClaimFactory _claimFactory;
        private readonly IClaimStatusFactory _claimStatusFactory;
        private readonly IClaimTemplateFactory _claimTemplateFactory;
        private readonly IFieldTypeFactory _fieldTypeFactory;
        private readonly Acturis.Factories.IActurisClaimFactory _acturisClaimFactory;
        private readonly Eclipse.Factories.IEclipseClaimFactory _eclipseClaimFactory;
        private readonly Eclipse.Factories.IEclipsePolicyFactory _eclipsePolicyFactory;



        public ClaimController()
        {
        }
        public ClaimController(IUserFactory userFactory, IClaimFactory claimFactory, IClaimStatusFactory claimStatusFactory, IClaimTemplateFactory claimTemplateFactory, IFieldTypeFactory fieldTypeFactory
            , Acturis.Factories.IActurisClaimFactory acturisClaimFactory, Eclipse.Factories.IEclipseClaimFactory iEclipseClaimFactory, Eclipse.Factories.IEclipsePolicyFactory eclipsePolicyFactory
            )
        {
            _userFactory = userFactory;
            _claimFactory = claimFactory;
            _claimStatusFactory = claimStatusFactory;
            _claimTemplateFactory = claimTemplateFactory;
            _fieldTypeFactory = fieldTypeFactory;
            _acturisClaimFactory = acturisClaimFactory;
            _eclipseClaimFactory = iEclipseClaimFactory;
            _eclipsePolicyFactory = eclipsePolicyFactory;
        }

        //
        // GET: /Claims/Claim/

        public ActionResult Index()
        {
            var claims = _claimFactory.GetClaims();
            var acturisClaims = _acturisClaimFactory.GetClaims();
            var eclipseClaims = _eclipseClaimFactory.GetClaims();
            var eclipsePolicy = _eclipsePolicyFactory.GetPolicies();
            var totalClaims = claims.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaim>();
            totalClaims.AddRange(acturisClaims);
            totalClaims.AddRange(eclipseClaims);
            totalClaims.AddRange(eclipsePolicy);


            var finalResult =
                from claim in totalClaims
                orderby claim.ClaimDate descending
                select claim;

            return View(finalResult);

        }

        public ActionResult Home()
        {
            return View();
        }

        //
        // GET: /Claims/Claim/Details/5

        public ActionResult Details(int id = 0, string source = Constants.CFT_SOURCE)
        {
            BrokingPlatformIntegrationBase.Interfaces.IClaim claim = null;

            if (id > 0)
            {
                switch (source)
                {
                    case Constants.ACTURIS_SOURCE:
                        claim = _acturisClaimFactory.GetClaim(id);
                        break;
                    case Constants.ECLIPSE_SOURCE:
                        claim = _eclipseClaimFactory.GetClaim(id);
                        break;
                    case Constants.ECLIPSE_POLICY_SOURCE:
                        claim = _eclipsePolicyFactory.GetPolicy(id);
                        break;
                    default:
                        claim = _claimFactory.GetClaim(id);
                        break;

                }
            }

            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        //
        // GET: /Claims/Claim/Create

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Claim claim, FormCollection formCollection)
        {
            if (!ModelState.IsValid) return View(claim);

            var claimId = CreateFromTemplate(((claim.ClaimTemplateID != null) ? claim.ClaimTemplateID.Value : 0), formCollection);
            var newClaim = _claimFactory.GetClaim(claimId);
            newClaim.Name = claim.Name;
            newClaim.ClaimStatusID = claim.ClaimStatusID;
            newClaim.DateCreated = DateTime.Now;
            newClaim.Description = claim.Description;
            newClaim.CountryID = claim.CountryID;
            _claimFactory.UpdateClaim(newClaim);
            return RedirectToAction("Index");
        }


        //
        // GET: /Claims/Claim/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var claim = _claimFactory.GetClaim(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }




        //
        // POST: /Claims/Claim/Edit/5

        [HttpPost]
        public ActionResult Edit(Claim claim, FormCollection formCollection)
        {
            if (!ModelState.IsValid) return View(claim);
            UpdateFromTemplate(claim, formCollection);
            return RedirectToAction("Index");
        }

        //
        // GET: /Claims/Claim/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Claim claim = _claimFactory.GetClaim(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        //
        // POST: /Claims/Claim/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Claim claim = db.Claims.Find(id);
            //db.Claims.Remove(claim);
            //db.SaveChanges();
            Claim claim = _claimFactory.GetClaim(id);
            _claimFactory.DeleteClaim(claim);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _claimFactory.Dispose(disposing);
            base.Dispose(disposing);
        }

        public Claim GetClaim(int claimId)
        {
            return _claimFactory.GetClaim(claimId);
        }

        public int CreateFromTemplate(int claimTemplateId)
        {

            var claimTemplateController = new ClaimTemplateController();
            var mytemplate = claimTemplateController.GetClaimTemplate(claimTemplateId);
            var currentUser = _userFactory.GetCurrentUser();

            var claim = new Claim
            {

                ClaimTemplateID = claimTemplateId,
                CreatedBy = User.Identity.Name,
                DateCreated = DateTime.Now,
                UserID = currentUser.UserId,
                ClaimStatusID = 1,
                WillisEmployeeID = 1
            };

            _db.Claims.Add(claim);

            string claimName =
                claim.CreatedBy + " - " +
                claim.ClaimTemplateID + " - " +
                ((claim.DateCreated != null) ? (claim.DateCreated.Value).ToShortDateString() : String.Empty);

            claim.Name = claimName.Length < 50 ? claimName : claimName.Substring(0, 50);
            _db.SaveChanges();
            var claimId = claim.ClaimID; // here yo


            foreach (var claimFieldGroupTemplate in mytemplate.ClaimFieldGroupTemplates)
            {
                var claimFieldGroup =
                      new ClaimFieldGroup
                      {
                          Name = claimFieldGroupTemplate.Name,
                          Description = claimFieldGroupTemplate.Description,
                          ItemOrder = claimFieldGroupTemplate.ItemOrder
                      };


                foreach (var claimFieldTemplate in claimFieldGroupTemplate.ClaimFieldTemplates)
                {
                    var claimField =
                        new ClaimField
                        {
                            Name = claimFieldTemplate.Name,
                            Code = claimFieldTemplate.Code,
                            ClaimFieldTemplateID = claimFieldTemplate.ClaimFieldTemplateID,
                            ClaimFieldGroupID = claimFieldGroupTemplate.ClaimFieldGroupTemplateID,
                            TemplateBName = claimFieldTemplate.FieldType.TemplateName
                        };

                    claimFieldGroup.ClaimFields.Add(claimField);

                }
                claim.ClaimFieldGroups.Add(claimFieldGroup);
            }
            _db.SaveChanges();

            _db.Dispose();

            return claimId;
        }

        public void CreateFieldsForClaimFromTemplate(Claim claim)
        {
            var mytemplate = new ClaimTemplate();

            if (claim.ClaimTemplateID != null)
                mytemplate = _claimTemplateFactory.GetClaimTemplate(claim.ClaimTemplateID.Value);

            foreach (var claimFieldGroupTemplate in mytemplate.ClaimFieldGroupTemplates)
            {
                var claimFieldGroup =
                      new ClaimFieldGroup
                      {
                          Name = claimFieldGroupTemplate.Name,
                          Description = claimFieldGroupTemplate.Description,
                          ItemOrder = claimFieldGroupTemplate.ItemOrder
                      };

                foreach (var claimFieldTemplate in claimFieldGroupTemplate.ClaimFieldTemplates)
                {
                    var claimField =
                        new ClaimField
                        {
                            Name = claimFieldTemplate.Name,
                            Code = claimFieldTemplate.Code,
                            ClaimFieldTemplateID = claimFieldTemplate.ClaimFieldTemplateID,
                            ClaimFieldGroupID = claimFieldGroupTemplate.ClaimFieldGroupTemplateID,
                            TemplateBName = claimFieldTemplate.FieldType.TemplateName
                        };

                    claimFieldGroup.ClaimFields.Add(claimField);

                }
                claim.ClaimFieldGroups.Add(claimFieldGroup);
            }
            _db.SaveChanges();
            _db.Dispose();
        }

        public int CreateFromTemplate(int claimTemplateId, FormCollection formCollection)
        {

            var mytemplate = _claimTemplateFactory.GetClaimTemplate(claimTemplateId);

            var currentUser = _userFactory.GetCurrentUser();

            var formDictionary =
                new Dictionary<string, string>();

            for (int i = 0; i < formCollection.Keys.Count; i++)
            {
                var key1 = formCollection.Keys[i];
                formDictionary.Add(key1, formCollection[key1]);
            }

            var claimId = _claimFactory.CreateFromTemplate(claimTemplateId, formDictionary, currentUser, mytemplate);
            return claimId;
        }


        public int UpdateFromTemplate(Claim claim, FormCollection formCollection)
        {

            var claimFields =
                _db.ClaimFields.Where(m => m.ClaimFieldGroup.ClaimID == claim.ClaimID);



            var claimId = claim.ClaimID; // here yo

            var fieldTypesList = _fieldTypeFactory.GetFieldTypes();

            foreach (var claimField in claimFields)
            {


                var value = formCollection[claimField.Code];

                if (value != null)
                {
                    var fieldType = fieldTypesList.Single(ft => ft.FieldTypeID == claimField.ClaimFieldTemplate.FieldTypeID);
                    switch (fieldType.Code)
                    {
                        case "ShortText":
                            claimField.ShortTextValue = value;
                            break;

                        case "LongText":
                            claimField.LongTextValue = value;
                            break;

                        case "Integer":
                            int tmpvalue;
                            claimField.IntegerValue = int.TryParse(value, out tmpvalue) ? tmpvalue : (int?)null;
                            break;

                        case "Float":
                            double tmpvalue2;
                            claimField.FloatValue = double.TryParse(value, out tmpvalue2) ? tmpvalue2 : (double?)null;
                            break;

                        case "Date":
                            DateTime tmpvalue3;
                            claimField.DateValue = DateTime.TryParse(value, out tmpvalue3) ? tmpvalue3 : (DateTime?)null;
                            break;

                        case "DateTime":
                            DateTime tmpvalue4;
                            claimField.DateTimeValue = DateTime.TryParse(value, out tmpvalue4) ? tmpvalue4 : (DateTime?)null;
                            break;

                        case "DropDown":
                            claimField.DropDownValue = value;
                            break;

                        case "MultiChoice":
                            claimField.MultiChoiceValue = value;
                            break;

                        case "File":
                            claimField.FileValue = null;
                            break;

                        case "Money":
                            decimal tmpvalue5;
                            claimField.CurrecncyValue = decimal.TryParse(value, out tmpvalue5) ? tmpvalue5 : (decimal?)null;
                            break;

                        case "Country":
                            claimField.CountryValue = value;
                            break;

                        case "Range":
                            double tmpvalue6;
                            claimField.RangeValue = double.TryParse(value, out tmpvalue6) ? tmpvalue6 : (double?)null;
                            break;
                    }


                }

            }


            _db.Entry(claim).State = EntityState.Modified;


            _db.SaveChanges();

            return claimId;
        }

        public void SubmitClaim(Claim newClaim)
        {
            newClaim.CreatedBy = User.Identity.Name;
            newClaim.DateCreated = DateTime.Now;

            _db.Claims.Add(newClaim);

            _db.SaveChanges();


            foreach (ClaimFieldGroup claimFieldGroup in newClaim.ClaimFieldGroups)
            {
                foreach (ClaimField claimField in claimFieldGroup.ClaimFields)
                {
                    claimField.ClaimFieldGroupID = claimFieldGroup.ClaimFieldGroupID;

                    _db.ClaimFields.Add(claimField);
                }
            }

            _db.SaveChanges();
        }

        public int GetClaimsCount()
        {
            return _db.Claims.Count();
        }

        public List<Claim> GetClaimsForWillisAssociate(string willisEmployeeUsername)
        {
            var ae = new ClaimsEntities();
            var willisEmployee =
                from we in ae.WillisEmployees
                where we.EmployeeUserID == willisEmployeeUsername
                select we;

            var userId = willisEmployee.First().WillisEmployeeID;

            var willisClaims =
                from claims in _db.Claims
                where claims.WillisEmployeeID == userId
                select claims;

            return willisClaims.ToList();
        }

        public ActionResult GetListofUnassignedClaims()
        {
            var unassignedClaimsList =
                from claims in _db.Claims
                where claims.WillisEmployeeID == null
                select claims;

            return View(unassignedClaimsList.ToList());
        }


        public ActionResult FilterClaimSources()
        {
            var claimSources = new List<string> { "CFT", "Acturis", "Eclipse"
               , "Eclipse Policy" 
            };
            return Json(claimSources, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FilterClaimTemplateNames()
        {
            var test =
                from cmt in _db.ClaimTemplates
                select cmt.Name;

            var claimTemplateNames = test.ToList();

            return Json(claimTemplateNames, JsonRequestBehavior.AllowGet);
        }

    }
}