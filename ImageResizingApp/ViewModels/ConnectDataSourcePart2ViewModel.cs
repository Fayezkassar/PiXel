using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourcePart2ViewModel : ViewModelBase
    {
        private readonly Registry _dataSourceRegistry;
        private readonly DataSourceStore _dataSourceStore;
       
        private readonly ObservableCollection<ConnectionParameterViewModel> _connectionParameters;
        public IEnumerable<ConnectionParameterViewModel> ConnectionParameters => _connectionParameters;
        
        public IDataSource _dataSource { get; set; }

        public ConnectDataSourcePart2ViewModel(Registry dataSourceRegistry, DataSourceStore dataSourceStore)
        {
            _connectionParameters = new ObservableCollection<ConnectionParameterViewModel>();
            _dataSourceRegistry = dataSourceRegistry;
            _dataSourceStore = dataSourceStore;
        }

        public void SetDataSourceFromKey(string key)
        {
            _dataSource = _dataSourceRegistry.GetDataSourceFromKey(key);
        }

        //public void SetDataSourceName(string name)
        //{
        //    _dataSource.Name = name;
        //}

        public void UpdateConnectionParameters()
        {
            _connectionParameters.Clear();
            foreach (string param in _dataSource.ConnectionParameters)
            {
                _connectionParameters.Add(new ConnectionParameterViewModel(param));
            }
        }
        public bool ConnectAndStoreDataSource()
        {
            Dictionary<string, string> connectionParametersMap = new Dictionary<string, string>();
            foreach (string paramName in _dataSource.ConnectionParameters)
            {
                ConnectionParameterViewModel param = ConnectionParameters.First(e => e.DisplayName == paramName);
                if (param == null) return false;
                connectionParametersMap.Add(paramName, param.Value);
            }

            return _dataSourceStore.OpenDataSourceConnection(_dataSource, connectionParametersMap);
        }
        public void SetPassword(string password)
        {
            ConnectionParameters.First(e=>e.IsPassword).Value = password;
        }
    }
}
