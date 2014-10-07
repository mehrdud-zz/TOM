using System;

namespace ModelsLayer
{
    public class Filter
    {
        public int FieldId;
        public string Name;
    }
    public class PageElementToken
    {
        public double Width;
        public double Height;
        public double Left;
        public double Top;
        public int ReportId;
        public int PageElementId;
        public string FrameId;
        public string Title;
        public string Description;
        public Filter[] Filters;

        public string vAxisTitle;
        public string vAxisColour;

        public string hAxisTitle;
        public string hAxisColour;
        public PageElementToken() { 
        
        }
        public PageElementToken(PageElement pageElement)
        {
            Width = (pageElement.Width != null) ? pageElement.Width.Value : 0;
            Height = (pageElement.Height != null) ? pageElement.Height.Value : 0;
            Left = (pageElement.ElementLeft != null) ? pageElement.ElementLeft.Value : 0;
            Top = (pageElement.ElementTop != null) ? pageElement.ElementTop.Value : 0;
            PageElementId = pageElement.PageElementID;
            ReportId = (pageElement.ReportID != null) ? pageElement.ReportID.Value : 0;
            FrameId = pageElement.FrameID;
            Title = pageElement.Title;
            Description = pageElement.Description;
            Filters = new Filter[0];
            vAxisTitle = pageElement.vAxisTitle;
            vAxisColour = pageElement.vAxisColour;

            hAxisTitle = pageElement.hAxisTitle;
            hAxisColour = pageElement.hAxisColour;

            foreach (ReportField field in pageElement.ReportTemplate.ReportFields)
            {
                if (field.Filter != null && field.Filter.Value == true)
                {
                    Array.Resize(ref this.Filters, this.Filters.Length + 1);
                    this.Filters[this.Filters.Length - 1] = new Filter() { FieldId = field.FieldId.Value, Name = field.DisplayName };
                }
            }
        }
    }
    public partial class PageElement
    {
        public string PanelId
        {
            get
            {
                var panelId = "";
                if (!String.IsNullOrEmpty(FrameID))
                {
                    panelId = FrameID.Replace("Frame", "Panel");
                }
                return panelId;
            }
        }

        public string ReportType { get; set; }

        public string vAxisTitle { get { return ReportTemplate.VAxis; } }
        public string vAxisColour { get { return ReportTemplate.VAxisColour; } }
        public string hAxisTitle { get { return ReportTemplate.HAxis; } }
        public string hAxisColour { get { return ReportTemplate.HAxisColour; } }

        public string Description { get { return ReportTemplate.Description; } }

    }
}