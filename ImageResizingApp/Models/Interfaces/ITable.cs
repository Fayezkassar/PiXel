using System;
using System.Collections.Generic;

namespace ImageResizingApp.Models.Interfaces
{
    public interface ITable
    {
        public string Name { get; set; }

        public List<IColumn> Columns { get; set; }
    }
}
