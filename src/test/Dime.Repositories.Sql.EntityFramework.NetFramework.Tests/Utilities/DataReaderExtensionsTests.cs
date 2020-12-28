using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dime.Repositories.EF.Tests
{
    [TestClass]
    public class DataReaderExtensionsTests
    {
        private class MyClass
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }

        private const string Column1 = "FirstName";
        private const string Column2 = "MiddleName";
        private const string Column3 = "LastName";
        private const string ExpectedValue1 = "Handsome";
        private const string ExpectedValue2 = "B.";
        private const string ExpectedValue3 = "Wonderful";

        private static Mock<DbDataReader> CreateDataReader()
        {
            Mock<DbDataReader> dataReader = new Mock<DbDataReader>();

            dataReader.Setup(m => m["FirstName"]).Returns(ExpectedValue1);
            dataReader.Setup(m => m["MiddleName"]).Returns(ExpectedValue2);
            dataReader.Setup(m => m["LastName"]).Returns(ExpectedValue3);

            dataReader.Setup(m => m.FieldCount).Returns(3);
            dataReader.Setup(m => m.GetName(0)).Returns(Column1);
            dataReader.Setup(m => m.GetName(1)).Returns(Column2);
            dataReader.Setup(m => m.GetName(2)).Returns(Column3);

            dataReader.Setup(m => m.GetFieldType(0)).Returns(typeof(string));
            dataReader.Setup(m => m.GetFieldType(1)).Returns(typeof(string));
            dataReader.Setup(m => m.GetFieldType(2)).Returns(typeof(string));

            dataReader.Setup(m => m.GetOrdinal("FirstName")).Returns(0);
            dataReader.Setup(m => m.GetValue(0)).Returns(ExpectedValue1);
            dataReader.Setup(m => m.GetValue(1)).Returns(ExpectedValue2);
            dataReader.Setup(m => m.GetValue(2)).Returns(ExpectedValue3);

            dataReader.SetupSequence(m => m.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            return dataReader;
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void DataReaderExtensions_GetRecords_MapsToClass()
        {
            using IDataReader reader = CreateDataReader().Object;
            IEnumerable<MyClass> items = reader.GetRecords<MyClass>();

            MyClass firstItem = items.ElementAt(0);
            Assert.IsTrue(firstItem.FirstName == "Handsome");
            Assert.IsTrue(firstItem.MiddleName == "B.");
            Assert.IsTrue(firstItem.LastName == "Wonderful");
        }
    }
}