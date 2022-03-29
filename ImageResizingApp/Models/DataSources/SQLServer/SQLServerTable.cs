using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerTable : ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }
        public IEnumerable<IColumn> GetColumns()
        {
            throw new NotImplementedException();
        }

        public SQLServerTable(string name)
        {
            Name = name;
        }

        public void SetStats()
        {
            throw new NotImplementedException();
        }

        public DataTable GetData()
        {
            throw new NotImplementedException();
        }

        public void SetPrimaryKeys()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<IColumn>> ITable.GetColumnsAsync()
        {
            throw new NotImplementedException();
        }

        Task ITable.SetStatsAsync()
        {
            throw new NotImplementedException();
        }

        Task ITable.SetPrimaryKeysAsync()
        {
            throw new NotImplementedException();
        }

        Task<DataTable> ITable.GetDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
