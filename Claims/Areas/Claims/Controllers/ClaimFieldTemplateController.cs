using System.Web.Mvc;
using Factories;
using ModelsLayer;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class ClaimFieldTemplateController : Controller
    {
        private readonly IClaimFieldTemplateFactory _claimFieldTemplateFactory;


        //
        // GET: /Administration/ClaimFieldTemplate/
        public ClaimFieldTemplateController()
        {
        }

        public ClaimFieldTemplateController(IClaimFieldTemplateFactory claimFieldTemplateFactory)
        {
            _claimFieldTemplateFactory = claimFieldTemplateFactory;

        }
        public ActionResult Index()
        {
            var claimfieldtemplates = _claimFieldTemplateFactory.GetClaimFieldTemplates();
            return View(claimfieldtemplates);
        }

        //
        // GET: /Administration/ClaimFieldTemplate/Details/5

        public ActionResult Details(int id = 0)
        {
            var claimfieldtemplate = _claimFieldTemplateFactory.GetClaimFieldTemplate(id);
            return View(claimfieldtemplate);
        }

        //
        // GET: /Administration/ClaimFieldTemplate/Create

        public ActionResult Create(int claimTemplateId)
        {
            //ViewBag.FieldTypeIDList = new SelectList(db.FieldTypes.ToList<FieldType>(), "FieldTypeID", "Name");
            //ViewBag.CountryIDList = new SelectList(db.Countries, "CountryID", "Name");
            //ViewBag.CurrencyIDList = new SelectList(db.Currencies, "CurrencyID", "Name");

            //if (HttpContext.Request.QueryString["mode"] == "grid" ||
            //        HttpContext.Request.QueryString["mode"] == "fromTemplate")
            //{
            //    int claimTemplateID = Convert.ToInt32(HttpContext.Request.QueryString["ClaimTemplateID"]);
            //    ViewBag.ClaimTemplateID = new SelectList(db.ClaimTemplates, "ClaimTemplateID", "Name", claimTemplateID);
            //}
            //else
            //{
            //    ViewBag.ClaimTemplateID = new SelectList(db.ClaimTemplates, "ClaimTemplateID", "Name");
            //}



            return View();
        }

        //
        // POST: /Administration/ClaimFieldTemplate/Create

        [HttpPost]
        public ActionResult Create(ClaimFieldTemplate claimfieldtemplate, int claimTemplateId)
        {
            if (!ModelState.IsValid) return View(claimfieldtemplate);
            _claimFieldTemplateFactory.CreateClaimFieldTemplate(claimfieldtemplate);

            return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimTemplateId, area = "Claims" });
        }




        //
        // GET: /Administration/ClaimFieldTemplate/Edit/5

        public ActionResult Edit(int id = 0, int claimTemplateId = 0)
        {
            var claimfieldtemplate = _claimFieldTemplateFactory.GetClaimFieldTemplate(id);
            return View(claimfieldtemplate);
        }

        //
        // POST: /Administration/ClaimFieldTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(ClaimFieldTemplate claimfieldtemplate, int claimTemplateId = 0)
        {
            if (!ModelState.IsValid) return View(claimfieldtemplate);
            _claimFieldTemplateFactory.UpdateClaimFieldTemplate(claimfieldtemplate);

            return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimTemplateId, area = "Claims" }); 
        }

        //
        // GET: /Administration/ClaimFieldTemplate/Delete/5

        public ActionResult Delete(int id = 0, int claimTemplateId = 0)
        {
            var claimfieldtemplate = _claimFieldTemplateFactory.GetClaimFieldTemplate(id);
            return View(claimfieldtemplate);
        }

        //
        // POST: /Administration/ClaimFieldTemplate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, int claimTemplateId = 0)
        {
            var claimfieldtemplate = _claimFieldTemplateFactory.GetClaimFieldTemplate(id);
            _claimFieldTemplateFactory.DeleteClaimFieldTemplate(claimfieldtemplate);
            return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimTemplateId });
        }

        protected override void Dispose(bool disposing)
        {
            _claimFieldTemplateFactory.Dispose(disposing);
        }

        public ActionResult ListViewForClaimTemplate(int claimTemplateId = 0)
        {
            if (claimTemplateId <= 0) return View();

            ViewBag.ClaimTemplateID = claimTemplateId;

            if (Request.QueryString["Grid-mode"] == "insert")
            {
                return RedirectToAction("ClaimFieldTemplate_CreateFromGrid", new { ClaimTemplateID = claimTemplateId });
            }
            //if (Request.QueryString["Grid-mode"] == "edit")
            //{
            //    var claimFieldTemplateId = Convert.ToInt32(Request.QueryString["ClaimFieldTemplateID"]);
            //    return RedirectToAction("EditReturn", new { @Id = claimFieldTemplateId });
            //}

            var claimFieldTemplates = _claimFieldTemplateFactory.GetClaimFieldTemplates(claimTemplateId);

            return View(claimFieldTemplates);
        }





        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(ClaimFieldTemplate claimFieldTemplate)
        {
            _claimFieldTemplateFactory.UpdateClaimFieldTemplate(claimFieldTemplate);
            if (claimFieldTemplate.ClaimFieldGroupTemplate.ClaimTemplateID == null) return View("Index");
            return RedirectToAction("ListViewForClaimTemplate", new { ClaimTemplateID = claimFieldTemplate.ClaimFieldGroupTemplate.ClaimTemplateID.Value });

        }



        public ActionResult ClaimFieldTemplate_CreateFromGrid(int claimTemplateId)
        {
            //ViewBag.ClaimTemplateID = new SelectList(db.ClaimTemplates, "FieldTypeID", "Name", ClaimTemplateID);

            //ViewBag.FieldTypeID = new SelectList(db.FieldTypes.ToList<FieldType>(), "FieldTypeID", "Name");
            //ViewBag.ClaimTemplateID = new SelectList(db.ClaimTemplates, "ClaimTemplateID", "Name");

            return View();
        }

        //
        // POST: /Administration/ClaimFieldTemplate/Create

        [HttpPost]
        public ActionResult ClaimFieldTemplate_CreateFromGrid(ClaimFieldTemplate claimfieldtemplate)
        {
            //if (ModelState.IsValid)
            //{
            //    db.ClaimFieldTemplates.Add(claimfieldtemplate);
            //    db.SaveChanges();
            //    return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimfieldtemplate.ClaimFieldGroupTemplate.ClaimTemplateID });
            //}

            //ViewBag.FieldTypeID = new SelectList(db.FieldTypes.ToList<FieldType>(), "FieldTypeID", "Name", claimfieldtemplate.FieldTypeID);
            //ViewBag.ClaimTemplateID = new SelectList(db.ClaimTemplates, "ClaimTemplateID", "Name", claimfieldtemplate.ClaimFieldGroupTemplate.ClaimTemplateID);

            _claimFieldTemplateFactory.CreateClaimFieldTemplate(claimfieldtemplate);
            return RedirectToAction("Edit", "ClaimTemplate", new { @id = ViewBag.ClaimTemplateID });
        }


        public ClaimFieldTemplate GetClaimFieldTemplate(int claimFieldTemplateId)
        {
            return _claimFieldTemplateFactory.GetClaimFieldTemplate(claimFieldTemplateId);
        }




        /// Sction to create inline

        /// <summary>
        /// Section
        /// </summary>
        /// <param name="claimTemplateId"></param>
        /// <returns></returns>

        public ActionResult ClaimFieldTemplate_EditFromGrid(int claimTemplateId)
        {
            ViewBag.ClaimTemplateID = claimTemplateId;
            if (HttpContext.Request.QueryString["Grid-mode"] == "insert")
            {
                return RedirectToAction("Create", "ClaimFieldTemplate", new { ClaimTemplateID = claimTemplateId, mode = "grid" });

            }


            var claimFieldsTemplates =
                _claimFieldTemplateFactory.GetClaimFieldTemplates(claimTemplateId);

            return View(claimFieldsTemplates);

        }


        [HttpPost]
        public ActionResult ClaimFieldTemplate_EditFromGrid(ClaimFieldTemplate claimfieldtemplate)
        {
            //if (ModelState.IsValid)
            //{
            //    db.ClaimFieldTemplates.Add(claimfieldtemplate);
            //    db.SaveChanges();
            //    return RedirectToAction("Edit", "ClaimTemplate", new { @id = claimfieldtemplate.ClaimFieldGroupTemplate.ClaimTemplateID, mode = "grid" });
            //}

            //ViewBag.FieldTypeID = new SelectList(db.FieldTypes.ToList<FieldType>(), "FieldTypeID", "Name", claimfieldtemplate.FieldTypeID);
            //ViewBag.ClaimTemplateID = new SelectList(db.ClaimTemplates, "ClaimTemplateID", "Name", claimfieldtemplate.ClaimFieldGroupTemplate.ClaimTemplateID);

            _claimFieldTemplateFactory.UpdateClaimFieldTemplate(claimfieldtemplate);
            return RedirectToAction("Edit", "ClaimTemplate", new { @id = ViewBag.ClaimTemplateID });
        }

    }
}