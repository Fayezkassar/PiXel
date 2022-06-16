using ImageMagick;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static ImageResizingApp.Models.ResizingProgress;

namespace ImageResizingApp.Models.DataSources.SQLServer
{
    public class SQLServerColumn : IColumn
    {
        public ITable Table { get; set; }
        public string Name { get; set; }
        public string ColumnType { get; set; }
        public bool CanResize { get; set; }

        public event EventHandler<ResizingProgress.ProgressChangedEventHandler> ProgressChanged;

        public BitmapImage GetBitmapImage(DataRowView row)
        {
            throw new NotImplementedException();
        }

        public IImage GetImageForPrimaryKeysValues(IEnumerable<string> primaryKeysValues)
        {
            throw new NotImplementedException();
        }


        public void Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination, object sender, DoWorkEventArgs e, IQualityAssessment iqa)
        {
            throw new NotImplementedException();
        }

    }
}
