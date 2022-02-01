using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IDataSource
    {
        public String Name { get; set; }

        public List<String> ConnectionParameters { get; set; }

        public List<ITable> Tables { get; set; }

        public bool open(Dictionary<String,String> connectionParametersMap);

        public bool close();
        public IDataSource clone();
    }
}
