using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ClaimFieldTemplateMetadata))]
    public partial class ClaimFieldTemplate
    {
        public string Value
        {
            get
            {
                string result = "";

                switch (this.FieldType.Code)
                {
                    case "ShortText":
                        result = this.ShortTextDefaultValue;
                        break;

                    case "LongText":
                        result = this.LongTextDefaultValue;
                        break;

                    case "Integer":
                        result = this.IntegerDefaultValue.ToString();
                        break;

                    case "Float":
                        result = this.FloatDefaultValue.ToString();
                        break;

                    case "Date":
                        result = (this.DateDefaultValue != null ? this.DateDefaultValue.Value.ToShortDateString() : "");
                        break;

                    case "DateTime":
                        result = (this.DateTimeDefaultValue != null ? this.DateTimeDefaultValue.Value.ToString() : "");
                        break;

                    case "DropDown":
                        result = this.DropDownDefaultValue;
                        break;

                    case "MultiChoice":
                        result = this.MultiChoiceDefaultValue;
                        break;

                    case "File":
                        result = "File";
                        break;

                    case "Money":
                        result =
                            String.Format(
                            "{0}{1}",
                            "",
                            this.CurrecncyDefaultValue != null ? this.CurrecncyDefaultValue.Value.ToString() : "");

                        break;

                    case "Country":
                        result = this.CountryDefaultValue;
                        break;

                    case "Range":
                        result = this.RangeDefaultValue.Value.ToString();
                        break;

                    default:
                        break;
                }

                return result;
            }
        }

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class CheckFieldValue : ValidationAttribute
    {
        public CheckFieldValue()
            : base("Field must have value.") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Value cannot be null.");

            return ValidationResult.Success;
        }
    }


    public class ClaimFieldTemplateMetadata
    {
        [Required(ErrorMessage = "Name is requried")]
        public string Name { get; set; }

        [Required(ErrorMessage = "FieldTypeID is requried")]
        public Nullable<int> FieldTypeID { get; set; } 
    }
}