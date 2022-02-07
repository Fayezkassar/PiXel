using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ViewModelBase : ObservableValidator
    {
        public void Validate()
        {
            ValidateAllProperties();
        }
        public virtual void Dispose() { }
    }
}
