using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace ImageResizingApp.ViewModels
{
    public class FilterParameterViewModel: ViewModelBase
    {
        public string DisplayName { get; }
        private IFilter _filter { get; }

        private int? _value;

        [Required]
        [DisplayAttribute(Name = "above")]
        public int? Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != null) _filter.Parameters[DisplayName] = (int)value;
                SetProperty(ref _value, value, true);
            }
        }
        public FilterParameterViewModel(string name, IFilter filter)
        {
            DisplayName = name;
            _filter = filter;
        }
    }
}
