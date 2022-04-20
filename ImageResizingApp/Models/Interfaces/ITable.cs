using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;

namespace ImageResizingApp.Models.Interfaces
{
    public interface ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }
        public Task<IEnumerable<IColumn>> GetColumnsAsync();
        public Task SetStatsAsync();
        public Task SetPrimaryKeysAsync();
        public Task<DataTable> GetDataAsync(int start, int itemCount);

    }
}
