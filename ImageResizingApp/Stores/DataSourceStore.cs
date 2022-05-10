using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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
            if (connected) { DataSource = dataSource; }
            else
            {
                DataSource = null;
            }
            return connected;
        }

        public IEnumerable<ITable> GetTables()
        {
            return DataSource?.Tables ?? new List<ITable>();
        }

        public async Task UpdateTableInfoAsync(ITable table)
        {
            await table.SetStatsAsync();
            if (table.PrimaryKeys==null || table.PrimaryKeys.Count() == 0)
            {
                await table.SetPrimaryKeysAsync();
            }
        }

        public async Task<IEnumerable<IColumn>> GetTableColumns(ITable table)
        {
            return await table.GetColumnsAsync();
        }

        public void CloseDataSourceConnectionIfAny()
        {
            DataSource?.Close();
        }

        public async Task<DataTable> GetTableDataAsync(ITable table, int start, int itemCount)
        {
            return await table.GetDataAsync(start, itemCount);

        }
    }
}
