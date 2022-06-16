using ImageResizingApp.Models.Filters;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.ViewModels;
using Moq;
using NUnit.Framework;
using System.Linq;


namespace UnitTests.ViewModels
{
    [TestFixture]
    public class ResizeConfigurationWindowViewModelTests
    {
        private Mock<IColumn> _mockColumn;
        private Registry _registry;
        private ResizeConfigurationWindowViewModel _columnResizeConfigurationWindowViewModel;
        //private ResizeConfigurationWindowViewModel _imageResizeConfigurationWindowViewModel;

        [SetUp]
        public void SetUp()
        {
            _registry = new Registry();
            _registry.AddFilter("Scale", new ScaleFilter("Scale"));

            _mockColumn = new Mock<IColumn>();

            _columnResizeConfigurationWindowViewModel = new ResizeConfigurationWindowViewModel(_mockColumn.Object, _registry);
            //_imageResizeConfigurationWindowViewModel = new ResizeConfigurationWindowViewModel(_mockColumn.Object.GetImageForPrimaryKeysValues(), _registry);
        }

        [Test]
        public void OnConfirmPart1_WithEmptyPart1Fields_PassesToPart2AndReturns()
        {
            _columnResizeConfigurationWindowViewModel.ConfirmCommand.Execute(null);

            Assert.AreEqual(_columnResizeConfigurationWindowViewModel.ConfirmButtonContent, "Confirm");
            Assert.IsInstanceOf(typeof(ResizeConfigurationPart2ViewModel), _columnResizeConfigurationWindowViewModel.CurrentViewModel);
            Assert.IsFalse(_columnResizeConfigurationWindowViewModel.CurrentViewModel.HasErrors);
        }

        [Test]
        public void OnConfirmPart1_WithFilledPart1Fields_PassesToPart2AndReturns()
        {

            ((ResizeConfigurationPart1ViewModel)_columnResizeConfigurationWindowViewModel.CurrentViewModel).MinSize = 1;
            ((ResizeConfigurationPart1ViewModel)_columnResizeConfigurationWindowViewModel.CurrentViewModel).MaxSize = 100000;
            ((ResizeConfigurationPart1ViewModel)_columnResizeConfigurationWindowViewModel.CurrentViewModel).From = 0;
            ((ResizeConfigurationPart1ViewModel)_columnResizeConfigurationWindowViewModel.CurrentViewModel).To = 10000;
            ((ResizeConfigurationPart1ViewModel)_columnResizeConfigurationWindowViewModel.CurrentViewModel).BackupDestination = "test dest";

            _columnResizeConfigurationWindowViewModel.ConfirmCommand.Execute(null);

            Assert.AreEqual(_columnResizeConfigurationWindowViewModel.ConfirmButtonContent, "Confirm");
            Assert.IsInstanceOf(typeof(ResizeConfigurationPart2ViewModel), _columnResizeConfigurationWindowViewModel.CurrentViewModel);
            Assert.IsFalse(_columnResizeConfigurationWindowViewModel.CurrentViewModel.HasErrors);
        }

        [Test]
        public void OnConfirmPart2_WithInvalidFilters_ShowsErrorsAndReturns()
        {
            _columnResizeConfigurationWindowViewModel.ConfirmCommand.Execute(null);
            ((ResizeConfigurationPart2ViewModel)(_columnResizeConfigurationWindowViewModel.CurrentViewModel)).SelectFilter(new FilterViewModel(_registry.GetFilterFromKey("Scale")));
            _columnResizeConfigurationWindowViewModel.ConfirmCommand.Execute(null);

            Assert.IsInstanceOf(typeof(ResizeConfigurationPart2ViewModel), _columnResizeConfigurationWindowViewModel.CurrentViewModel);
            Assert.NotZero(((ResizeConfigurationPart2ViewModel)_columnResizeConfigurationWindowViewModel.CurrentViewModel).SelectedFilters.Count(e => e.HasErrors));
        }

        [Test]
        public void OnConfirmPart2_WithValidFilters_ResizesAndPassesToPart2()
        {
            _mockColumn.SetReturnsDefault(true);

            _columnResizeConfigurationWindowViewModel.ConfirmCommand.Execute(null);

            FilterViewModel filterViewModel = new FilterViewModel(_registry.GetFilterFromKey("Scale"));
            filterViewModel.Height = 1000;
            filterViewModel.Width = 1000;   
            ((ResizeConfigurationPart2ViewModel)(_columnResizeConfigurationWindowViewModel.CurrentViewModel)).SelectFilter(filterViewModel);
            _columnResizeConfigurationWindowViewModel.ConfirmCommand.Execute(null);

            Assert.IsInstanceOf(typeof(ResizeConfigurationPart3ViewModel), _columnResizeConfigurationWindowViewModel.CurrentViewModel);
        }
    }
}
