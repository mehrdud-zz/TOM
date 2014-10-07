using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ClaimFielddata))]
    public partial class ClaimField: BrokingPlatformIntegrationBase.Interfaces.IClaimField
    {
        public string Value
        {
            get
            {
                string result = "";

                if (this.ClaimFieldTemplate != null && this.ClaimFieldTemplate.FieldType != null)
                    switch (this.ClaimFieldTemplate.FieldType.Code)
                    {
                        case "ShortText":
                            result = this.ShortTextValue;
                            break;

                        case "LongText":
                            result = this.LongTextValue;
                            break;

                        case "Integer":
                            result = this.IntegerValue.ToString();
                            break;

                        case "Float":
                            result = this.FloatValue.ToString();
                            break;

                        case "Date":
                            result = (this.DateValue != null ? this.DateValue.Value.ToShortDateString() : "");
                            break;

                        case "DateTime":
                            result = (this.DateTimeValue != null ? this.DateTimeValue.Value.ToString() : "");
                            break;

                        case "DropDown":
                            result = this.DropDownValue;
                            break;

                        case "MultiChoice":
                            result = this.MultiChoiceValue;
                            break;

                        case "File":
                            result = "File";
                            break;

                        case "Money":
                            result =
                                String.Format(
                                "{0}{1}",
                                "",
                                this.CurrecncyValue != null ? this.CurrecncyValue.Value.ToString() : "");

                            break;

                        case "Country":
                            result = this.CountryValue;
                            break;

                        case "Range":
                            result = this.RangeValue.Value.ToString();
                            break;

                        default:
                            break;
                    }

                return result;
            }

            set { }
        }


        public string TemplateBName { get; set; }


        public string Description
        {
            get
            {
                return this.Description;
            }
            set
            {
                
            }
        }


        public string TemplateName
        {
            get
            {
                return this.ClaimFieldTemplate.FieldType.TemplateName;
            }
            set
            {

            }
        }




        string BrokingPlatformIntegrationBase.Interfaces.IClaimField.ClaimFieldTemplate
        {
            get;
            set;
        }

        public bool Mandatory
        {
            get;
            set;
        }

        public string CountryValueName { get; set; }

        public string CurrecncySign { get; set; }
    }

    public class ClaimFielddata
    {
        //[DataType(DataType.MultilineText)]
        //public string Description { get; set; }


    }
}