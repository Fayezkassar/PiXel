using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Models.Filters;
using System.Linq;
using System.Threading.Tasks;
using static ImageResizingApp.Models.ResizeConfig;
using System.ComponentModel;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationWindowViewModel : ViewModelBase
    {
        private readonly IColumn? _column;
        private readonly Registry _registry;
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand PreviousCommand { get; }
        public RelayCommand CancelCommand { get; }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value, false);
            }
        }

        private ResizeConfigurationPart1ViewModel _part1ViewModel { get; }
        private ResizeConfigurationPart2ViewModel _part2ViewModel { get; }
        private ResizeConfigurationPart3ViewModel _part3ViewModel { get; }

        private BackgroundWorker _worker;

        private string _confirmButtonContent = "Next";
        public string ConfirmButtonContent
        {
            get { return _confirmButtonContent; }
            set { SetProperty(ref _confirmButtonContent, value, false); }
        }

        private IImage? _image;
        private bool _isBatch;

        public ResizeConfigurationWindowViewModel(IColumn column, Registry registry) : this(registry, true)
        {
            _column = column;
            _column.ProgressChanged += myProgressChanged;
        }

        public ResizeConfigurationWindowViewModel(IImage image, Registry registry): this(registry, false)
        {
            _image = image;
        }

        private ResizeConfigurationWindowViewModel(Registry registry, bool isBatch)
        {
            _registry = registry;
            _isBatch = isBatch;

            _worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _worker.DoWork += worker_DoWork;

            ConfirmCommand = new RelayCommand(OnConfirm, CanConfirm);
            PreviousCommand = new RelayCommand(OnPrevious, CanGoBack);
            CancelCommand = new RelayCommand(OnCancel);

            _part1ViewModel = new ResizeConfigurationPart1ViewModel(isBatch);
            _part2ViewModel = new ResizeConfigurationPart2ViewModel(registry, ConfirmCommand);
            _part3ViewModel = new ResizeConfigurationPart3ViewModel();
            CurrentViewModel = _part1ViewModel;

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            OnConfirmResize(sender, e);
        }

        void myProgressChanged(object sender, Models.ResizeConfig.ProgressChangedEventHandler e)
        {
            _part3ViewModel.ProgressBarConfig = e.Config;
        }

        private void OnConfirm()
        {
            CurrentViewModel.Validate();
            if (!CurrentViewModel.HasErrors)
            {
                if (CurrentViewModel == _part1ViewModel)
                {
                    ConfirmButtonContent = "Confirm";
                    CurrentViewModel = _part2ViewModel;
                    PreviousCommand.NotifyCanExecuteChanged();
                    ConfirmCommand.NotifyCanExecuteChanged();
                }
                else
                {
                    if (ValidatePart2Filters())
                    {
                        if(!_worker.IsBusy)
                            _worker.RunWorkerAsync();
                        CurrentViewModel = _part3ViewModel;
                        PreviousCommand.NotifyCanExecuteChanged();
                        ConfirmCommand.NotifyCanExecuteChanged();
                    }
                }
            }
        }

        private void OnConfirmResize(object sender, DoWorkEventArgs e)
        {
            CurrentViewModel.Validate();
            if (!CurrentViewModel.HasErrors)
            {
                if (ValidatePart2Filters())
                {
                    CompositeFilter filterCombo = new CompositeFilter();
                    foreach (FilterViewModel filerViewModel in _part2ViewModel.SelectedFilters)
                    {
                        filterCombo.AddFilter(filerViewModel.Filter);
                    }
                    if (_isBatch)
                    {
                        _column.Resize(_part1ViewModel.From, _part1ViewModel.To, _part1ViewModel.MinSize, _part1ViewModel.MaxSize, filterCombo, _part1ViewModel.BackupDestination,sender, e);
                    }
                    else
                    {
                        _image.Resize(filterCombo, _part1ViewModel.BackupDestination);
                    }

                }
            }
        }

        private bool ValidatePart2Filters()
        {
            bool valid = true;
            foreach (FilterViewModel param in _part2ViewModel.SelectedFilters)
            {
                param.Validate();
                if (param.HasErrors) valid = false;
            }
            return valid;
        }

        private void OnPrevious()
        {
            ConfirmButtonContent = "Next";
            CurrentViewModel = _part1ViewModel;
            PreviousCommand.NotifyCanExecuteChanged();
            ConfirmCommand.NotifyCanExecuteChanged();
        }

        private void OnCancel()
        {
            if (CurrentViewModel == _part3ViewModel)
            {
                _worker.CancelAsync();
            }
        }

        private bool CanGoBack() => CurrentViewModel == _part2ViewModel;

        private bool CanConfirm()
        {
            if(CurrentViewModel == _part2ViewModel)
            {
                return _part2ViewModel.SelectedFilters.Count() > 0;
            } else if (CurrentViewModel == _part1ViewModel)
                return true;
            return false;
        }
    }

}
