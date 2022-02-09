using ImageResizingApp.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainWindowViewModel(DataSourceStore dataSourceStore)
        {
            CurrentViewModel = new TablesViewModel(dataSourceStore);
        }
    }
}
