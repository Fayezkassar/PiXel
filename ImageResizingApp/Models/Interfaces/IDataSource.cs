using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IDataSource
    {
        public string Name { get; set; }

        public List<string> ConnectionParameters { get; set; }

        public List<ITable> Tables { get; set; }

        public bool Open(Dictionary<string,string> connectionParametersMap);

        public bool Close();
        public IDataSource Clone();
    }
}
