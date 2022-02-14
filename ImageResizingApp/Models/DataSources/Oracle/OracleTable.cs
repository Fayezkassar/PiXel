using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleTable : ITable
    {
        public string Name { get; set; }

        public IEnumerable<IColumn> getColumns()
        {
            throw new NotImplementedException();
        }
    }
}
