using BrokingPlatformIntegrationBase.Interfaces;
using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{


    public class ClaimsStatusReportItemBase
    {
        public ClaimStatu ClaimStatus;
        public int ClaimStatusCount;
    }
    public class ClaimsStatusReportItem : ClaimsStatusReportItemBase
    {
        public Country Country;
    }


    public class ClaimsStatusReport
    {
        public List<ClaimsStatusReportItem> ClaimsStatusReportItemsIncludingCountryList;
        public List<ClaimsStatusReportItemBase> ClaimsStatusReportItemsList;
        public int TotalClaimCount;

    }




    public interface IReportFactory
    {
        void Initialize();
        ClaimsStatusReport GetClaimStatusReport();
        int SaveReportTemplate(Dictionary<string, string> data);

        ReportTemplate GetReportTemplate(int reportTemplateId);

        List<ReportTemplate> GetReportTemplates();

        int UpdateReportTemplate(Dictionary<string, string> data);

        bool UpdateReportTemplate(ReportTemplate reportTemplate);

        DataTable ExecuteReportQuery(ReportTemplate reportTemplate, out String source, int clientId = 0);
        List<ClientElement> GetClientList();


        bool CreateReportTemplate(ModelsLayer.ReportTemplate reportTemplate);

        bool DeleteReport(ReportTemplate report);
    }


    public class ReportFactory : IReportFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public ClaimsStatusReport GetClaimStatusReport()
        {
            var claimsStatusReport = new ClaimsStatusReport();

            var claim1 =
             from claims in _db.Claims
             group claims by new { country = claims.Country, status = claims.ClaimStatu } into grp
             let counter = grp.Count()
             select new ClaimsStatusReportItem { Country = grp.Key.country, ClaimStatusCount = counter, ClaimStatus = grp.Key.status };

            claimsStatusReport.ClaimsStatusReportItemsIncludingCountryList = claim1.ToList();

            var claim2 =
            from claims in _db.Claims
            group claims by new { status = claims.ClaimStatu } into grp
            let counter = grp.Count()
            select new ClaimsStatusReportItem { ClaimStatusCount = counter, ClaimStatus = grp.Key.status };


            claimsStatusReport.ClaimsStatusReportItemsList = claim2.ToList<ClaimsStatusReportItemBase>();

            claimsStatusReport.TotalClaimCount = _db.Claims.Count();

            return claimsStatusReport;
        }


        public int SaveReportTemplate(Dictionary<string, string> data)
        {
            //var intKeys = new string[] {
            //    "Field1","Field2","Field3","Field4","Field5","Field6","XAxis1","XAxis2","XAxis3","YAxis1","YAxis2","YAxis3","ZAxis1","ZAxis2","ZAxis3"           };

            var stringKeys = new[] {
                "Name","ReportType"
            };


            var dateKeys = new[] {
                "StartDate","EndDate"
            };

            //var functionKeys = new string[]{
            //    "Field1Function","Field2Function","Field3Function","Field4Function","Field5Function","Field6Function","XAxis1Function","XAxis2Function",
            //    "XAxis3Function","YAxis1Function","YAxis2Function","YAxis3Function","ZAxis1Function","ZAxis2Function","ZAxis3Function"
            //};

            var reportTemplate1 = new ReportTemplate();


            if (data != null)
            {
                if (data.Keys.Contains("NumberofRecords"))
                {
                    reportTemplate1.NumberofRecords = Convert.ToInt32(data["NumberofRecords"]);
                    data.Remove("NumberofRecords");
                }

                if (data.Keys.Contains("OrderDirection"))
                {
                    reportTemplate1.OrderDirection = data["OrderDirection"];
                    data.Remove("OrderDirection");
                }
            }

            foreach (string key in stringKeys)
            {
                if (data != null)
                    if (data.Keys.Contains(key))
                    {
                        if (!String.IsNullOrEmpty(data[key]))
                        {
                            reportTemplate1.GetType().GetProperty(key).SetValue(reportTemplate1, data[key]);

                        }
                        data.Remove(key);
                    }
            }

            foreach (string key in dateKeys)
            {
                if (data != null)
                    if (data.Keys.Contains(key))
                    {
                        if (!String.IsNullOrEmpty(data[key]))
                        {
                            reportTemplate1.GetType().GetProperty(key).SetValue(reportTemplate1, Convert.ToDateTime(data[key]));

                        }
                        data.Remove(key);
                    }
            }

            _db.ReportTemplates.Add(reportTemplate1);
            _db.SaveChanges();

            var reportTemplateId = reportTemplate1.ReportID;




            //if (data != null && data.Keys.Any())
            //    foreach (var key in data.Keys)
            //    {
            //        var reportField = new ReportField {ReportID = reportTemplateId};

            //        var value = data[key];
            //        var valueArray = value.Split(',');

            //        if (valueArray.cou)
            //        reportField.FieldId = Convert.ToInt32(valueArray[3]);
            //        reportField.Func = valueArray[1];
            //        _db.ReportFields.Add(reportField);
            //    }

            _db.SaveChanges();
            return reportTemplate1.ReportID;
        }



        public int UpdateReportTemplate(Dictionary<string, string> data)
        {
            //var intKeys = new[] {
            //    "Field1","Field2","Field3","Field4","Field5","Field6","XAxis1","XAxis2","XAxis3","YAxis1","YAxis2","YAxis3","ZAxis1","ZAxis2","ZAxis3"           };

            var stringKeys = new[] {
                "Name","ReportType"
            };


            var dateKeys = new[] {
                "StartDate","EndDate"
            };

            //var functionKeys = new[]{
            //    "Field1Function","Field2Function","Field3Function","Field4Function","Field5Function","Field6Function","XAxis1Function","XAxis2Function",
            //    "XAxis3Function","YAxis1Function","YAxis2Function","YAxis3Function","ZAxis1Function","ZAxis2Function","ZAxis3Function"
            //};

            var reportTemplate1 = new ReportTemplate();


            if (data != null)
            {
                if (data.Keys.Contains("NumberofRecords"))
                {
                    reportTemplate1.NumberofRecords = Convert.ToInt32(data["NumberofRecords"]);
                    data.Remove("NumberofRecords");
                }

                if (data.Keys.Contains("OrderDirection"))
                {
                    reportTemplate1.OrderDirection = data["OrderDirection"];
                    data.Remove("OrderDirection");
                }
            }


            foreach (var key in stringKeys)
            {
                if (data != null)
                    if (data.Keys.Contains(key))
                    {
                        if (!String.IsNullOrEmpty(data[key]))
                        {
                            reportTemplate1.GetType().GetProperty(key).SetValue(reportTemplate1, data[key]);

                        }
                        data.Remove(key);
                    }
            }

            foreach (var key in dateKeys)
            {
                if (data != null)
                    if (data.Keys.Contains(key))
                    {
                        if (!String.IsNullOrEmpty(data[key]))
                        {
                            reportTemplate1.GetType().GetProperty(key).SetValue(reportTemplate1, Convert.ToDateTime(data[key]));

                        }
                        data.Remove(key);
                    }
            }


            _db.ReportTemplates.Add(reportTemplate1);
            _db.SaveChanges();

            var reportTemplateId = reportTemplate1.ReportID;

            if (data != null && data.Keys.Any())
                foreach (string key in data.Keys)
                {
                    var reportField = new ReportField();
                    reportField.ReportID = reportTemplateId;

                    var value = data[key];
                    var valueArray = value.Split(',');

                    reportField.FieldId = Convert.ToInt32(valueArray[3]);
                    reportField.Func = valueArray[1];
                    _db.ReportFields.Add(reportField);
                }

            _db.SaveChanges();
            return reportTemplate1.ReportID;
        }




        public ReportTemplate GetReportTemplate(int reportTemplateId)
        {
            var report = _db.ReportTemplates.Include("ReportFields.ClaimFieldTemplate.ClaimFieldGroupTemplate.ClaimTemplate").Single(m => m.ReportID == reportTemplateId);
            report.ReportFields.OrderBy(m => m.FieldNumber);

            return report;
        }

        public List<ReportTemplate> GetReportTemplates()
        {
            return _db.ReportTemplates.ToList();
        }





        public DataTable ExecuteReportQuery(ReportTemplate reportTemplate, out string source, int clientId = 0)
        {
            var dt = new DataTable();
            var fieldToLookupValue = "FloatValue";
            var fieldName = "Field1";
            var queryParts = new List<string>();
            var orderParts = new List<string>();
            var groupByParts = new List<string>();
            var queryParts2 = new List<string>();
            var queryParts3 = new List<string>();
            var queryParts5 = new List<string>();

            var whereParts = new List<string>();


            var fieldIds1 = new List<String>();

            // var claimTemplateIdList = new List<int>();

            var jsonHeadings = new List<String>();

            var flattenClaimFields = new List<String>();

            var reportFieldVar = _db.ReportFields.Where(m => m.ReportID == reportTemplate.ReportID).OrderBy(m => m.FieldNumber);


            source = "CFT";
            var enforceGroupBy = false;
            if (reportFieldVar.Any())
            {
                var reportFieldList = reportFieldVar.ToList();

                foreach (var reportField in reportFieldList)
                {
                    if (reportField != null)
                    {


                        whereParts.Add(" ClaimFieldTemplateID=" + reportField + " ");
                        var field = reportField;
                        var claimFieldTemplateVariable = _db.ClaimFieldTemplates.Where(m => m.ClaimFieldTemplateID == field.FieldId);

                        if (claimFieldTemplateVariable.Any())
                        {
                            var claimFieldTemplate = claimFieldTemplateVariable.Single();
                            var tempClaimFieldGroupTemplate = _db.ClaimFieldGroupTemplates.Single(m => m.ClaimFieldGroupTemplateID == claimFieldTemplate.ClaimFieldGroupTemplateID);

                            //var templateId = 0;

                            //if (tempClaimFieldGroupTemplate != null &&
                            //    tempClaimFieldGroupTemplate.ClaimTemplateID != null)
                            //    templateId = tempClaimFieldGroupTemplate.ClaimTemplateID.Value;

                            // claimTemplateIdList.Add(templateId);
                            fieldName = "[" + claimFieldTemplate.Name + "]";
                            if (!String.IsNullOrEmpty(reportField.TableName))
                                fieldName = "[" + reportField.TableName + "]." + fieldName;

                            var fieldDisplayName = (!String.IsNullOrEmpty(reportField.DisplayName) ? reportField.DisplayName : fieldName);

                            // jsonHeadings.Add(fieldName);

                            //'string' 'number' 'boolean' 'date' 'datetime' 'timeofday'.

                            switch (claimFieldTemplate.FieldType.Name)
                            {
                                case "Integer":
                                    fieldToLookupValue = "IntegerValue";

                                    break;

                                case "Float":
                                    fieldToLookupValue = "FloatValue";
                                    break;

                                case "ShortText":
                                    fieldToLookupValue = "ShortTextValue";
                                    break;

                                case "Money":
                                    fieldToLookupValue = "CurrecncyValue";
                                    break;

                                case "Date":
                                    fieldToLookupValue = "DateValue";
                                    break;

                                case "Date Time":
                                    fieldToLookupValue = "DateValue";
                                    break;

                                default:
                                    fieldToLookupValue = "ShortTextValue";
                                    break;
                            }



                            if (reportField.Func != null)
                                switch (reportField.Func.ToUpper())
                                {
                                    case "FIELDVALUE":


                                        queryParts.Add(fieldName + " AS [" + fieldDisplayName + "] ");
                                        groupByParts.Add(fieldName);
                                        queryParts3.Add(fieldName);

                                        jsonHeadings.Add(fieldDisplayName);


                                        if (!String.IsNullOrEmpty(reportField.Direction))
                                            orderParts.Add("[" + fieldDisplayName + "] " + reportField.Direction);
                                        break;

                                    case "COUNT":
                                        queryParts.Add(reportField.Func.ToUpper() + "(" + fieldName + ") AS  [" + fieldDisplayName + "] ");
                                        fieldIds1.Add("[" + reportField.FieldId + "]");


                                        jsonHeadings.Add(fieldDisplayName);


                                        if (!String.IsNullOrEmpty(reportField.Direction))
                                            orderParts.Add("[" + fieldDisplayName + "] " + reportField.Direction);


                                        enforceGroupBy = true;
                                        break;

                                    case "AVG":
                                    case "MAX":
                                    case "MIN":
                                    case "SUM":
                                        queryParts.Add(reportField.Func.ToUpper() + "(" + fieldName + ") AS  [" + fieldDisplayName + "] ");
                                        fieldIds1.Add("[" + reportField.FieldId + "]");

                                        jsonHeadings.Add(fieldDisplayName);

                                        if (!String.IsNullOrEmpty(reportField.Direction))
                                            orderParts.Add("[" + fieldDisplayName + "] " + reportField.Direction);

                                        enforceGroupBy = true;
                                        break;
                                }
                        }
                        else
                            queryParts.Add("NULL AS [" + fieldName + "]");



                        //'string' 'number' 'boolean' 'date' 'datetime' 'timeofday'



                        queryParts5.Add(fieldToLookupValue + " AS [" + fieldName + "]");
                        queryParts2.Add("MAX(" + fieldName + " AS [" + fieldName + "]");

                        if (reportField.FieldId != null)
                            flattenClaimFields.Add("MAX(CASE When ClaimFieldTemplateID = " + reportField.FieldId.Value + " THEN " + fieldToLookupValue + " ELSE NULL END) AS [" + fieldName + "] ");
                    }

                }

            }

            if (reportTemplate.ClaimTemplateId != null)
            {

                //var claimTemplate = _db.ClaimTemplates.Single(m => m.ClaimTemplateID == claimTemplateId1);
                //claimTemplateIdList = claimTemplateIdList.Distinct().ToList();


                var claimTemplateIdsString = "(" + string.Join(",", reportTemplate.ClaimTemplateId.Value) + ")";

                var isActuris =
                    (reportTemplate.ClaimTemplate.Source == "Acturis");
                //(claimTemplate.Source == "Acturis");


                source = isActuris ? "Acturis" : "CFT";


                var whereClause = new List<string>();



                string query = "";
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["reportConnectionString"].ConnectionString;



                switch (reportTemplate.ClaimTemplate.Source)
                {
                    case "CFT":


                        whereClause.Add("Claim.ClaimTemplateID  IN " + claimTemplateIdsString);

                        if (reportTemplate.StartDate != null)
                            whereClause.Add("[Claim].[DateCreated] >=  @ReportTemplateStartDate");

                        if (reportTemplate.EndDate != null)
                            whereClause.Add("[Claim].[DateCreated] <=  @ReportTemplateEndDate");

                        if (clientId > 0)
                            whereClause.Add(" [Client].[ClientID] = " + clientId);



                        query =
                            "SELECT\r\n\t" +
                            ((reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n TOP " + reportTemplate.NumberofRecords.Value + " ") : " ") +
                            String.Join(",\r\n\t", queryParts.ToArray()) +
                            "\r\nFROM (\r\n" +
                            "SELECT Claim.ClaimID," +
                            " Client.ClientID, " +
                            String.Join(",\r\n\t", flattenClaimFields.ToArray()) +
                            " FROM ClaimField " +
                            " INNER JOIN ClaimFieldGroup  ON ClaimField.ClaimFieldGroupID = ClaimFieldGroup.ClaimFieldGroupID " +
                            " INNER JOIN Claim ON ClaimFieldGroup.ClaimID = Claim.ClaimID " +
                            " INNER JOIN [User] ON Claim.UserID = [User].UserID " +
                            " INNER JOIN Client ON Client.ClientID = [User].ClientID " +
                            ((whereClause.Any()) ? "\r\nWHERE " + String.Join("\r\n\tAND ", whereClause.ToArray()) : String.Empty) +

                            //" WHERE Claim.ClaimTemplateID  IN " + claimTemplateIDsString + " " +

                            //((ReportTemplate.StartDate != null) ? " AND [Claim].[DateCreated] >=  @ReportTemplateStartDate " : " ") +
                            //((ReportTemplate.EndDate != null) ? " AND [Claim].[DateCreated] <=   @ReportTemplateEndDate " : " ") +

                            " GROUP BY Claim.ClaimID, Client.ClientID " +
                            ")  AS totalRowsII \r\n" +
                            ((groupByParts.Any() && enforceGroupBy) ? "\r\n GROUP BY \r\n" + String.Join(",\r\n\t", groupByParts.ToArray()) : "") +
                             ((orderParts.Any() && reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n ORDER BY \r\n" + String.Join(",\r\n\t", orderParts.ToArray())) : " ")
                            ;
                        break;
                    case "Acturis":

                        if (reportTemplate.StartDate != null)
                            whereClause.Add("[ClaimCore].[ADWLastUpdate] >=  @ReportTemplateStartDate");

                        if (reportTemplate.EndDate != null)
                            whereClause.Add("[ClaimCore].[ADWLastUpdate] <=  @ReportTemplateEndDate");

                        if (clientId > 0)
                            whereClause.Add(" [Client].[ClientID] = " + clientId);


                        query =
                            "SELECT\r\n\t" +
                            ((reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n TOP " + reportTemplate.NumberofRecords.Value + " ") : " ") +
                            String.Join(",\r\n\t", queryParts.ToArray()) +
                            "\r\n FROM (" +
                            "\r\n\t SELECT TOP 100000   " +
                            "\r\n\t\t [ClaimCore].*, " +
                            //"\r\n\t\t [ClaimAmount].[Amount] , " +
                            "\r\n\t\t [Country].[Country] AS [CountryName], " +
                            "\r\n\t\t [NatureOfInjury].[Description] AS [Nature of Injury],  " +
                            "\r\n\t\t [ClaimStatus].[Description] AS [Claim Status], " +
                            "\r\n\t\t [ClaimCauseType].[ClaimCause] AS [Claim Cause], " +
                            "\r\n\t\t [Client].[ClientId] AS [ClientId], " +
                            "\r\n\t\t [Client].[Name] AS [Client] " +
                            "\r\n\t FROM [ClaimCore]  " +

                            "\r\n\t INNER JOIN [Policy] ON [ClaimCore].[PolicyVersionRef] = [Policy].[VersionRef]" +
                            "\r\n\t INNER JOIN [Country] ON [Country].[CountryId] = [ClaimCore].[Country]" +
                            "\r\n\t INNER JOIN [Client] ON [Client].[ClientId] = [Policy].[ClientId]" +


                            "\r\n\t INNER JOIN [NatureOfInjury] ON [ClaimCore].[NatureOfInjury] = [NatureOfInjury].[NatureOfInjuryId] " +
                            "\r\n\t INNER JOIN [ClaimStatus] ON [ClaimCore].[ClaimStatus] = [ClaimStatus].[ClaimStatusId]" +
                            "\r\n\t INNER JOIN [ClaimCauseType] ON [ClaimCore].[ClaimCause] = [ClaimCauseType].[ClaimCauseRef]" +
                            // "\r\n\t LEFT JOIN [ClaimAmount] on [ClaimCore].[ClaimId] = [ClaimAmount].ClaimId" +

                            ((whereClause.Any()) ? "\r\nWHERE" + String.Join("\r\n\tAND ", whereClause.ToArray()) : String.Empty) +

                            "\r\n ORDER BY  [ClaimCore].[ADWLastUpdate] DESC " +
                            "\r\n)  AS totalRowsII \r\n" +
                            ((groupByParts.Any() && enforceGroupBy) ? "\r\n GROUP BY \r\n" + String.Join(",\r\n\t", groupByParts.ToArray()) : "") +
                            ((orderParts.Any() && reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n ORDER BY \r\n" + String.Join(",\r\n\t", orderParts.ToArray())) : " ")
                            ;


                        connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["acturisReportConnectionString"].ConnectionString;

                        break;


                    case "Eclipse":
                        query =
                            "SELECT\r\n\t" +
                            ((reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n TOP " + reportTemplate.NumberofRecords.Value + " ") : " ") +
                            String.Join(",\r\n\t", queryParts.ToArray()) +
                            "  FROM  " +
                            " [tblPolicy] " +
                            "  INNER JOIN [EclipseODS].[dbo].[tblPolicySection]  " +
                            "  ON [tblPolicy].[PolicyId] = [tblPolicySection].[PolicyId] " +
                            "  AND [tblPolicySection].[PolicySectionStatus] = 'Authorised' " +
                            "  AND [tblPolicy].[BusinessType] = 'Direct' " +
                            "  AND [tblPolicy].[PolicyExpired] = 'No' " +
                            "  AND [tblPolicy].[PolicyStatus] = 'Authorised' " +

                            " INNER JOIN [EclipseODS].[dbo].[tblClaim]  " +
                            " ON [tblClaim].[PolicySectionId] = [tblPolicySection].[PolicySectionId] " +

                            "  CROSS APPLY  ( " +
                            " SELECT TOP 1 *  " +
                            " FROM [EclipseODS].[dbo].[tblClaimMovement]  " +
                            "    WHERE [tblClaimMovement].[ClaimId] = [tblClaim].[ClaimId] " +
                            "   AND [tblClaimMovement].[CancelledReason] IS NULL    AND [tblClaimMovement].[Deleted] = 0 " +
                            " ORDER BY [tblClaimMovement].[SequenceNumber] DESC  ) AS LatestMovements " +
                            " INNER JOIN [EclipseODS].[dbo].[tblClaimMovementBreakdown] " +
                            " ON [tblClaimMovementBreakdown].[ClaimMovementId]  = LatestMovements.[ClaimMovementId] " +
                            " AND [tblClaimMovementBreakdown].[Deleted] = 0 " +
                            ((whereClause.Any()) ? "\r\n  " + String.Join("\r\n\tAND ", whereClause.ToArray()) : String.Empty) +
                            ((groupByParts.Any() && enforceGroupBy) ? "\r\n GROUP BY \r\n" + String.Join(",\r\n\t", groupByParts.ToArray()) : "") +
                            ((orderParts.Any() && reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n ORDER BY \r\n" + String.Join(",\r\n\t", orderParts.ToArray())) : " ")
                            ;


                        connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["eclipseReportConnectionString"].ConnectionString;
                        break;


                    case "Eclipse Policy":

                        query =
                            "SELECT\r\n\t" +
                            ((reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n TOP " + reportTemplate.NumberofRecords.Value + " ") : " ") +
                            String.Join(",\r\n\t", queryParts.ToArray()) +

                     "  FROM [EclipseODS].[dbo].[tblPolicy] " +
                     " INNER JOIN [EclipseODS].[dbo].[tblPolicySection]  " +
                     "   ON [tblPolicy].[PolicyId] = [tblPolicySection].[PolicyId] " +
                     " AND [tblPolicySection].[PolicySectionStatus] = 'Authorised' " +
                     "AND [tblPolicy].[BusinessType] = 'Direct' " +
                     "    AND [tblPolicy].[PolicyExpired] = 'No' " +
                     "    AND [tblPolicy].[PolicyStatus] = 'Authorised' " +
                     "  INNER JOIN [EclipseODS].[dbo].[tblClaim]  " +
                       "    ON [tblClaim].[PolicySectionId] = [tblPolicySection].[PolicySectionId] " +

                     "  WHERE [tblPolicy].[BusinessType] = 'Direct' " +
                     "  AND [tblPolicy].[PolicyExpired] = 'No' " +
                     "  AND [tblPolicy].[PolicyStatus] = 'Authorised' " +
                            ((whereClause.Any()) ? "\r\n AND " + String.Join("\r\n\tAND ", whereClause.ToArray()) : String.Empty) +
                            ((groupByParts.Any() && enforceGroupBy) ? "\r\n GROUP BY \r\n" + String.Join(",\r\n\t", groupByParts.ToArray()) : "") +
                            ((orderParts.Any() && reportTemplate.NumberofRecords != null && reportTemplate.NumberofRecords.Value > 0) ? ("\r\n ORDER BY \r\n" + String.Join(",\r\n\t", orderParts.ToArray())) : " ")
                            ;


                        connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["eclipseReportConnectionString"].ConnectionString;
                        break;
                    default:
                        break;
                }



                using (var sqlConnecion = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    sqlConnecion.Open();


                    using (var da = new System.Data.SqlClient.SqlDataAdapter(query, sqlConnecion))
                    {
                        if (reportTemplate.StartDate != null)
                            da.SelectCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("ReportTemplateStartDate", reportTemplate.StartDate.Value));

                        if (reportTemplate.EndDate != null)
                            da.SelectCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("ReportTemplateEndDate", reportTemplate.EndDate.Value));

                        da.Fill(dt);

                    }
                }
            }
            return dt;
        }


        public bool UpdateReportTemplate(ReportTemplate reportTemplate)
        {
            _db.Entry(reportTemplate).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }


        public List<ClientElement> GetClientList()
        {
            var clientElementList =
                new List<ClientElement>();

            var clientList = _db.Clients;

            foreach (var client in clientList)
            {
                clientElementList.Add(
                    new ClientElement
                    {
                        Name = client.Name,
                        ClientID = client.ClientID,
                        Source = "Acturis"
                    }
                    );
            }
            return clientElementList;
        }


        public bool CreateReportTemplate(ModelsLayer.ReportTemplate reportTemplate)
        {
            _db.ReportTemplates.Add(reportTemplate);
            _db.SaveChanges();
            return true;
        }


        public bool DeleteReport(ReportTemplate report)
        {
            _db.ReportTemplates.Remove(report);
            _db.SaveChanges();
            return true;
        }
    }
}