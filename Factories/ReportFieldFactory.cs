using ModelsLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public interface IReportFieldFactory
    {
        void Initialize();
        ReportField GetReportField(int reportFieldId);
        List<ReportField> GetReportFields();
        List<ReportField> GetReportFields(int reportId);
        bool CreateReportField(ReportField reportField);
        bool UpdateReportField(ReportField reportField);
        bool DeleteReportField(ReportField reportField);

        bool DeleteReportField(int reportFieldId);
        void Dispose(bool disposing);
        
        void MoveUp(int ReportID, int ReportFieldID);
        void MoveDown(int ReportID, int ReportFieldID);
    }

    public class ReportFieldFactory : IReportFieldFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public ReportField GetReportField(int reportFieldId)
        {
            var reportField = _db.ReportFields.Include("ClaimFieldTemplate.ClaimFieldGroupTemplate.ClaimTemplate").Single(m => m.ReportFieldID == reportFieldId);
            //reportField.ClaimTemplateName = reportField.ClaimFieldTemplate.ClaimFieldGroupTemplate.ClaimTemplate.Name;
            return reportField;
        }

        public List<ReportField> GetReportFields()
        {
            return _db.ReportFields.ToList();
        }

        public bool CreateReportField(ReportField reportField)
        {
            _db.ReportFields.Add(reportField);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateReportField(ReportField reportField)
        {
            _db.Entry(reportField).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteReportField(ReportField reportField)
        {
            _db.ReportFields.Remove(reportField);
            _db.SaveChanges();
            return true;
        }


        public bool DeleteReportField(int reportFieldId)
        {
            var reportField = GetReportField(reportFieldId);
            return DeleteReportField(reportField);
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public List<ReportField> GetReportFields(int reportId)
        {
            var reportFields =
                _db.ReportFields.Include("ClaimFieldGroupTemplate").Include("ClaimTemplate").Where(m => m.ReportID == reportId);

            return reportFields.ToList();
        }


        public void MoveUp(int ReportID, int ReportFieldID)
        {
            var reportField = GetReportField(ReportFieldID);
            var report = _db.ReportTemplates.Single(m => m.ReportID == ReportID);

            if (reportField != null && reportField.FieldNumber > 1 && reportField.FieldNumber != null)
            {
                int newIndex = reportField.FieldNumber.Value - 1;
                foreach (var field in report.ReportFields)
                {
                    if (field.FieldNumber != null && field.FieldNumber.Value == newIndex && field.ReportFieldID != reportField.ReportFieldID)
                    {
                        field.FieldNumber += 1;
                    }
                }
                reportField.FieldNumber -= 1;
            }
            else
                reportField.FieldNumber = report.ReportFields.Count();

            _db.SaveChanges();            
        }


        public void MoveDown(int ReportID, int ReportFieldID)
        {
            var reportField = GetReportField(ReportFieldID);
            var report = _db.ReportTemplates.Single(m => m.ReportID == ReportID);

            if (reportField != null && reportField.FieldNumber != report.ReportFields.Count() && reportField.FieldNumber != null)
            {
                int newIndex = reportField.FieldNumber.Value + 1;
                foreach (var field in report.ReportFields)
                {
                    if (field.FieldNumber != null && field.FieldNumber.Value == newIndex && field.ReportFieldID != reportField.ReportFieldID)
                    {
                        field.FieldNumber -= 1;
                    }
                }
                reportField.FieldNumber += 1;
            }
            else
                reportField.FieldNumber = report.ReportFields.Count();

            _db.SaveChanges();   
        }
    }
}