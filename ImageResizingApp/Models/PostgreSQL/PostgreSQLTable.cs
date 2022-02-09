using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.PostgreSQL
{
    public class PostgreSQLTable : ITable
    {
        public string Name { get ; set; }

        public IEnumerable<IColumn> getColumns()
        {
            throw new NotImplementedException();
        }
    }
}
