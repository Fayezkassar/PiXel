using System;
using System.Collections.Generic;

namespace ImageResizingApp.MVVM.Model
{
    public interface ITable
    {
        public String Name { get; set; }

        public List<IColumn> Columns { get; set; }
    }
}
