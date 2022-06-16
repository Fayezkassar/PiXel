using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ConnectionParameterViewModel: ViewModelBase
    {
        public string DisplayName { get; }

        private string _value;
        [Required]
        [DisplayAttribute(Name ="above")]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetProperty(ref _value, value, true);
            }
        }

        public bool IsPassword { get; }

        public ConnectionParameterViewModel(string displayName)
        {
            DisplayName = displayName;
            IsPassword = displayName == "Password";
        }
    }
}
