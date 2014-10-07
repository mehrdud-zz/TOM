using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace Factories
{
    public interface IClaimFactory
    {
        void Initialize();
        Claim GetClaim(int claimId);
        List<Claim> GetClaims();
        bool CreateClaim(Claim claim);
        bool UpdateClaim(Claim claim);
        bool DeleteClaim(Claim claim);
        void Dispose(bool disposing);
        int CreateFromTemplate(int claimTemplateId, Dictionary<string, string> formCollection, User currentUser, ClaimTemplate myTemplate);
        int UpdateFromTemplate(Claim claim, Dictionary<string, string> formCollection);
        void SubmitClaim(Claim newClaim);
        int GetClaimsCount();
        List<Claim> GetListofUnassignedClaims();
    }

    public class ClaimFactory : IClaimFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public Claim GetClaim(int claimId)
        {
            return _db.Claims.Include("ClaimFieldGroups.ClaimFields").Single(m => m.ClaimID == claimId);
        }


        public List<BrokingPlatformIntegrationBase.Interfaces.IClaim> GetClaimsCombined()
        { 
            return null;
        }

        public List<Claim> GetClaims()
        {
            return _db.Claims.ToList();
        }

        public bool CreateClaim(Claim claim)
        {
            claim.CreatedBy = HttpContext.Current.User.Identity.Name;
            _db.Claims.Add(claim);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateClaim(Claim claim)
        {
            _db.Entry(claim).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteClaim(Claim claim)
        {
            _db.Claims.Remove(claim);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public int CreateFromTemplate(int claimTemplateId, Dictionary<string, string> formCollection, User currentUser, ClaimTemplate myTemplate)
        {
            var claim = new Claim
            {
                ClaimTemplateID = claimTemplateId,
                CreatedBy = "User.Identity.Name",
                DateCreated = DateTime.Now,
                UserID = currentUser.UserId,
                ClaimStatusID = 1,
                WillisEmployeeID = 1
            };

            var claimName =
                     claim.CreatedBy + " - " +
                     claim.ClaimTemplateID + " - " +
                     ((claim.DateCreated != null) ? (claim.DateCreated.Value).ToShortDateString() : String.Empty);

            claim.Name = claimName.Length < 50 ? claimName : claimName.Substring(0, 50); 
            CreateClaim(claim);

            var claimId = claim.ClaimID; // here yo

            var fieldTypesList = _db.FieldTypes.ToList();

            foreach (var claimFieldGroupTemplate in myTemplate.ClaimFieldGroupTemplates)
            {
                var claimFieldGroup =
                    new ClaimFieldGroup
                    {
                        Name = claimFieldGroupTemplate.Name,
                        Description = claimFieldGroupTemplate.Description,
                        ItemOrder = claimFieldGroupTemplate.ItemOrder
                    };

                claim.ClaimFieldGroups.Add(claimFieldGroup);

                foreach (var claimFieldTemplate in claimFieldGroupTemplate.ClaimFieldTemplates)
                {
                    var claimField =
                        new ClaimField
                        {
                            Name = claimFieldTemplate.Name,
                            Code = claimFieldTemplate.Code,
                            ClaimFieldTemplateID = claimFieldTemplate.ClaimFieldTemplateID,
                            ClaimFieldGroupID = claimFieldGroup.ClaimFieldGroupID,
                            TemplateBName = claimFieldTemplate.FieldType.TemplateName
                        };

                    claimFieldGroup.ClaimFields.Add(claimField);

                    if (claimField.Code != null && formCollection.Keys.Contains(claimField.Code))
                    {
                        string value = formCollection[claimField.Code];

                        if (value != null)
                        {
                            var fieldType = fieldTypesList.Single(ft => ft.FieldTypeID == claimFieldTemplate.FieldTypeID);
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
                }
            }




            _db.SaveChanges();



            return claimId;
        }


        public void CreateFieldsForClaimFromTemplate(Claim claim)
        {

            var mytemplate = _db.ClaimTemplates.Find(
                ((claim.ClaimTemplateID != null) ? claim.ClaimTemplateID.Value : 0)
                );

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

        }

        public int CreateFromTemplate(int claimTemplateId, Dictionary<string, string> formCollection, User currentUser)
        {

            var claim = new Claim
            {
                ClaimTemplateID = claimTemplateId,
                CreatedBy = ((currentUser != null) ? currentUser.UserName : String.Empty),
                DateCreated = DateTime.Now,
                UserID = ((currentUser != null) ? currentUser.UserId : 0),
                ClaimStatusID = 1,
                WillisEmployeeID = 1
            };



            var claimName =
                claim.CreatedBy + " - " +
                claim.ClaimTemplateID + " - " +
                ((claim.DateCreated != null) ? (claim.DateCreated.Value).ToShortDateString() : String.Empty);

            claim.Name = claimName.Length < 50 ? claimName : claimName.Substring(0, 50);

            _db.Claims.Add(claim);
            _db.SaveChanges();

            var mytemplate = _db.ClaimTemplates.Find(claimTemplateId); 

            var fieldTypesList = _db.FieldTypes.ToList();

            foreach (var claimFieldGroupTemplate in mytemplate.ClaimFieldGroupTemplates)
            {
                var claimFieldGroup =
                    new ClaimFieldGroup
                    {
                        Name = claimFieldGroupTemplate.Name,
                        Description = claimFieldGroupTemplate.Description,
                        ItemOrder = claimFieldGroupTemplate.ItemOrder
                    };

                claim.ClaimFieldGroups.Add(claimFieldGroup);

                foreach (var claimFieldTemplate in claimFieldGroupTemplate.ClaimFieldTemplates)
                {
                    var claimField =
                        new ClaimField
                        {
                            Name = claimFieldTemplate.Name,
                            Code = claimFieldTemplate.Code,
                            ClaimFieldTemplateID = claimFieldTemplate.ClaimFieldTemplateID,
                            ClaimFieldGroupID = claimFieldGroup.ClaimFieldGroupID,
                            TemplateBName = claimFieldTemplate.FieldType.TemplateName
                        };

                    claimFieldGroup.ClaimFields.Add(claimField);

                    var value = formCollection[claimField.Code];

                    if (value != null)
                    {
                        var fieldType = fieldTypesList.Single(ft => ft.FieldTypeID == claimFieldTemplate.FieldTypeID);
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
            }
            _db.SaveChanges();

            _db.Dispose();

            return claim.ClaimID;
        }


        public int UpdateFromTemplate(Claim claim, Dictionary<string, string> formCollection)
        {



            var claimFields =
                _db.ClaimFields.Where(m => m.ClaimFieldGroup.ClaimID == claim.ClaimID);


            var claimId = claim.ClaimID; // here yo

            var fieldTypesList = _db.FieldTypes.ToList();

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


                //  db.ClaimFields.Add(claimField);

            }


            _db.Entry(claim).State = EntityState.Modified;


            _db.SaveChanges();

            return claimId;
        }

        public void SubmitClaim(Claim newClaim)
        {
            newClaim.CreatedBy = HttpContext.Current.User.Identity.Name;
            newClaim.DateCreated = DateTime.Now;

            _db.Claims.Add(newClaim);

            _db.SaveChanges();


            foreach (var claimFieldGroup in newClaim.ClaimFieldGroups)
            {
                foreach (var claimField in claimFieldGroup.ClaimFields)
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


        public List<Claim> GetListofUnassignedClaims()
        {
            var unassignedClaimsList =
                from claims in _db.Claims
                where claims.WillisEmployeeID == null
                select claims;

            return unassignedClaimsList.ToList();
        }

    }
}