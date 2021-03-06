using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System;

namespace ImageResizingApp.Models.Interfaces
{
    public interface IImage
    {
        public IColumn Column { get; set; }
        public IEnumerable<string> PrimaryKeysValues { get; set; }
        public bool Resize(ImageResizeParameters irp);
        public BitmapImage GetBitmapImage();
    }
}
