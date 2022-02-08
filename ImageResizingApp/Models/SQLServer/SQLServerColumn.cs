using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Oracle
{
    public class SQLServerColumn : IColumn
    {
        public string Name { get; set; }
        public ColumnType ColumnType { get; set; }
    }
    }
