using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
{
    public class PostgresSQLColumn : IColumn
    {
        public string Name { get; set; }

        public ColumnType ColumnType { get; set; }
    }
}
