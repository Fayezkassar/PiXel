using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class FilterViewModel : ViewModelBase
    {
        public IFilter Filter { get; }

        public string Name => Filter.Name;

        private int? _width;
        [Required]
        [DisplayAttribute(Name = "Width")]
        public int? Width
        {
            get
            {
                return _width;
            }
            set
            {
                Filter.Width = value ?? 0;
                SetProperty(ref _width, value, true);
            }
        }

        private int? _height;
        [Required]
        [DisplayAttribute(Name = "Height")]
        public int? Height
        {
            get
            {
                return _height;
            }
            set
            {
                Filter.Height = value ?? 0;
                SetProperty(ref _height, value, true);
            }
        }

        public FilterViewModel(IFilter filter)
        {
            Filter = filter;
        }
    }
}
