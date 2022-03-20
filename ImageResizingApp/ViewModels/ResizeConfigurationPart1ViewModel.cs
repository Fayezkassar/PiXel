using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationPart1ViewModel: ViewModelBase
    {

        private int? _from;
        public int? From
        {
            get
            {
                return _from;
            }
            set
            {
                SetProperty(ref _from, value, true);
            }
        }

        private int? _to;
        public int? To
        {
            get
            {
                return _to;
            }
            set
            {
                SetProperty(ref _to, value, true);
            }
        }


        private int? _maxSize;
        public int? MaxSize
        {
            get
            {
                return _maxSize;
            }
            set
            {
                SetProperty(ref _maxSize, value, true);
            }
        }

        private int? _minSize;
        public int? MinSize
        {
            get
            {
                return _minSize;
            }
            set
            {
                SetProperty(ref _minSize, value, true);
            }
        }
    }
}
