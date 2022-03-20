using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Stores;
using System.Windows;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourceWindowViewModel : ViewModelBase
    {
        public RelayCommand<Window> ContinueCommand { get; }
        public RelayCommand PreviousCommand { get; }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value, false);
            }
        }

        private ConnectDataSourcePart1ViewModel _part1ViewModel { get; }
        private ConnectDataSourcePart2ViewModel _part2ViewModel { get; }

        private string _continueButtonContent = "Next";
        public string ContinueButtonContent
        {
            get { return _continueButtonContent; }
            set { SetProperty(ref _continueButtonContent, value, false); }
        }

        private string _selectedDataSourceType { get; set; }

        public ConnectDataSourceWindowViewModel(DataSourceRegistry dataSourceRegistry, DataSourceStore dataSourceStore)
        {
            _part1ViewModel = new ConnectDataSourcePart1ViewModel(dataSourceRegistry);
            _part2ViewModel = new ConnectDataSourcePart2ViewModel(dataSourceRegistry, dataSourceStore);
            CurrentViewModel = _part1ViewModel;

            ContinueCommand = new RelayCommand<Window>(OnContinue);
            PreviousCommand = new RelayCommand(OnPrevious, CanGoBack);
        }

        private void OnContinue(Window connectWindow)
        {
            CurrentViewModel.Validate();
            if (!CurrentViewModel.HasErrors)
            {
                if (CurrentViewModel == _part1ViewModel)
                {
                    ContinueButtonContent = "Finish";
                    if(_selectedDataSourceType != _part1ViewModel.SelectedDataSourceType)
                    {
                        _selectedDataSourceType = _part1ViewModel.SelectedDataSourceType;
                        _part2ViewModel.SetDataSourceFromKey(_part1ViewModel.SelectedDataSourceType);
                        _part2ViewModel.UpdateConnectionParameters();
                    }
                    _part2ViewModel.SetDataSourceName(_part1ViewModel.DataSourceName);
                    CurrentViewModel = _part2ViewModel;
                    PreviousCommand.NotifyCanExecuteChanged();
                }
                else
                {
                    if (!ValidatePart2ConnectionParameters()) return;
                    if (_part2ViewModel.ConnectAndStoreDataSource())
                    {
                        //dialogService: Dispose() the view model
                        connectWindow.Close();
                    }
                }
            }
        }
        private void OnPrevious() {
            ContinueButtonContent = "Next";
            CurrentViewModel = _part1ViewModel;
            _part2ViewModel.SetPassword("");
        }
        private bool CanGoBack() => CurrentViewModel == _part2ViewModel;

        private bool ValidatePart2ConnectionParameters()
        {
            bool valid = true;
            foreach (ConnectionParameterViewModel param in _part2ViewModel.ConnectionParameters)
            {
                param.Validate();
                if (param.HasErrors) valid=false;
            }
            return valid;
        }
    }
}
