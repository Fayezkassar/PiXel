using ImageResizingApp.Models.Interfaces;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerColumn : IColumn
    {
        public string Name { get; set; }

        public string ColumnType { get; set; }

        public string TotalSize { get; set; }
    }
}
