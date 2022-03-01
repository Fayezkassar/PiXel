using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Interfaces
{
    public interface ITable
    {
        public string Name { get; set; }

        public IEnumerable<IColumn> getColumns();
        public TableStats GetStats();
    }
}
