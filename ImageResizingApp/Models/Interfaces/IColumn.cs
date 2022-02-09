using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IColumn
    {
        public string Name { get; set; }

        public ColumnType ColumnType { get; set; }

    }
}
