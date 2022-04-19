using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IImage
    {
        public IColumn Column { get; set; }
        public List<string> PrimaryValues { get; set; }

        public bool Resize(IFilter filter, string backupDestination);

        public BitmapImage GetBitmapImage();
    }
}
