using ImageResizingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationPart3ViewModel : ViewModelBase
    {
        private ResizeConfig status = new ResizeConfig();
        public ResizeConfig ProgressBarConfig
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
