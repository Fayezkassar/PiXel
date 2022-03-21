using System.Collections.Generic;
using System.Data;

namespace ImageResizingApp.Models.Interfaces
{
    public interface ITable
    {
        public string Name { get; set; }
        public string TableSize { get; set; }
        public string RecordsNumber { get; set; }
        public string RecordSize { get; set; }
        public IEnumerable<string> PrimaryKeys { get; set; }
        public IEnumerable<IColumn> GetColumns();
        public void SetStats();
        public DataTable GetData(int start, int itemCount);
        public void SetPrimaryKeys();

    }
}
