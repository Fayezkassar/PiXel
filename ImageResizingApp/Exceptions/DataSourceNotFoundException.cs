using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Exceptions
{
    public class DataSourceNotFoundException: Exception
    {
        public DataSourceNotFoundException(string key): base("No data source found for key: " + key + ".") {}
    }
}
