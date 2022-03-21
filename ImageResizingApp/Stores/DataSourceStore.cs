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

        public IDataSource DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource?.Close();
                _dataSource = value;
                OnCurrentDataSourceChanged();
            }
        }

        public event Action CurrentDataSourceChanged;

        private void OnCurrentDataSourceChanged()
        {
            CurrentDataSourceChanged?.Invoke();
        }

        public bool OpenDataSourceConnection(IDataSource dataSource, Dictionary<string, string> connectionParametersMap)
        {
            bool connected = dataSource.Open(connectionParametersMap);
            if (connected) DataSource = dataSource;
            return connected;
        }

        public IEnumerable<ITable> GetTables()
        {
            return DataSource?.Tables ?? new List<ITable>();
        }

        public ITable GetUpdatedTableByName(string tableName)
        {
            ITable table = DataSource?.Tables?.First(t => t.Name.Equals(tableName));
            table?.SetStats();
            table?.SetPrimaryKeys();
            return table;
        }

        public IEnumerable<IColumn> GetColumnsByTableName(string tableName)
        {
            ITable table = DataSource?.Tables?.First(t => t.Name.Equals(tableName));
            return table?.GetColumns();
        }

        public void CloseDataSourceConnectionIfAny()
        {
            DataSource?.Close();
        }

        internal DataTable GetDataByTableName(string tableName, int start, int itemCount)
        {
            ITable table = DataSource?.Tables?.First(t => t.Name.Equals(tableName));
            return table?.GetData(start, itemCount);
        }
    }
}
