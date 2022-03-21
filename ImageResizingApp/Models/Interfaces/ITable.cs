using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
        public void SetPrimaryKeys();
        public DataTable GetData();
    }
}
