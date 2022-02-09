using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Stores;
using ImageResizingApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        public ConnectDataSourcePart1ViewModel _part1ViewModel { get; }
        public ConnectDataSourcePart2ViewModel _part2ViewModel { get; }

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

            ContinueCommand = new RelayCommand<Window>(OnContinue, CanContinue);
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
                    bool errors = false;
                    foreach (ConnectionParameterViewModel param in _part2ViewModel.ConnectionParameters)
                    {
                        param.Validate();
                        if (param.HasErrors) errors = true;
                    }
                    if (errors) return;
                    if (_part2ViewModel.ConnectAndStoreDataSource())
                    {
                        //dialogService: Dispose() the view model
                        connectWindow.Close();
                    }
                }
            }
        }
        private bool CanContinue(Window connectWindow) => true;
        private void OnPrevious() {
            ContinueButtonContent = "Next";
            CurrentViewModel = _part1ViewModel; 
        }
        private bool CanGoBack() => CurrentViewModel == _part2ViewModel;
    }
}
