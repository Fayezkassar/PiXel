using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Oracle
{
    public class SQLServerDataSource : IDataSource
    {
        public string Name { get; set; }
        public List<string> ConnectionParameters { get; set; }
        public List<ITable> Tables { get; set; }
        public IDataSource Clone()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool Open(Dictionary<string, string> connectionParametersMap)
        {
            throw new NotImplementedException();
        }
    }
}
