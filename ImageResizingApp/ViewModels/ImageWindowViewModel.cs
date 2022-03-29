using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.ViewModels
{
    public class ImageWindowViewModel : ViewModelBase
    {
        private readonly DataRowView _row;

        private BitmapImage _displayedImage;

        public BitmapImage DisplayedImage
        {
            get
            {
                return _displayedImage;
            }
            set
            {
                SetProperty(ref _displayedImage, value, false);
            }
        }
        public ImageWindowViewModel(BitmapImage image)
        {
            DisplayedImage = image;
           // byte[] imagebytes = _row.Row["DOC"] as byte[];
            //DisplayedImage = LoadImage(imagebytes);
        }
        
    }
}
