using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        public virtual void Dispose() { }
    }
}
