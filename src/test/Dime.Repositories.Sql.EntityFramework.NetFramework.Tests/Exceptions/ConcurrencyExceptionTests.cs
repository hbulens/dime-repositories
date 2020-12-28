using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.EF.Tests.Exceptions
{
    [TestClass]
    public class ConcurrencyExceptionTests
    {
        [TestMethod]
        [TestCategory("Repository")]
        public void ConcurrencyException_Constructor_Default_MapsCorrectly()
        {
            ConcurrencyException exception = new ConcurrencyException();
            Assert.IsTrue(exception != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void ConcurrencyException_Constructor_Message_MapsCorrectly()
        {
            ConcurrencyException exception = new ConcurrencyException("Dime error");
            Assert.IsTrue(exception.Message == "Dime error");
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void ConcurrencyException_Constructor_Message_Exception_MapsCorrectly()
        {
            ConcurrencyException exception = new ConcurrencyException("Dime error", new Exception("Dime inner error"));
            Assert.IsTrue(exception.Message == "Dime error");
            Assert.IsTrue(exception.InnerException.Message == "Dime inner error");
        }
    }
}