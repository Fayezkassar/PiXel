using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Views.Windows;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.ViewModels
{
    public class ImageWindowViewModel : ViewModelBase
    {
        private readonly Registry _registry;
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

        public RelayCommand<IColumn> ResizeImageCommand { get; }
        public ImageWindowViewModel(BitmapImage image, Registry registry)
        {
            DisplayedImage = image;
            _registry = registry;
            ResizeImageCommand = new RelayCommand<IColumn>(OnResizeImage);
        }

        private void OnResizeImage(IColumn column)
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel(column, _registry);
            window.ShowDialog();
        }

    }
}
