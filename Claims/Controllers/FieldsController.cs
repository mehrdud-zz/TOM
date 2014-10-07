using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimsPoC.Controllers
{
    public enum FieldMode { View, Insert, Edit };
    public class FieldTemplate {
        public string FieldMode { get; set; }
        public ClaimFieldTemplate ClaimFieldTemplate { get; set; }

    }
    public class FieldsController : Controller
    {
        //
        // GET: /Fields/
        public ActionResult MultiSelectTemplate(ClaimFieldTemplate claimField, FieldMode FieldMode)
        {
            ViewBag.FieldMode = FieldMode;
            var claimFieldTemplateController = new Claims.Controllers.ClaimFieldTemplateController();
            ClaimFieldTemplate claimFieldTemplate = claimFieldTemplateController.GetClaimFieldTemplate((int)claimField.ClaimFieldTemplateID);
            if (claimFieldTemplate.MultiChoiceDefaultValue != null)
            {
                string[] multiChoiceDefaultValues = claimFieldTemplate.MultiChoiceDefaultValue.Split('\n');

                List<SelectListItem> list = new List<SelectListItem>();

                foreach (string choice in multiChoiceDefaultValues)
                {
                    SelectListItem selectListItem = new SelectListItem() { Text = choice, Value = choice };

                    list.Add(selectListItem);

                }

                ViewBag.MultiChoiceList = list;
                ViewBag.claimField = claimField;

            }
            return View();
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


        public ActionResult DropDownListTemplate(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField)
        {
            Claims.Controllers.ClaimFieldTemplateController claimFieldTemplateController = new Claims.Controllers.ClaimFieldTemplateController();
            List<SelectListItem> list = new List<SelectListItem>();
            ViewBag.claimField = claimField;

            if (claimField != null || claimFieldTemplate != null)
            {
                if (claimField.ClaimFieldID > 0)
                    claimFieldTemplate = claimFieldTemplateController.GetClaimFieldTemplate((int)claimField.ClaimFieldTemplateID);

                list.Add(new SelectListItem() { Text = " ", Value = "" });

                if (claimFieldTemplate.DropDownDefaultValue != null)
                {
                    string[] multiChoiceDefaultValues = claimFieldTemplate.DropDownDefaultValue.Split('\n');



                    foreach (string choice in multiChoiceDefaultValues)
                    {
                        SelectListItem selectListItem = new SelectListItem() { Text = choice, Value = choice };

                        list.Add(selectListItem);

                    }


                }



            }
            ViewBag.claimFieldCode = claimFieldTemplate.Code;
            ViewBag.claimFieldDropDownValue = claimFieldTemplate.DropDownDefaultValue;
            ViewBag.DropDownList = list;
            return View();
        }


        public ActionResult MultiChoiceListTemplate(ClaimFieldTemplate claimFieldTemplate, ClaimField claimField)
        {
            Claims.Controllers.ClaimFieldTemplateController claimFieldTemplateController = new Claims.Controllers.ClaimFieldTemplateController();

            List<SelectListItem> list = new List<SelectListItem>();

            if (claimField != null || claimFieldTemplate != null)
            {
                if (claimField.ClaimFieldID > 0)
                    claimFieldTemplate = claimFieldTemplateController.GetClaimFieldTemplate((int)claimField.ClaimFieldTemplateID);

                if (claimFieldTemplate.MultiChoiceDefaultValue != null)
                {
                    string[] multiChoiceDefaultValues = claimFieldTemplate.MultiChoiceDefaultValue.Split('\n');



                    foreach (string choice in multiChoiceDefaultValues)
                    {
                        SelectListItem selectListItem = new SelectListItem() { Text = choice, Value = choice };

                        list.Add(selectListItem);

                    }


                }

            }

            ViewBag.claimFieldMultiChoiceValue = claimFieldTemplate.MultiChoiceDefaultValue;
            ViewBag.claimFieldCode = claimFieldTemplate.Code;
            ViewBag.MultiChoiceList = list;
            return View();
        }

    }
}
