using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        public IFilter Filter { get; }

        public string Name => Filter.Name;

        private readonly ObservableCollection<FilterParameterViewModel> _parameters;
        public IEnumerable<FilterParameterViewModel> Parameters => _parameters;

        public FilterViewModel(IFilter filter)
        {
            Filter = filter;
            _parameters = new ObservableCollection<FilterParameterViewModel>();
            foreach (string param in Filter.Parameters?.Keys)
            {
                _parameters.Add(new FilterParameterViewModel(param, Filter));
            }
        }
    }
}
