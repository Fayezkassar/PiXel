using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Oracle
{
    public class SQLServerTable : ITable
    {
        public string Name { get; set; }
        public async Task<IEnumerable<IColumn>> getColumns()
        {
            throw new NotImplementedException();
        }
    }
}
