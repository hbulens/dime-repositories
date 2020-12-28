using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.EF.Tests.Exceptions
{
    [TestClass]
    public class DatabaseAccessExceptionTests
    {
        [TestMethod]
        [TestCategory("Repository")]
        public void DatabaseAccessException_Constructor_Default_MapsCorrectly()
        {
            DatabaseAccessException exception = new DatabaseAccessException();
            Assert.IsTrue(exception != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void DatabaseAccessException_Constructor_Message_MapsCorrectly()
        {
            DatabaseAccessException exception = new DatabaseAccessException("Dime error");
            Assert.IsTrue(exception.Message == "Dime error");
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void DatabaseAccessException_Constructor_Message_Exception_MapsCorrectly()
        {
            DatabaseAccessException exception = new DatabaseAccessException("Dime error", new Exception("Dime inner error"));
            Assert.IsTrue(exception.Message == "Dime error");
            Assert.IsTrue(exception.InnerException.Message == "Dime inner error");
        }
    }
}