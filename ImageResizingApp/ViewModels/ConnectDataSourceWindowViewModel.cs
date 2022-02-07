using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourceWindowViewModel : ViewModelBase
    {
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value, false);
            }
        }
        public ViewModelBase _part1ViewModel { get; }
        public ViewModelBase _part2ViewModel { get; }

        public ConnectDataSourceWindowViewModel(DataSourceRegistry dataSourceRegistry)
        {
            _part1ViewModel = new ConnectDataSourcePart1ViewModel(dataSourceRegistry);
            _part2ViewModel = new ConnectDataSourcePart2ViewModel();
            CurrentViewModel = _part1ViewModel;

            NextCommand = new RelayCommand(onNext, canContinue);
            PreviousCommand = new RelayCommand(onPrevious, canGoBack);
        }

        private void onNext() {
            CurrentViewModel.Validate();
            if (!CurrentViewModel.HasErrors)
            {
                CurrentViewModel = _part2ViewModel;
            }
        }
        private bool canContinue() => CurrentViewModel == _part1ViewModel;
        private void onPrevious() => Console.WriteLine("Getting Started");
        private bool canGoBack() => _part2ViewModel == CurrentViewModel;
    }
}
