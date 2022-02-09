using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class TableListingViewModel : ViewModelBase
    {
        private readonly DataSourceStore _dataSourceStore;

        private readonly ObservableCollection<ITable> _tables;

        public IEnumerable<ITable> Tables => _tables;

        public TableListingViewModel(DataSourceStore dataSourceStore) 
        {
            _dataSourceStore = dataSourceStore;

            _tables = new ObservableCollection<ITable>(_dataSourceStore.getTables());
        }


    
    }
       
}
