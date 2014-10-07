using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Claims.Tests
{
    [TestClass]
    public class Sprint2
    {
     

        [TestMethod]

        public void TestDescrFieldExists()
        {
            var mock = new Mock<Factories.IReportFactory>();
            var reports = mock.Setup(r => r.GetReportTemplates());
        
        }
    }
}
