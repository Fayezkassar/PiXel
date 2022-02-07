using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.PostgreSQL
{
    public class PostgreSQLTable : ITable
    {
        public string Name { get ; set; }
        public List<IColumn> Columns { get; set; }
    }
}
