using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerTable : ITable
    {
        public string Name { get; set; }

        public IEnumerable<IColumn> getColumns()
        {
            throw new NotImplementedException();
        }

        public SQLServerTable(string name)
        {
            Name = name;
        }
    }
}
