using System.Globalization;
using Factories;
using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace ClaimsPoC.Claims.Controllers
// ReSharper restore CheckNamespace
{
    public class SubDropDownItem
    {
        public string Level1;
        public string Level2;
    }

    public class MultiSelectMehrdad
    {
        public string Text;
        public string Value;
        public bool Selected = false;
    }


    public class CustomTemplateController : Controller
    {
        //
        // GET: /Claims/CustomTemplate/



        private readonly IClaimFactory _claimFactory;
        private readonly IClaimStatusFactory _claimStatusFactory;
        private readonly IClaimTemplateFactory _claimTemplateFactory;
        private readonly ICountryFactory _countryFactory;
        private readonly IClaimFieldTemplateFactory _claimFieldTemplateFactory;
        private readonly Acturis.Factories.IActurisClaimFactory _acturisClaimFactory;
        private readonly  Eclipse.Factories.IEclipseClaimFactory _eclipseClaimFactory;
        private readonly Eclipse.Factories.IEclipsePolicyFactory _eclipsePolicyFactory;


        //public CustomTemplateController()
        //{
        //    this.userFactory = new UserFactory();
        //    this.claimFactory = new ClaimFactory();
        //    this.claimStatusFactory = new ClaimStatusFactory();
        //    this.claimTemplateFactory = new ClaimTemplateFactory();
        //    this.fieldTypeFactory = new FieldTypeFactory();
        //}

        public CustomTemplateController(IClaimFactory claimFactory2, IClaimStatusFactory claimStatusFactory, IClaimTemplateFactory claimTemplateFactory
            , ICountryFactory countryFactory, IClaimFieldTemplateFactory claimFieldTemplateFactory, Acturis.Factories.IActurisClaimFactory acturisClaimFactory,
             Eclipse.Factories.IEclipseClaimFactory eclipseClaimFactory, Eclipse.Factories.IEclipsePolicyFactory eclipsePolicyFactory)
        {
            _claimFactory = claimFactory2;
            _claimStatusFactory = claimStatusFactory;
            _claimTemplateFactory = claimTemplateFactory;
            _countryFactory = countryFactory;
            _claimFieldTemplateFactory = claimFieldTemplateFactory;
            _acturisClaimFactory = acturisClaimFactory;
            _eclipseClaimFactory = eclipseClaimFactory;
            _eclipsePolicyFactory = eclipsePolicyFactory;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FieldsListCreateFromClaimTemplate(int claimTemplateId = 0)
        {
            ClaimTemplate claimTemplate = null;
            if (claimTemplateId > 0)
            {
                claimTemplate = _claimTemplateFactory.GetClaimTemplate(claimTemplateId);
            }
            return View(claimTemplate);
        }

        public ActionResult FieldsListEditExistingClaim(int id = 0)
        {
            Claim claim = _claimFactory.GetClaim(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClaimStatusID = new SelectList(_claimStatusFactory.GetClaimStatus(), "ClaimStatusID", "Name", claim.ClaimStatusID);



            return View(claim);
        }


        public ActionResult FieldsListViewExistingClaim(int id = 0, String source = Constants.CFT_SOURCE)
        {
            switch (source)
            {
                case Constants.ACTURIS_SOURCE:
                    var claim1 = _acturisClaimFactory.GetClaim(id);
                    return View(claim1);

                case Constants.ECLIPSE_SOURCE:
                    var claim2 = _eclipseClaimFactory.GetClaim(id);
                    return View(claim2);


                case Constants.ECLIPSE_POLICY_SOURCE:
                    var claim4 = _eclipsePolicyFactory.GetPolicy(id);
                    return View(claim4);
                default:
                    var claim3 = _claimFactory.GetClaim(id);
                    if (claim3 == null)
                    {
                        return HttpNotFound();
                    }
                  //  ViewBag.ClaimStatusID = new SelectList(_claimStatusFactory.GetClaimStatus(), "ClaimStatusID", "Name", claim3.ClaimStatusID);
                    return View(claim3);
                    break;
            }            
        }


        public ActionResult MultiSelectTemplate(ClaimFieldTemplate claimField, ClaimsPoC.Controllers.FieldMode fieldMode)
        {
            ViewBag.FieldMode = fieldMode;

            var claimFieldTemplate = _claimFieldTemplateFactory.GetClaimFieldTemplate(claimField.ClaimFieldTemplateID);
            if (claimFieldTemplate.MultiChoiceDefaultValue != null)
            {
                string[] multiChoiceDefaultValues = claimFieldTemplate.MultiChoiceDefaultValue.Split('\n');

                var list = multiChoiceDefaultValues.Select(choice => new SelectListItem { Text = choice, Value = choice }).ToList();

                ViewBag.MultiChoiceList = list;
                ViewBag.claimField = claimField;

            }
            return View();
        }

        public ActionResult DropDownListTemplate(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField)
        {
            var dropDownTemplateValues = new List<string>();
            string selectedValue = String.Empty;

            if (claimFieldTemplate.ClaimFieldTemplateID != 0)
            {
                dropDownTemplateValues = claimFieldTemplate.DropDownDefaultValue.Split('\r').ToList();
                ViewBag.claimFieldCode = claimFieldTemplate.Code;
            }
            else if (claimField.ClaimFieldID != 0)
            {
                dropDownTemplateValues = claimField.ClaimFieldTemplate.DropDownDefaultValue.Split('\r').ToList();
                selectedValue = claimField.DropDownValue;
                ViewBag.claimFieldCode = claimField.Code;
            }


            var dropDownItemList = dropDownTemplateValues.Select(dropDownTemplateValue => dropDownTemplateValue.Trim()).Select(temp => new Kendo.Mvc.UI.DropDownListItem
            {
                Text = temp,
                Value = temp,
                Selected = (selectedValue == temp)
            }).ToList();
            return View(dropDownItemList);
        }

        public ActionResult MultiChoiceListTemplate2(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField, ClaimsPoC.Controllers.FieldMode fieldMode)
        {
            var list = new List<Kendo.Mvc.UI.DropDownListItem>();

            if (claimField != null || claimFieldTemplate != null)
            {
                if (claimField != null && claimField.ClaimFieldID > 0)
                    if (claimField.ClaimFieldTemplateID != null)
                        claimFieldTemplate = _claimFieldTemplateFactory.GetClaimFieldTemplate((int)claimField.ClaimFieldTemplateID);

                if (claimFieldTemplate.MultiChoiceDefaultValue != null)
                {
                    string[] multiChoiceDefaultValues = claimFieldTemplate.MultiChoiceDefaultValue.Split('\n');



                    foreach (string choice in multiChoiceDefaultValues)
                    {
                        var selectListItem = new Kendo.Mvc.UI.DropDownListItem { Text = choice, Value = choice };

                        if (claimField != null && (fieldMode == ClaimsPoC.Controllers.FieldMode.Edit && claimField.MultiChoiceValue.Contains(choice)))
                        {
                            selectListItem.Selected = true;
                        }
                        else
                            selectListItem.Selected = false;

                        list.Add(selectListItem);

                    }


                }
            }
            ViewBag.FieldMode = fieldMode;
            if (claimField != null) ViewBag.claimFieldCode = claimField.Code;
            return View(list);
        }


        public ActionResult ServerFiltering()
        {
            return View();
        }

        public ActionResult MultiChoiceListTemplate(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField)
        {

            var charsToTrim = new[] { ',', ' ' };


            if (claimField != null && claimField.ClaimFieldTemplate != null && !String.IsNullOrEmpty(claimField.ClaimFieldTemplate.MultiChoiceDefaultValue))
            {
                var multiChoiceDefaultValue = claimField.ClaimFieldTemplate.MultiChoiceDefaultValue.Split('\n');
                for (int i = 0; i < multiChoiceDefaultValue.Length; i++)
                {
                    multiChoiceDefaultValue[i] = multiChoiceDefaultValue[i].TrimEnd(charsToTrim);
                }
                ViewBag.MultiChoiceDefaultValue = multiChoiceDefaultValue.ToList();
            }
            else
            {
                ViewBag.MultiChoiceDefaultValue = new List<string>();
            }

            if (claimField != null && !String.IsNullOrEmpty(claimField.MultiChoiceValue))
            {
                string[] strSelectedVals = claimField.MultiChoiceValue.Split('\n');
                for (int i = 0; i < strSelectedVals.Length; i++)
                {
                    strSelectedVals[i] = strSelectedVals[i].TrimEnd(charsToTrim);
                    strSelectedVals[i] = strSelectedVals[i].TrimStart(new[] { ',', ' ', '\r', '\n' });
                }

                ViewBag.SelectedValues = strSelectedVals;
            }
            else
                ViewBag.SelectedValues = new List<string>();

            if (claimField != null && !String.IsNullOrEmpty(claimField.Code))
                ViewBag.claimFieldCode = claimField.Code;
            else if (claimFieldTemplate != null && !String.IsNullOrEmpty(claimFieldTemplate.Code))
                ViewBag.claimFieldCode = claimFieldTemplate.Code;

            ViewBag.claimField = claimField;
            return View();
        }

        public ActionResult CountryTemplate(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField)
        {
            var countries = _countryFactory.GetCountries();
            var list = new List<SelectListItem> { new SelectListItem { Text = " ", Value = "" } };
            list.AddRange(countries.Select(country => new SelectListItem { Text = country.Name, Value = country.CountryID.ToString(CultureInfo.InvariantCulture) }));


            ViewBag.CountryList = list;
            if (claimField != null && claimField.Code != null)
                ViewBag.Code = claimField.Code;

            else if (claimFieldTemplate != null && claimFieldTemplate.Code != null)
                ViewBag.Code = claimFieldTemplate.Code;

            return View(countries);
        }

        public ActionResult CountryTemplateEdit(ClaimField claimField)
        {
            var countries = _countryFactory.GetCountries();
            var list = new List<SelectListItem> { new SelectListItem { Text = " ", Value = "" } };
            list.AddRange(countries.Select(country => new SelectListItem { Text = country.Name, Value = country.CountryID.ToString(CultureInfo.InvariantCulture) }));

            ViewBag.claimField = claimField;
            ViewBag.CountryList = list;
            ViewBag.Code = claimField.Code;
            return View();
        }



        public ActionResult SubDropDown(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField)
        {
            var json = "";

            if (claimFieldTemplate != null && claimFieldTemplate.ClaimFieldTemplateID != 0)
            {
                json = claimFieldTemplate.DropDownLevel2Default;
                ViewBag.claimFieldCode = claimFieldTemplate.Code;
            }
            else if (claimField != null && claimField.ClaimFieldID != 0)
            {
                json = claimField.ClaimFieldTemplate.DropDownLevel2Default;
                ViewBag.claimFieldCode = claimField.Code;
            }

            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var persons = js.Deserialize<List<SubDropDownItem>>(json);
            ViewBag.Json = json;

            //foreach (string dropDownTemplateValue in dropDownTemplateValues)
            //{
            //    string temp = dropDownTemplateValue.Trim();

            //    dropDownItemList.Add(
            //        new Kendo.Mvc.UI.DropDownListItem()
            //        {
            //            Text = temp,
            //            Value = temp,
            //            Selected = (selectedValue == temp) ? true : false
            //        });
            //}
            return View(persons);
        }
    }
}
