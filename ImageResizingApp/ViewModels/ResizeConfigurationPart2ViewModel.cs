using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationPart2ViewModel: ViewModelBase
    {
        public readonly Registry Registry;

        public RelayCommand ConfirmCommand { get; }

        public IEnumerable<string> Filters
        {
            get
            {
                return Registry.GetFilterKeys();
            }
        }

        private readonly ObservableCollection<FilterViewModel> _selectedFilters;
        public IEnumerable<FilterViewModel> SelectedFilters => _selectedFilters;

        public RelayCommand<FilterViewModel> DeleteFilterCommand { get; }
        public ResizeConfigurationPart2ViewModel(Registry registry, RelayCommand confirmCommand)
        {
            Registry = registry;
            ConfirmCommand = confirmCommand;
            _selectedFilters = new ObservableCollection<FilterViewModel>();
            _selectedFilters.CollectionChanged += OnCollectionChanged;
            DeleteFilterCommand = new RelayCommand<FilterViewModel>(OnDeleteFilter);
        }

        private void OnDeleteFilter(FilterViewModel filter)
        {
            _selectedFilters.Remove(filter);
        }

        public void SelectFilter(FilterViewModel filter)
        {
            _selectedFilters.Add(filter);
        }

        void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.ConfirmCommand.NotifyCanExecuteChanged();
        }

    }
}
