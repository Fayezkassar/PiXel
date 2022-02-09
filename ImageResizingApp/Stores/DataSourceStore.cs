using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizingApp.Stores
{
    public class DataSourceStore
    {
        private IDataSource _dataSource;

        public bool OpenDataSourceConnection(IDataSource dataSource, Dictionary<string, string> connectionParametersMap)
        {
            bool connected = dataSource.Open(connectionParametersMap);
            if (connected) _dataSource = dataSource;
            return connected;
        }

        public IEnumerable<ITable> getTables()
        {
            return _dataSource?.Tables ?? new List<ITable>();
        }

        public void CloseDataSourceConnectionIfAny()
        {
            if(_dataSource != null) _dataSource.Close();
        }
    }
}
