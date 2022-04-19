using ImageMagick;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IColumn
    {
        public ITable Table { get; set; }

        public string Name { get; set; }

        public string ColumnType { get; set; }
        
        public bool Resizable { get;set; }

        public bool Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination);

        public IImage GetImagePerPrimaryKeys(List<string> primaryValues);

    }
}
