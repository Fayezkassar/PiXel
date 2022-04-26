using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationPart3ViewModel : ViewModelBase
    {
        private int status = 10;
        public int ProgressBarValue
        {
            get => status;
            set
            {
                SetProperty(ref status, value, false);
            }
        }

        public ResizeConfigurationPart3ViewModel()
        {
        }
    }
}
