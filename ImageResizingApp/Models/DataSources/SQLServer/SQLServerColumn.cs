using ImageResizingApp.Models.Interfaces;
using System;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerColumn : IColumn
    {
        public ITable Table { get; set; }

        public string Name { get; set; }

        public string ColumnType { get; set; }

        public bool Resizable { get; set; }
        public bool Resize(int? from, int? to, int? minSize, int? maxSize)
        {
            throw new NotImplementedException();
        }

    }
}
