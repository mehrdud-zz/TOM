using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Claims.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mock = new Mock<Factories.IClientFactory>();
            mock.Setup(r => r.GetClient("Diageo")).Returns(new ModelsLayer.Client() { Name = "Diageo" });
            ModelsLayer.Client diageoClient = mock.Object.GetClient("Diageo");
            Assert.AreEqual(diageoClient.Name, "Diageo");
        }
    }
}
