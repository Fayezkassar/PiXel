using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.Models.DataSources.PostgreSQL
{
    public class PostgresSQLColumn : IColumn
    {
        public string Name { get; set; }

        public string ColumnType { get; set; }

        public bool Resizable { get; set; }

        public ITable Table { get; set; }

        public event EventHandler<IColumn.ProgressChangedEventHandler> ProgressChanged;

        public BitmapImage GetBitmapImage(DataRowView row)
        {
            throw new NotImplementedException();
        }

        public IImage GetImageWithPrimaryKeysValues(IEnumerable<string> primaryKeysValues)
        {
            throw new NotImplementedException();
        }

        public bool Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination)
        {
            throw new NotImplementedException();
        }
    }
}
