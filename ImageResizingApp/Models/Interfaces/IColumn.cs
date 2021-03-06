using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using static ImageResizingApp.Models.ResizingProgress;
using System.ComponentModel;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IColumn
    {
        public ITable Table { get; set; }
        public string Name { get; set; }
        public string ColumnType { get; set; }
        public bool CanResize { get; set; }
        public void Resize(int? from, int? to, int? minSize, int? maxSize, IFilter filter, string backupDestination, object sender, DoWorkEventArgs e, IQualityAssessment iqa);
        public IImage GetImageForPrimaryKeysValues(IEnumerable<string> primaryKeysValues);

        public event EventHandler<ResizingProgress.ProgressChangedEventHandler> ProgressChanged;
        

    }
}
