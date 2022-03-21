using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ConnectDataSourcePart1ViewModel : ViewModelBase
    {
        private readonly Registry _dataSourceRegistry;

        private string _dataSourceName;
        [Required]
        [DisplayAttribute(Name = "Data Source Name")]
        public string DataSourceName
        {
            get
            {
                return _dataSourceName;
            }
            set
            {
                SetProperty(ref _dataSourceName, value, true);
            }
        }

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
                return _dataSourceRegistry.GetDataSourceKeys();
            }
        }

        public ConnectDataSourcePart1ViewModel(Registry dataSourceRegistry)
        {
            _dataSourceRegistry = dataSourceRegistry;
        }
    }
}
