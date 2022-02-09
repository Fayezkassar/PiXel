using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IDataSource
    {
        public string Name { get; set; }

        public IEnumerable<string> ConnectionParameters { get; }

        public IEnumerable<ITable> Tables { get; set; }

        public bool Open(Dictionary<string,string> connectionParametersMap);

        public bool Close();

        public IDataSource Clone();
    }
}
