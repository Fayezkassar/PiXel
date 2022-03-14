using ImageResizingApp.Models.Interfaces;

namespace ImageResizingApp.Models.DataSources.Oracle
{
    internal class OracleColumn : IColumn
    {
        public string Name { get; set; }

        public string ColumnType { get; set; }

        public bool Resizable { get; set; }

    }
}
