using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Factories
{
    public class PageElementDetail
    {
        public double Width;
        public double Height;
        public double Top;
        public double Left;
        public int ReportId;
        public string FrameId;
        public string FrameSrc;
        public string Title;
    }

    public class PageElementDetailList
    {
        public List<PageElementDetail> PageElementDetails;
        public String Username;
        public String PageUrl;
        public String Name;
    }

    public interface IPageElementFactory
    {
        void Initialize();
        PageElement GetPageElement(int pageElementId);
        List<PageElement> GetPageElements();
        bool CreatePageElement(PageElement pageElement);
        bool UpdatePageElement(PageElement pageElement);
        bool DeletePageElement(PageElement pageElement);
        void Dispose(bool disposing);

        bool SavePage(PageElementDetailList pageElementDetailList, string username);


        List<PageElement> GetPageElementListByUsername(String username);

        List<PageElement> GetPageElementListByUsernameandPageUrl(String username, String pageUrl);


        List<PageElement> GetPageElementListByDashboardId(int dashboardId);
    }

    public class PageElementFactory : IPageElementFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public PageElement GetPageElement(int pageElementId)
        {
            var pageElement = _db.PageElements.Find(pageElementId);
            pageElement.ReportType = _db.ReportTemplates.Single(m => m.ReportID == pageElement.ReportID.Value).ReportType;
            return pageElement;
        }

        public List<PageElement> GetPageElements()
        {
            return _db.PageElements.ToList();
        }

        public bool CreatePageElement(PageElement pageElement)
        {
            _db.PageElements.Add(pageElement);
            _db.SaveChanges();
            return true;
        }

        public bool UpdatePageElement(PageElement pageElement)
        {
            _db.Entry(pageElement).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeletePageElement(PageElement pageElement)
        {
            _db.PageElements.Remove(pageElement);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public bool SavePage(PageElementDetailList pageElementDetailList, string username)
        {
            var userId = 0;
            var users = _db.Users.Where(m => m.UserName.Equals(username)).ToList();


            for (var i = 0; i < users.Count(); i++)
            {
                if (users[i] != null)
                {
                    userId = users[i].UserId;

                    var id = userId;
                    var pageSetups = _db.PageSetups.Where(ps => ps.UserID == id);
                    foreach (var pageSetup in pageSetups)
                    {
                        //_db.PageSetups.Remove(pageSetup);
                    }
                }
            }

            if (userId > 0)
            {
                var newPageSetup =
                    new PageSetup { UserID = userId, PageURL = pageElementDetailList.PageUrl, Name = pageElementDetailList.Name };

                _db.PageSetups.Add(newPageSetup);
                _db.SaveChanges();

                foreach (var ped in pageElementDetailList.PageElementDetails)
                {
                    if (ped.ReportId > 0)
                    {
                        var report = _db.ReportTemplates.Single(m => m.ReportID == ped.ReportId);

                        var pageElement = new PageElement
                        {
                            PageSetupID = newPageSetup.PageSetupID,
                            FrameID = ped.FrameId,
                            ElementLeft = ped.Left,
                            ElementTop = ped.Top,
                            ReportID = ped.ReportId,
                            Width = ped.Width,
                            Height = ped.Height,
                            Title = report.Name
                        };
                        _db.PageElements.Add(pageElement);
                    }
                }
                _db.SaveChanges();
            }

            _db.SaveChanges();
            return true;
        }


        public List<PageElement> GetPageElementListByUsername(String username)
        {
            var pageElementList = new List<PageElement>();

            if (!String.IsNullOrEmpty(username))
            {
                var pageElements =
                    _db.PageElements.Where(m => m.PageSetup.User.UserName == username);

                foreach (var pe in pageElements)
                {
                    pe.ReportType = _db.ReportTemplates.Single(m => m.ReportID == pe.ReportID.Value).ReportType;
                }

                if (pageElements.Any())
                {
                    pageElementList = pageElements.ToList();
                }
            }

            return pageElementList;
        }


        public List<PageElement> GetPageElementListByUsernameandPageUrl(String username, String pageUrl)
        {
            var pageElementList = new List<PageElement>();

            if (!String.IsNullOrEmpty(username))
            {
                var pageElements =
                    _db.PageElements.Where(m => m.PageSetup.User.UserName == username && m.PageSetup.PageURL == pageUrl).ToList();

                foreach (var pe in pageElements)
                {
                    pe.ReportType = _db.ReportTemplates.Single(m => m.ReportID == pe.ReportID.Value).ReportType;
                }


                if (pageElements.Any())
                {
                    pageElementList = pageElements.ToList();
                }

            }
            return pageElementList.ToList();
        }


        public List<PageElement> GetPageElementListByDashboardId(int dashboardId)
        {
            var pageElementList = new List<PageElement>();

            if (dashboardId > 0)
            {
                var pageElements =
                    _db.PageElements.Where(m => m.PageSetupID == dashboardId).ToList();

                foreach (var pe in pageElements)
                {
                    pe.ReportType = _db.ReportTemplates.Single(m => m.ReportID == pe.ReportID.Value).ReportType;
                }


                if (pageElements.Any())
                {
                    pageElementList = pageElements.ToList();
                }

            }
            return pageElementList.ToList();
        }


    }
}