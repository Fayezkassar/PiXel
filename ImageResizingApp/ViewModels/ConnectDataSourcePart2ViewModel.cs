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
        
        public IDataSource DataSource { get; set; }

        public ConnectDataSourcePart2ViewModel(Registry dataSourceRegistry, DataSourceStore dataSourceStore)
        {
            _connectionParameters = new ObservableCollection<ConnectionParameterViewModel>();
            _dataSourceRegistry = dataSourceRegistry;
            _dataSourceStore = dataSourceStore;
        }

        public void SetDataSourceFromKey(string key)
        {
            DataSource = _dataSourceRegistry.GetDataSourceFromKey(key);
        }

        public void UpdateConnectionParameters()
        {
            _connectionParameters.Clear();
            foreach (string param in DataSource.ConnectionParameters)
            {
                _connectionParameters.Add(new ConnectionParameterViewModel(param));
            }
        }
        public bool ConnectAndStoreDataSource()
        {
            Dictionary<string, string> connectionParametersMap = new Dictionary<string, string>();
            foreach (string paramName in DataSource.ConnectionParameters)
            {
                ConnectionParameterViewModel param = ConnectionParameters.First(e => e.DisplayName == paramName);
                connectionParametersMap.Add(paramName, param.Value);
            }

            return _dataSourceStore.OpenDataSourceConnection(DataSource, connectionParametersMap);
        }
        public void SetPassword(string password)
        {
            ConnectionParameters.First(e=>e.IsPassword).Value = password;
        }
    }
}
