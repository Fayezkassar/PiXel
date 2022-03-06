using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerTable : ITable
    {
        public string Name { get; set; }

        public string SchemaName { get; set; }

        public IEnumerable<IColumn> getColumns()
        {
            throw new NotImplementedException();
        }

        public SQLServerTable(string name)
        {
            Name = name;
        }

        public TableStats GetStats()
        {
            throw new NotImplementedException();
        }

        public DataTable getData()
        {
            throw new NotImplementedException();
        }
    }
}
