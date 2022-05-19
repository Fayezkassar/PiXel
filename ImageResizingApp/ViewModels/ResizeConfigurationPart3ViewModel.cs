using ImageResizingApp.Helpers;
using ImageResizingApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationPart3ViewModel : ViewModelBase
    {
        private ResizingProgress _resizeProgress = new ResizingProgress();
        public ResizingProgress ProgressBarConfig
        {
            get => _resizeProgress;
            set
            {
                SetProperty(ref _resizeProgress, value);
            }
        }

        public string SpaceGain
        {
            get => Utilities.GetFormatedSize(_resizeProgress.SpaceGain);
        }

        public ResizeConfigurationPart3ViewModel()
        {
        }
    }
}
