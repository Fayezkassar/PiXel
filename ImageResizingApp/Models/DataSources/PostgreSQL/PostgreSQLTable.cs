using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
{
    public class PostgreSQLTable : ITable
    {
        public string Name { get; set; }

        public IEnumerable<IColumn> getColumns()
        {
            throw new NotImplementedException();
        }
    }
}
