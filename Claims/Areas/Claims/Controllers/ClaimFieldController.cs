using System;
using System.Linq;
using System.Web.Mvc;
using ModelsLayer;
using Factories;

// ReSharper disable once CheckNamespace
namespace ClaimsPoC.Claims.Controllers 
{

    public class DropDownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }

    public class ClaimFieldController : Controller
    {
        private readonly IClaimFieldFactory _claimFieldFactory;
        private readonly IClaimFactory _claimFactory;
        private readonly IFieldTypeFactory _fieldTypeFactory;

        public ClaimFieldController(IClaimFieldFactory claimFieldFactory, IClaimFactory claimFactory, IFieldTypeFactory fieldTypeFactory)
        {
            _claimFieldFactory = claimFieldFactory;
            _claimFactory = claimFactory;
            _fieldTypeFactory = fieldTypeFactory;
        }

        

        //
        // GET: /Claims/ClaimField/

        public ActionResult Index()
        {
            var claimfields = _claimFieldFactory.GetClaimFields();
            return View(claimfields);
        }

        //
        // GET: /Claims/ClaimField/Details/5

        public ActionResult Details(int id = 0)
        {
            return View(_claimFieldFactory.GetClaimField(id));
        }

        //
        // GET: /Claims/ClaimField/Create

        public ActionResult Create()
        {
            ViewBag.ClaimID = new SelectList(_claimFactory.GetClaims(), "ClaimID", "CreatedBy");
            ViewBag.FieldTypeID = new SelectList(_fieldTypeFactory.GetFieldTypes(), "FieldTypeID", "Name");
            return View();
        }

        //
        // POST: /Claims/ClaimField/Create

        [HttpPost]
        public ActionResult Create(ClaimField claimField)
        {
            _claimFieldFactory.CreateClaimField(claimField);
            ViewBag.ClaimID = new SelectList(_claimFactory.GetClaims(), "ClaimID", "CreatedBy", claimField.ClaimFieldGroup.ClaimID);
            ViewBag.FieldTypeID = new SelectList(_fieldTypeFactory.GetFieldTypes(), "FieldTypeID", "Name", claimField.ClaimFieldTemplate.FieldTypeID);
            return View(claimField);
        }

        //
        // GET: /Claims/ClaimField/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var claimField = _claimFieldFactory.GetClaimField(id);
            ViewBag.ClaimID = new SelectList(_claimFactory.GetClaims(), "ClaimID", "CreatedBy", claimField.ClaimFieldGroup.ClaimID);
            ViewBag.FieldTypeID = new SelectList(_fieldTypeFactory.GetFieldTypes(), "FieldTypeID", "Name", claimField.ClaimFieldTemplate.FieldTypeID);
            return View(claimField);
        }

        //
        // POST: /Claims/ClaimField/Edit/5

        [HttpPost]
        public ActionResult Edit(ClaimField claimField)
        {
            _claimFieldFactory.UpdateClaimField(claimField);
            ViewBag.ClaimID = new SelectList(_claimFactory.GetClaims(), "ClaimID", "CreatedBy", claimField.ClaimFieldGroup.ClaimID);
            ViewBag.FieldTypeID = new SelectList(_fieldTypeFactory.GetFieldTypes(), "FieldTypeID", "Name", claimField.ClaimFieldTemplate.FieldTypeID);
            return View(claimField);
        }

        //
        // GET: /Claims/ClaimField/Delete/5

        public ActionResult Delete(int id = 0)
        {
            return View(_claimFieldFactory.GetClaimField((id)));
        }

        //
        // POST: /Claims/ClaimField/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var claimField = _claimFieldFactory.GetClaimField(id);
            var deleteClaimField = _claimFieldFactory.DeleteClaimField(claimField);

            if (deleteClaimField)
                return RedirectToAction("Index");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _claimFieldFactory.Dispose(disposing);
        }



        public void SetValue(ClaimField claimField, ClaimFieldTemplate claimFieldTemplate)
        {
            var fieldTypesList = _fieldTypeFactory.GetFieldTypes();

            if (claimFieldTemplate != null && claimFieldTemplate.FieldTypeID != null)
            {
                var fieldType = fieldTypesList.Single(ft => ft.FieldTypeID == claimFieldTemplate.FieldTypeID);
                switch (fieldType.Code)
                {
                    case "ShortText":
                        claimField.ShortTextValue = (claimFieldTemplate.ShortTextDefaultValue ?? String.Empty);
                        break;

                    case "LongText":
                        claimField.LongTextValue = (claimFieldTemplate.LongTextDefaultValue ?? String.Empty);
                        break;

                    case "Integer":
                        claimField.IntegerValue = claimFieldTemplate.IntegerDefaultValue;
                        break;

                    case "Float":
                        claimField.FloatValue = claimFieldTemplate.FloatDefaultValue;
                        break;

                    case "Date":
                        claimField.DateValue = claimFieldTemplate.DateDefaultValue;
                        break;

                    case "DateTime":
                        claimField.DateTimeValue = claimFieldTemplate.DateTimeDefaultValue;
                        break;

                    case "DropDown":
                        claimField.DropDownValue = claimFieldTemplate.DropDownDefaultValue;
                        break;

                    case "MultiChoice":
                        claimField.MultiChoiceValue = claimFieldTemplate.MultiChoiceDefaultValue;
                        break;

                    case "File":
                        claimField.FileValue = null;
                        break;

                    case "Money":
                        claimField.CurrecncyValue = claimFieldTemplate.CurrecncyDefaultValue;
                        break;

                    case "Country":
                        claimField.CountryValue = claimFieldTemplate.CountryDefaultValue;
                        break;

                    case "Range":
                        claimField.RangeValue = claimFieldTemplate.RangeDefaultValue;
                        break; 
                }


            }

        }

        public void SetValue(ClaimField claimField, object value)
        {
            var fieldTypesList = _fieldTypeFactory.GetFieldTypes();

            if (value != null && claimField.ClaimFieldTemplate.FieldTypeID != null)
            {
                var fieldType = fieldTypesList.Single(ft => ft.FieldTypeID == claimField.ClaimFieldTemplate.FieldTypeID);
                switch (fieldType.Code)
                {
                    case "ShortText":
                        claimField.ShortTextValue = (value.ToString());
                        break;

                    case "LongText":
                        claimField.LongTextValue = (value.ToString());
                        break;

                    case "Integer":
                        int tmpvalue;
                        claimField.IntegerValue = int.TryParse((string)value, out tmpvalue) ? tmpvalue : (int?)null;
                        break;

                    case "Float":
                        double tmpvalue2;
                        claimField.FloatValue = double.TryParse((string)value, out tmpvalue2) ? tmpvalue2 : (double?)null;
                        break;

                    case "Date":
                        DateTime tmpvalue3;
                        claimField.DateValue = DateTime.TryParse((string)value, out tmpvalue3) ? tmpvalue3 : (DateTime?)null;
                        break;

                    case "DateTime":
                        DateTime tmpvalue4;
                        claimField.DateTimeValue = DateTime.TryParse((string)value, out tmpvalue4) ? tmpvalue4 : (DateTime?)null;
                        break;

                    case "DropDown":
                        claimField.DropDownValue = (value.ToString());
                        break;

                    case "MultiChoice":
                        claimField.MultiChoiceValue = (value.ToString());
                        break;

                    case "File":
                        claimField.FileValue = null;
                        break;

                    case "Money":
                        decimal tmpvalue5;
                        claimField.CurrecncyValue = decimal.TryParse((string)value, out tmpvalue5) ? tmpvalue5 : (decimal?)null;
                        break;

                    case "Country":
                        claimField.CountryValue = (value.ToString());
                        break;

                    case "Range":
                        double tmpvalue6;
                        claimField.RangeValue = double.TryParse((string)value, out tmpvalue6) ? tmpvalue6 : (double?)null;
                        break;
                         
                }
                 
            }

        }





    }
}