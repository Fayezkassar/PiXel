using ImageResizingApp.Models.Interfaces;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public string Name { get; set; }

        public ColumnType ColumnType { get; set; }
    }
}
