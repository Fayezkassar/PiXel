using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourcePart1ViewModel : ViewModelBase
    {
        private readonly Registry _registry;

        private string _selectedDataSourceType;
        [Required]
        [DisplayAttribute(Name = "Data Source Type")]
        public string SelectedDataSourceType
        {
            get
            {
                return _selectedDataSourceType;
            }
            set
            {
                SetProperty(ref _selectedDataSourceType, value, true);
            }
        }

        public IEnumerable<string> DataSourceTypes
        {
            get
            {
                return _registry.GetDataSourceKeys();
            }
        }

        public ConnectDataSourcePart1ViewModel(Registry dataSourceRegistry)
        {
            _registry = dataSourceRegistry;
        }
    }
}
