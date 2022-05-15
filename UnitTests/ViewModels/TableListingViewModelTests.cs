using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using ImageResizingApp.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.ViewModels
{
    [TestFixture]
    public class TableListingViewModelTests
    {
        private Mock<IDataSource> _mockDataSource;
        private DataSourceStore _dataSourceStore;
        private Registry _registry;
        private TableListingViewModel _tableListingViewModel;

        [SetUp]
        public void SetUp()
        {
            _registry = new Registry();
            _dataSourceStore = new DataSourceStore();
            _mockDataSource = new Mock<IDataSource>();
            _dataSourceStore.DataSource = _mockDataSource.Object;

            _tableListingViewModel = new TableListingViewModel(_dataSourceStore, _registry);
        }
        //pagination tests
    }
}
