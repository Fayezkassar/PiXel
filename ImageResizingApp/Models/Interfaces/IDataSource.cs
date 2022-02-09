using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IDataSource
    {
        public string Name { get; set; }

        public IEnumerable<string> ConnectionParameters { get; }

        public List<ITable> Tables { get; set; }

        public Task<IEnumerable<ITable>> getTables();

        public bool Open(Dictionary<string,string> connectionParametersMap);

        public bool Close();
        public IDataSource Clone();
    }
}
