using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.MVVM.Model
{
    public interface IColumn
    {
        public String Name { get; set; }
        public ColumnType ColumnType { get; set; }

    }
}
