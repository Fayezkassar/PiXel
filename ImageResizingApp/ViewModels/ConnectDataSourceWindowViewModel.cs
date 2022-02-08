using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
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


        public ConnectDataSourceWindowViewModel(DataSourceRegistry dataSourceRegistry)
        {
            _part1ViewModel = new ConnectDataSourcePart1ViewModel(dataSourceRegistry);
            _part2ViewModel = new ConnectDataSourcePart2ViewModel(dataSourceRegistry);
            CurrentViewModel = _part1ViewModel;

            ContinueCommand = new RelayCommand<Window>(onContinue, canContinue);
            PreviousCommand = new RelayCommand(onPrevious, canGoBack);
        }

        private void onContinue(Window connectWindow)
        {
            CurrentViewModel.Validate();
            if (!CurrentViewModel.HasErrors)
            {
                if (CurrentViewModel == _part1ViewModel)
                {
                    ContinueButtonContent = "Finish";
                    setDataSourceForPart2();
                    CurrentViewModel = _part2ViewModel;
                    PreviousCommand.NotifyCanExecuteChanged();

                }
                {
                    connectWindow.Close();
                    //_part2ViewModel.DataSource.Open();
                }
            }
        }
        private bool canContinue(Window connectWindow) => true;
        private void onPrevious() {
            ContinueButtonContent = "Next";
            CurrentViewModel = _part1ViewModel; 
    }
        private bool canGoBack() => _part2ViewModel == CurrentViewModel;
        private void setDataSourceForPart2()
        {
            _part2ViewModel.SetDataSourceFromKey(_part1ViewModel.SelectedDataSourceType);
            _part2ViewModel.DataSource.Name = _part1ViewModel.DataSourceName;
        }
    }
}
