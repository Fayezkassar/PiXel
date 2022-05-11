using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
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
        public IEnumerable<IColumn> Columns { get; set; }

        public SQLServerTable(string name)
        {
            Name = name;
        }

        public Task<IEnumerable<IColumn>> SetColumnsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetStatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SetPrimaryKeysAsync()
        {
            throw new NotImplementedException();
        }

        Task ITable.SetColumnsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DataTable> GetDataAsync(int start, int itemCount)
        {
            throw new NotImplementedException();
        }
    }
}
