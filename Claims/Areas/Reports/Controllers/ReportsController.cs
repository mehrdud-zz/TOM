using System.Globalization;
using Factories;
using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;

namespace ClaimsPoC.Areas.Reports.Controllers
{
    public class GoogleDataTableColumn
    {
        public string Id;
        public string Label;
        public string Type;
    }

    public class GoogleDataTableRow
    {
        public GoogleDataRecord[] GoogleDataRecords;
    }

    public class GoogleDataRecord
    {
        public string Key;
        public string StringValue;
        public DateTime DateTimeValue;
        public double IntegerValue;
    }

    public class Filter
    {
        public string Name;
        public int FieldId; 
    }
    public class FunctionType
    {
        public string Function;
        public string FunctionName;
    }


    public class ReportFieldType
    {
        public String TypeName;
        public String TypeCode;
    }


    public class ReportClaimTemplateSelectItem : SelectListItem
    {
        public List<SelectListItem> ReportItemSelectListItemList;
    }


    public class CftReport
    {
        public int ReportId;
        public string FrameId;
        public string ChartType;
        public string Name;
        public string Description;
        public string HAxis;
        public string HAxisColour;
        public string VAxis;
        public string VAxisColour;
        public string ZAxis;
        public string ZAxisColour;
        public double Left;
        public double Top;
        public double Width;
        public double Height;
        public Filter[] Filters;
        public string[] Colours;
        public GoogleDataTableColumn[] GoogleDataTableColumns;
        public GoogleDataTableRow[] GoogleDataTableRows;
        public BrokingPlatformIntegrationBase.Interfaces.ClientElement[] ClientList;
    }

    public class ReportModel
    {
        public String ReportJson;
        public List<ReportRow> ReportRowList;
        public List<ReportRecord> ReportStructure;
        public String ReportId;
        public String ChartType;
        public String Name;
        public string[] Options;


        public GoogleDataTableColumn[] GoogleDataTableColumns;
        public GoogleDataTableRow[] GoogleDataTableRows;

        public ReportModel()
        {
            ReportRowList = new List<ReportRow>();
            ReportStructure = new List<ReportRecord>();
            Options = new string[] { };
        }
    }
    public class ReportRow
    {
        public List<ReportRecord> ReportRecordList;
    }
    public class ReportRecord
    {
        public String Name;
        public String Value;
        public String Type;
        public String ColumnHeader;
    }

    public class ClaimTemplatesSelector
    {
        public int NumberofRecords;
        public String OrderDirection;
        public List<ReportClaimTemplateSelectItem> ReportClaimTemplateSelectItemList;
    }
    public class ReportsController : Controller
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();
        private readonly IClaimTemplateFactory _claimTemplateFactory;
        private readonly IClaimFieldTemplateFactory _claimFieldTemplateFactory;
        private readonly IReportFactory _reportFactory;
        private readonly IReportFieldFactory _reportFieldFactory;
        private readonly Acturis.Factories.IActurisClaimFactory _acturisFactory;
        private readonly List<ListElement> _functionTypes;
        private readonly List<ListElement> _sortDirection;

        public ReportsController(IClaimTemplateFactory claimTemplateFactory, IClaimFieldTemplateFactory claimFieldTemplateFactory, IReportFactory reportFactory,
            IReportFieldFactory reportFieldFactory,
            Acturis.Factories.IActurisClaimFactory acturisFactory)
        {
            _claimTemplateFactory = claimTemplateFactory;
            _claimFieldTemplateFactory = claimFieldTemplateFactory;
            _reportFactory = reportFactory;

            _reportFieldFactory = reportFieldFactory;
            _acturisFactory = acturisFactory;


            _functionTypes = new List<ListElement>
            {
                new ListElement {Name = "Value of", Value = "FIELDVALUE"},
                new ListElement {Name = "Sum", Value = "SUM"},
                new ListElement {Name = "Number of", Value = "Count"},
                new ListElement {Name = "Average", Value = "AVG"},
                new ListElement {Name = "Maximum", Value = "MAX"},
                new ListElement {Name = "Minimum", Value = "MIN"},
                new ListElement {Name = "Median", Value = "MED"}
            };

            _sortDirection = new List<ListElement>
            {
                new ListElement {Name = "ASC", Value = "Ascending"},
                new ListElement {Name = "DESC", Value = "Descending"}
            };
        }

        public ClaimsEntities Db
        {
            get { return _db; }
        }

        //
        // GET: /Reports/Reports/

        public ActionResult Index()
        {
            List<ReportTemplate> reports = _reportFactory.GetReportTemplates();
            return View(reports);
        }



        public ActionResult Detail(int ReportID = 0)
        {
            var report = _reportFactory.GetReportTemplate(ReportID);
            return View(report);
        }

        public ActionResult Edit(int id = 0)
        {
            if (id <= 0) return View();
            ReportTemplate reportTemplate = _reportFactory.GetReportTemplate(id);
            ViewBag.Functions = _functionTypes;
            ViewBag.SortDirection = _sortDirection;
            ViewBag.ReportTemplate = reportTemplate;
            ViewBag.PageElement =
                new PageElement
                {
                    FrameID = "f1",
                    Height = 480,
                    Width = 640,
                    ElementTop = 500,
                    ElementLeft = 400,
                    ReportID = reportTemplate.ReportID,
                    ReportTemplate = reportTemplate,
                    ReportType = reportTemplate.ReportType
                };
            return View(reportTemplate);
        }


        [HttpPost]
        public ActionResult Edit(ReportTemplate reportTemplate)
        {
            var updated = _reportFactory.UpdateReportTemplate(reportTemplate);
            if (updated)
                return RedirectToAction("Edit", "Reports", new { area = "Reports", id = reportTemplate.ReportID, saved = "success" });
            return View();
        }

        //[HttpPost]
        //public ActionResult Edit(FormCollection formCollection)
        //{
        //    Dictionary<string, string> formDictionary =
        //       new Dictionary<string, string>();

        //    for (int i = 0; i < formCollection.Keys.Count; i++)
        //    {
        //        string key1 = formCollection.Keys[i];
        //        formDictionary.Add(key1, formCollection[key1]);
        //    }

        //    int reportID = reportFactory.UpdateReportTemplate(formDictionary);

        //    return RedirectToAction("RunReport", "Reports", new { area = "Reports", ReportID = reportID });
        //}


        public ActionResult Delete(int id = 0)
        {
            var report = _reportFactory.GetReportTemplate(id);
            return View(report);
        }

        //
        // POST: /Dashboard/Dashboard/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var report = _reportFactory.GetReportTemplate(id);
            _reportFactory.DeleteReport(report);
            return RedirectToAction("Index");
        }

        public CftReport GetGoogleReport(ReportTemplate reportTemplate, DataTable reportDataTable)
        {
            var counter = 0;
            var reportModel = new CftReport { GoogleDataTableColumns = new GoogleDataTableColumn[] { } };
            var reportFieldsCount = reportTemplate.ReportFields.Count();
            var reportRowCount = reportDataTable.Rows.Count;

            Array.Resize(ref reportModel.GoogleDataTableColumns, reportFieldsCount);


            var reportFields = reportTemplate.ReportFields.OrderBy(m => m.FieldNumber);
            foreach (var reportField in reportFields)
            {
                var headerType = reportField.ClaimFieldTemplate.FieldType.GoogleColumnType;

                if (reportField.Func == "Count" || reportField.Func == "Sum")
                    headerType = "number";

                if (reportField.FieldId != null)
                {
                    var googleDataTableColumn = new GoogleDataTableColumn
                    {
                        Id = "reportField" + reportField.FieldId.Value,
                        Label = reportField.DisplayName,
                        Type = headerType
                    };
                    reportModel.GoogleDataTableColumns[counter] = googleDataTableColumn;
                }
                counter++;
            }


            Array.Resize(ref reportModel.GoogleDataTableRows, reportRowCount);
            var rowCounter = 0;
            foreach (DataRow row in reportDataTable.Rows)
            {
                var googleDataTableRow = new GoogleDataTableRow();
                Array.Resize(ref googleDataTableRow.GoogleDataRecords, reportFieldsCount);
                int columnCounter = 0;
                foreach (var column in reportModel.GoogleDataTableColumns)
                {
                    var googleDataRecord = new GoogleDataRecord();
                    switch (column.Type)
                    {
                        case "string":
                            var o = row[column.Label];
                            if (o != null) googleDataRecord.StringValue = o.ToString();
                            break;

                        case "number":
                            googleDataRecord.IntegerValue =
                                ((row[column.Label] != null) && (!String.IsNullOrEmpty(row[column.Label].ToString()))) ?
                                    Convert.ToInt32(row[column.Label]) : 0;
                            break;

                        case "date":
                        case "datetime":
                            googleDataRecord.DateTimeValue = ((row[column.Label] != null) ? Convert.ToDateTime(row[column.Label]) : new DateTime());

                            break;
                    }
                    googleDataTableRow.GoogleDataRecords[columnCounter] = googleDataRecord;
                    columnCounter++;
                }
                reportModel.GoogleDataTableRows[rowCounter] = googleDataTableRow;
                rowCounter++;
            }

            return reportModel;
        }

        public CftReport GetCftReport(int reportId = 0, int clientId = 0, bool withData = true)
        {
            var cftReport = new CftReport();
            if (reportId > 0)
            {
                var reportTemplate = _reportFactory.GetReportTemplate(reportId);
                if (withData)
                {
                    string source;
                    var reportDataTable = _reportFactory.ExecuteReportQuery(reportTemplate, out source, clientId);



                    cftReport = GetGoogleReport(reportTemplate, reportDataTable);


                    cftReport.Filters = (from reportField in reportTemplate.ReportFields where reportField.Filter != null && (bool)reportField.Filter select new Filter() { FieldId = reportField.FieldId.Value, Name = reportField.DisplayName }).ToArray();


                    switch (source)
                    {
                        case "Acturis":
                            cftReport.ClientList = _acturisFactory.GetClientList().ToArray();
                            break;
                        default:
                        case "CFT":

                            cftReport.ClientList = _reportFactory.GetClientList().ToArray();
                            break;
                    }
                }

                Array.Resize(ref cftReport.Colours, 5);
                cftReport.Name = reportTemplate.Name;
                cftReport.ReportId = reportTemplate.ReportID;
                cftReport.ChartType = reportTemplate.ReportType;



                cftReport.Colours[0] = reportTemplate.Colour1 ?? "#ffffff";
                cftReport.Colours[1] = reportTemplate.Colour2 ?? "#ffffff";
                cftReport.Colours[2] = reportTemplate.Colour3 ?? "#ffffff";
                cftReport.Colours[3] = reportTemplate.Colour4 ?? "#ffffff";
                cftReport.Colours[4] = reportTemplate.Colour5 ?? "#ffffff";
                cftReport.HAxis = reportTemplate.HAxis ?? "";
                cftReport.HAxisColour = reportTemplate.HAxisColour ?? "";

                cftReport.VAxis = reportTemplate.VAxis ?? "";
                cftReport.VAxisColour = reportTemplate.VAxisColour ?? "";

                cftReport.ZAxis = reportTemplate.ZAxis ?? "";
                cftReport.ZAxisColour = reportTemplate.ZAxisColour ?? "";
                cftReport.Description = reportTemplate.Description;
            }

            return cftReport;
        }

        public JsonResult GetCftReportJson(int reportId = 0, int clientId = 0)
        {
            var cftReport = GetCftReport(reportId, clientId);

            return Json(
             new
             {
                 Result = cftReport
             }
             , JsonRequestBehavior.AllowGet);
        }
        public ActionResult RunTableReport(int reportId = 0, int pageElementId = 0)
        {
            //var cftReport = GetCFTReport(ReportID, PageElementID);            
            //return View(cftReport);
            return View();
        }


        public ActionResult CreateReportTemplate()
        {

            return View();
        }



        [HttpPost]
        public ActionResult CreateReportTemplate2(FormCollection formCollection)
        {
            var formDictionary =
               new Dictionary<string, string>();

            for (int i = 0; i < formCollection.Keys.Count; i++)
            {
                string key1 = formCollection.Keys[i];
                formDictionary.Add(key1, formCollection[key1]);
            }

            var reportId = _reportFactory.SaveReportTemplate(formDictionary);

            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = reportId });
        }



        [HttpPost]
        public ActionResult CreateReportTemplate(ModelsLayer.ReportTemplate reportTemplate)
        {
            if (!ModelState.IsValid)
                return View(reportTemplate);

            _reportFactory.CreateReportTemplate(reportTemplate);

            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = reportTemplate.ReportID });
        }


        public JsonResult GetCascadeClaimTemplates()
        {
            var claimTempaltes = _claimTemplateFactory.GetClaimTemplates();

            var result = claimTempaltes.Select(p => new { CategoryId = p.ClaimTemplateID, CategoryName = p.Name });
            return Json(result.AsQueryable(), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetCascadeClaimFieldTemplates(int claimTemplateId = 0)
        {
            var claimFieldTempaltes = _claimFieldTemplateFactory.GetClaimFieldTemplates();

            if (claimTemplateId > 0)
            {
                claimFieldTempaltes = _claimFieldTemplateFactory.GetClaimFieldTemplates(claimTemplateId);
            }

            var result = claimFieldTempaltes.Select(p => new { ProductID = p.ClaimFieldTemplateID, ProductName = p.Name });
            return Json(result.AsQueryable(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetReportFieldTypes()
        {
            var reportFieldTypeList = new List<ReportFieldType>
            {
                new ReportFieldType {TypeName = "XAxis2", TypeCode = "XAxis2"},
                new ReportFieldType {TypeName = "XAxis3", TypeCode = "XAxis3"},
                new ReportFieldType {TypeName = "YAxis1", TypeCode = "YAxis1"},
                new ReportFieldType {TypeName = "YAxis2", TypeCode = "YAxis2"},
                new ReportFieldType {TypeName = "YAxis3", TypeCode = "YAxis3"},
                new ReportFieldType {TypeName = "ZAxis1", TypeCode = "ZAxis1"},
                new ReportFieldType {TypeName = "ZAxis2", TypeCode = "ZAxis2"},
                new ReportFieldType {TypeName = "ZAxis3", TypeCode = "ZAxis3"},
                new ReportFieldType {TypeName = "XAxis1", TypeCode = "XAxis1"},
                new ReportFieldType {TypeName = "Field6", TypeCode = "Field6"},
                new ReportFieldType {TypeName = "Field5", TypeCode = "Field5"},
                new ReportFieldType {TypeName = "Field4", TypeCode = "Field4"},
                new ReportFieldType {TypeName = "Field3", TypeCode = "Field3"},
                new ReportFieldType {TypeName = "Field2", TypeCode = "Field2"},
                new ReportFieldType {TypeName = "Field1", TypeCode = "Field1"}
            };

            return Json(reportFieldTypeList.AsQueryable(), JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetAllReports()
        {

            var reportsArray = new List<CftReport>();

            var reportTemplates = _reportFactory.GetReportTemplates();
            foreach (var report in reportTemplates)
            {
                reportsArray.Add(GetCftReport(report.ReportID, 0, false));
            }

            return Json(
                new
                {
                    Result = reportsArray.ToArray()
                }
                , JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListClientReportTypes()
        {
            var clientClaimTemplates =
              _reportFactory.GetReportTemplates();

            return View(clientClaimTemplates);
        }


        public ActionResult GetReportTemplates()
        {
            var reportTemplate = _reportFactory.GetReportTemplates();
            return Json(
                new
                {
                    Result = (from obj in reportTemplate select new { obj.ReportID, obj.Name })
                }
                , JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateReportField()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateReportField(ReportField reportField)
        {
            if (!ModelState.IsValid) return View(reportField);

            var report = _reportFactory.GetReportTemplate(reportField.ReportID.Value);
            reportField.FieldNumber = report.ReportFields.Count() + 1;
            _reportFieldFactory.CreateReportField(reportField);
            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = reportField.ReportID });

        }



        // id is the ReportID and not the ReportFieldID
        // this command is coming from Report Edit page
        public ActionResult DeleteReportField(int reportId = 0, int reportFieldId = 0)
        {
            if (reportId > 0 && reportFieldId > 0)
            {
                _reportFieldFactory.DeleteReportField(reportFieldId);
            }
            return RedirectToAction("Edit", "Reports", new { id = reportId, area = "Reports" });
        }

        public ActionResult ReportFieldEdit(ReportField reportField)
        {
            return View(reportField);
        }

        public ActionResult EditReportField(int id = 0)
        {
            if (id > 0)
            {
                var reportField = _reportFieldFactory.GetReportField(id);
                return View(reportField);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditReportField(ReportField reportField)
        {
            if (!ModelState.IsValid) return View(reportField);

            _reportFieldFactory.UpdateReportField(reportField);
            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = reportField.ReportID });

        }

        [HttpPost]
        public ActionResult UpdateReportField(ReportField reportField)
        {
            _reportFieldFactory.UpdateReportField(reportField);
            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = reportField.ReportID });
        }



        public ActionResult MoveUp(int ReportID = 0, int ReportFieldID = 0)
        {
            if (ReportID > 0 && ReportFieldID > 0)
                _reportFieldFactory.MoveUp(ReportID, ReportFieldID);
            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = ReportID });
        }

        public ActionResult MoveDown(int ReportID = 0, int ReportFieldID = 0)
        {
            if (ReportID > 0 && ReportFieldID > 0)
                _reportFieldFactory.MoveDown(ReportID, ReportFieldID);
            return RedirectToAction("Edit", "Reports", new { area = "Reports", id = ReportID });
        }
    }
}
