using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourcePart2ViewModel : ViewModelBase
    {
        private IDataSource _dataSource;
        public IDataSource DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                SetProperty(ref _dataSource, value, false);
            }
        }

        private DataSourceRegistry _dataSourceRegistry;
        public ConnectDataSourcePart2ViewModel(DataSourceRegistry dataSourceRegistry)
        {
            _dataSourceRegistry = dataSourceRegistry;
        }

        public void SetDataSourceFromKey(string key)
        {
            DataSource = _dataSourceRegistry.getDataSourceFromKey(key);
        }
    }
}
