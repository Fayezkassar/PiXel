using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Stores
{
    [TestFixture]
    public class DataSourceStoreTests
    {
        private Mock<IDataSource> _mockDataSource;
        private DataSourceStore _dataSourceStore;

        [SetUp]
        public void SetUp()
        {
            _dataSourceStore = new DataSourceStore();
            _mockDataSource = new Mock<IDataSource>();
        }

        [Test]
        public void OpenDataSourceConnection_WithCorrectConnectionParameters_ReturnsTrueAndSetsDataSource()
        {
            IDataSource dataSource = _mockDataSource.Object;
            Dictionary<string, string> expectedConnectionParametersMap = new Dictionary<string, string>();
            expectedConnectionParametersMap.Add("host", "192.168.222.1");
            expectedConnectionParametersMap.Add("username", "testusername");
            expectedConnectionParametersMap.Add("password", "testpassword");
            expectedConnectionParametersMap.Add("port", "5001");

            _mockDataSource.Setup(s=> s.Open(expectedConnectionParametersMap)).Returns(true);
            Assert.IsTrue(_dataSourceStore.OpenDataSourceConnection(dataSource, expectedConnectionParametersMap));
            Assert.AreEqual(_dataSourceStore.DataSource, dataSource);
        }

        [Test]
        public void OpenDataSourceConnection_WithIncorrectConnectionParameters_ReturnsFalseAndClearsDataSource()
        {
            Dictionary<string, string> invalidConnectionParametersMap = new Dictionary<string, string>();
            invalidConnectionParametersMap.Add("host", "192.168.222.1");
            invalidConnectionParametersMap.Add("username", "inv");
            _mockDataSource.Setup(s => s.Open(invalidConnectionParametersMap)).Returns(false);
            Assert.IsFalse(_dataSourceStore.OpenDataSourceConnection(_mockDataSource.Object, invalidConnectionParametersMap));
            Assert.IsNull(_dataSourceStore.DataSource);
        }
    }
}
