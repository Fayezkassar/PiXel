using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Views.Windows;
using System.Data;
using System.Windows.Media.Imaging;

namespace ImageResizingApp.ViewModels
{
    public class ViewImageWindowViewModel : ViewModelBase
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
        private IImage _image;

        public string Size
        {
            get { return (int)DisplayedImage.Height +"x"+ (int)DisplayedImage.Width;}
           
        }

        public RelayCommand ResizeImageCommand { get; }
        public ViewImageWindowViewModel(IImage image, Registry registry)
        {
            _image = image;
            DisplayedImage = image.GetBitmapImage();
            _registry = registry;
            ResizeImageCommand = new RelayCommand(OnResizeImage);
        }

        private void OnResizeImage()
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel(_image, _registry);
            window.ShowDialog();
        }

    }
}
