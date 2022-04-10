using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Views.Windows;
using System.Data;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.ViewModels
{
    public class ImageWindowViewModel : ViewModelBase
    {
        private readonly Registry _registry;
        private IColumn _column;
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

        public RelayCommand ResizeImageCommand { get; }
        public ImageWindowViewModel(IColumn column, DataRowView row, Registry registry)
        {
            BitmapImage image = column.GetBitmapImage(row);
            DisplayedImage = image;
            _registry = registry;
            _column = column;
            ResizeImageCommand = new RelayCommand(OnResizeImage);
        }

        private void OnResizeImage()
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel(_column, _registry, false, 103896); // GET THE REAL ROWNNUM FROM ROW IN CONSTRUCTOR
            window.ShowDialog();
        }

    }
}
