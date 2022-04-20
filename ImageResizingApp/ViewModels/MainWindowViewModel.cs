using ImageResizingApp.Stores;

namespace ImageResizingApp.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainWindowViewModel(DataSourceStore dataSourceStore, Registry registry)
        {
            CurrentViewModel = new TableListingViewModel(dataSourceStore, registry);
        }
    }
}
