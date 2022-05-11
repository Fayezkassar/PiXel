using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IDataSource
    {
        public IEnumerable<string> ConnectionParameters { get; set; }
        public IEnumerable<ITable> Tables { get; set; }
        public bool Open(Dictionary<string,string> connectionParametersMap);
        public bool Close();
        public IDataSource Clone();
        public Task SetTablesAsync();
    }
}
