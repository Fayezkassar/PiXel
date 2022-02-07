using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Oracle
{
    public class SQLServerTable : ITable
    {
        public string Name { get; set; }
        public List<IColumn> Columns { get; set; }
    }
}
