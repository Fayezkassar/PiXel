using ImageResizingApp.Models.DataSources.Oracle;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using ImageResizingApp.ViewModels;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.ViewModels
{
    [TestFixture]
    public class ConnectDataSourceWindowViewModelTests
    {
        private DataSourceStore _dataSourceStore;
        private Registry _registry;
        private ConnectDataSourceWindowViewModel _connectDataSourceWindowViewModel;

        [SetUp]
        public void SetUp()
        {
            _registry = new Registry();
            _registry.AddDataSource("Oracle", new OracleDataSource());
            _registry.AddDataSource("SQL Server", new OracleDataSource());

            _dataSourceStore = new DataSourceStore();

            _connectDataSourceWindowViewModel = new ConnectDataSourceWindowViewModel(_registry, _dataSourceStore);
            ((ConnectDataSourcePart1ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).SelectedDataSourceType = "Oracle";
        }
        [Test]
        public void OnContinuePart1_WithEmptyPart1Fields_ShowsErrorsAndReturns()
        {
            ConnectDataSourceWindowViewModel connectDataSourceWindowViewModel = new ConnectDataSourceWindowViewModel(_registry, _dataSourceStore);
            connectDataSourceWindowViewModel.ContinueCommand.Execute(null);

            Assert.IsTrue(connectDataSourceWindowViewModel.CurrentViewModel.HasErrors);
            Assert.IsInstanceOf(typeof(ConnectDataSourcePart1ViewModel),connectDataSourceWindowViewModel.CurrentViewModel);
            Assert.AreEqual(connectDataSourceWindowViewModel.ContinueButtonContent, "Next");
        }

        [Test]
        public void OnContinuePart1_WithFilledPart1Fields_PassesToPart2CreatesDataSourceAndReturns()
        {
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);

            Assert.AreEqual(_connectDataSourceWindowViewModel.ContinueButtonContent, "Finish");
            Assert.IsInstanceOf(typeof(ConnectDataSourcePart2ViewModel), _connectDataSourceWindowViewModel.CurrentViewModel);
            Assert.IsInstanceOf(typeof(OracleDataSource), ((ConnectDataSourcePart2ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).DataSource);
        }

        [Test]
        public void OnContinuePart1_AfterGoingBackFromPart2WitoutChangingFields_PassesToPart2WithoutChangingDataSourceAndReturns()
        {
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            IDataSource dataSource = ((ConnectDataSourcePart2ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).DataSource;

            _connectDataSourceWindowViewModel.PreviousCommand.Execute(null);
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            Assert.AreEqual(dataSource, ((ConnectDataSourcePart2ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).DataSource);
        }

        [Test]
        public void OnContinuePart1_AfterGoingBackFromPart2WithChangingFields_PassesToPart2WithNewDataSourceAndReturns()
        {
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            IDataSource dataSource = ((ConnectDataSourcePart2ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).DataSource;

            _connectDataSourceWindowViewModel.PreviousCommand.Execute(null);
            ((ConnectDataSourcePart1ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).SelectedDataSourceType = "SQL Server";

            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            Assert.AreNotEqual(dataSource, ((ConnectDataSourcePart2ViewModel)(_connectDataSourceWindowViewModel.CurrentViewModel)).DataSource);
        }

        [Test]
        public void OnContinuePart2_WithEmptyPart1Fields_ShowsErrorsAndReturns()
        {
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            Assert.IsInstanceOf(typeof(ConnectDataSourcePart2ViewModel), _connectDataSourceWindowViewModel.CurrentViewModel);
            Assert.NotZero(((ConnectDataSourcePart2ViewModel)_connectDataSourceWindowViewModel.CurrentViewModel).ConnectionParameters.Count(e=>e.HasErrors));
        }

        [Test]
        public void OnPrevious_ClearsPasswordAndSetCurrentViewModelToPart1()
        {
            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            _connectDataSourceWindowViewModel.PreviousCommand.Execute(null);
            Assert.AreEqual(_connectDataSourceWindowViewModel.ContinueButtonContent, "Next");
            Assert.IsInstanceOf(typeof(ConnectDataSourcePart1ViewModel), _connectDataSourceWindowViewModel.CurrentViewModel);

            _connectDataSourceWindowViewModel.ContinueCommand.Execute(null);
            Assert.IsEmpty(((ConnectDataSourcePart2ViewModel)_connectDataSourceWindowViewModel.CurrentViewModel).ConnectionParameters.First(e => e.IsPassword).Value);
        }
    }
}
