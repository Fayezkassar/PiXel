using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.Interfaces
{
    public interface ITable
    {
        public String Name { get; set; }

        public List<IColumn> Columns { get; set; }
    }
}
