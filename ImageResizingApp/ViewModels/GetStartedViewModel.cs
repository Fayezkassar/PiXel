using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ImageResizingApp.ViewModels
{
    public class GetStartedViewModel: ViewModelBase
    {
        ICommand GetStartedCommand { get; }

        public GetStartedViewModel()
        {
            GetStartedCommand = new RelayCommand(GetStarted);
        }

        private void GetStarted() => Console.WriteLine("Getting Started");
    }
}
