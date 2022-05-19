using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;

namespace ImageResizingApp.Models.Interfaces
{
    /*! ITable Interface */
    public interface ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }
        public IEnumerable<IColumn> Columns { get; set; }
        public IDataSource DataSource { get; set; }
        public Task SetColumnsAsync();
        public Task SetStatsAsync();
        public Task SetPrimaryKeysAsync();
        public Task<DataTable> GetDataAsync(int start, int itemCount);

    }
}
