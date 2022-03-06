using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ImageResizingApp.Models;
using System.Data;

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

        public TableStats GetStatsByTableName(string tableName)
        {
            ITable table = _dataSource?.Tables?.First(t => t.Name.Equals(tableName));
            return table?.GetStats();
        }

        public IEnumerable<IColumn> GetColumnsStatsByTable(string tableName)
        {
            ITable table = _dataSource?.Tables?.First(t => t.Name.Equals(tableName));
            return table?.getColumns();
        }

        public void CloseDataSourceConnectionIfAny()
        {
            if(_dataSource != null) _dataSource.Close();
        }

        internal DataTable GetDataByTableName(string tableName)
        {
            ITable table = _dataSource?.Tables?.First(t => t.Name.Equals(tableName));
            return table?.getData();
        }
    }
}
