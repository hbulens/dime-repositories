using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.EF.Tests.Exceptions
{
    [TestClass]
    public class ConstraintViolationExceptionTests
    {
        [TestMethod]
        [TestCategory("Repository")]
        public void ConstraintViolationException_Constructor_Default_MapsCorrectly()
        {
            ConstraintViolationException exception = new ConstraintViolationException();
            Assert.IsTrue(exception != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void ConstraintViolationException_Constructor_Message_MapsCorrectly()
        {
            ConstraintViolationException exception = new ConstraintViolationException("Dime error");
            Assert.IsTrue(exception.Message == "Dime error");
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void ConstraintViolationException_Constructor_Message_Exception_MapsCorrectly()
        {
            ConstraintViolationException exception = new ConstraintViolationException("Dime error", new Exception("Dime inner error"));
            Assert.IsTrue(exception.Message == "Dime error");
            Assert.IsTrue(exception.InnerException.Message == "Dime inner error");
        }
    }
}