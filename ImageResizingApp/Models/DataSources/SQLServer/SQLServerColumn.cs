using ImageResizingApp.Models.Interfaces;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerColumn : IColumn
    {
        public string Name { get; set; }

        public ColumnType ColumnType { get; set; }
    }
}
