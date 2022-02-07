using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourcePart1ViewModel : ViewModelBase
    {
        private DataSourceRegistry _dataSourceRegistry;

        public List<string> DataSourceNames;

        private string _dataSourceName;
        public string DataSourceName
        {
            get
            {
                return _dataSourceName;
            }
            set
            {
                _dataSourceName = value;
                OnPropertyChanged();
            }
        }
        public ConnectDataSourcePart1ViewModel(DataSourceRegistry dataSourceRegistry)
        {
            _dataSourceRegistry = dataSourceRegistry;
            setDataSourceNamesFromRegistry();
        }
        private void setDataSourceNamesFromRegistry()
        {
            DataSourceNames = _dataSourceRegistry.GetDataSourceNames();
        }
    }
}
