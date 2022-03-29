using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationPart1ViewModel: ViewModelBase
    {
        public RelayCommand ChooseDestinationCommand { get; }

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

        private string _backupDestination;
        public string BackupDestination
        {
            get
            {
                return _backupDestination;
            }
            set
            {
                SetProperty(ref _backupDestination, value, true);
            }
        }

        public ResizeConfigurationPart1ViewModel()
        {
            ChooseDestinationCommand = new RelayCommand(OnChooseDestination);
        }

        public void OnChooseDestination()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    BackupDestination = dialog.SelectedPath;
                }
            }
        }
    }
}
