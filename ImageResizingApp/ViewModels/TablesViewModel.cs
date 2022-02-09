using Caliburn.Micro;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Models.PostgreSQL;
using ImageResizingApp.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    internal class TablesViewModel : ViewModelBase
    {
        private readonly DataSourceStore _dataSourceStore;

        public BindableCollection<ITable> lvDataBinding { get; set; }
       

        public TablesViewModel(DataSourceStore dataSourceStore) 
        {
            _dataSourceStore = dataSourceStore;

            lvDataBinding = new BindableCollection<ITable>(_dataSourceStore.getTables());
        }


    
    }
       
}
