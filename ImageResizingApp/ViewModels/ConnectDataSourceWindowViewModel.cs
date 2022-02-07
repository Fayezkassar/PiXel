using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourceWindowViewModel: ViewModelBase
    {
        ICommand NextCommand { get; }

        public ViewModelBase CurrentViewModel { get; }

        public ConnectDataSourceWindowViewModel(DataSourceRegistry dataSourceRegistry)
        {
            CurrentViewModel = new ConnectDataSourcePart1ViewModel(dataSourceRegistry);
            NextCommand = new RelayCommand(onNext, canContinue);
        }

        private void onNext() => Console.WriteLine("Getting Started");
        private Boolean canContinue() => true;
    }
}
