using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using ModelsLayer;
using System.Linq;
using System.Data.Entity;
using System.Configuration; 

namespace Claims.Tests
{
    [TestClass]
    public class DashboardUnitTests
    {


        [TestMethod]

        public void GetAllDashboards()
        {
            var mock = new Mock<Factories.IDashboardFactory>();
            var reports = mock.Setup(r => r.GetDashboards());
        }


        

        [TestMethod]

        public void GetAllDashboards_II()
        {  
            
            var service = new Factories.PageSetupFactory();
            var pageSetups = service.GetPageSetups();

            Assert.AreNotEqual(pageSetups, null);
        }


        //[TestMethod]

        //public void CreateDashboard()
        //{
        //    var dashboardFactoryMock = new Mock<Factories.IDashboardFactory>();
        //    var report = mock.Setup(r => r.GetDashboard());
        //}

    }
}
