using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Models.Filters;
using System.Linq;
using System.Threading.Tasks;
using static ImageResizingApp.Models.ResizingProgress;
using System.ComponentModel;
using ImageResizingApp.Models;
using System.Windows;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationWindowViewModel : ViewModelBase
    {
        private readonly Registry _registry;
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand PreviousCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value, false);
            }
        }

        private string _confirmButtonContent = "Next";
        public string ConfirmButtonContent
        {
            get { return _confirmButtonContent; }
            set { SetProperty(ref _confirmButtonContent, value, false); }
        }

        private ResizeConfigurationPart1ViewModel _part1ViewModel { get; }
        private ResizeConfigurationPart2ViewModel _part2ViewModel { get; }
        private ResizeConfigurationPart3ViewModel _part3ViewModel { get; }

        private readonly IImage? _image;
        private readonly bool _isBatch;
        private readonly IColumn? _column;
        private BackgroundWorker _worker;

        public ResizeConfigurationWindowViewModel(IColumn column, Registry registry) : this(registry, true)
        {
            _column = column;
            _column.ProgressChanged += OnResizingProgressChanged;
        }

        public ResizeConfigurationWindowViewModel(IImage image, Registry registry) : this(registry, false)
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
            _worker.DoWork += Worker_DoWork;

            ConfirmCommand = new RelayCommand(OnConfirm, CanConfirm);
            PreviousCommand = new RelayCommand(OnPrevious, CanGoBack);
            CancelCommand = new RelayCommand<Window>(OnCancel);

            _part1ViewModel = new ResizeConfigurationPart1ViewModel(registry, isBatch);
            _part2ViewModel = new ResizeConfigurationPart2ViewModel(registry, ConfirmCommand);
            _part3ViewModel = new ResizeConfigurationPart3ViewModel();
            CurrentViewModel = _part1ViewModel;

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Resize(sender, e);
        }

        private void OnResizingProgressChanged(object sender, Models.ResizingProgress.ProgressChangedEventHandler e)
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
                        if (!_worker.IsBusy)
                            _worker.RunWorkerAsync();
                        CurrentViewModel = _part3ViewModel;
                        PreviousCommand.NotifyCanExecuteChanged();
                        ConfirmCommand.NotifyCanExecuteChanged();
                    }
                }
            }
        }

        private void Resize(object sender, DoWorkEventArgs e)
        {
            IQualityAssessment iqa = _registry.GetIQAFromKey(_part1ViewModel.SelectedIQA);
            CompositeFilter filterCombo = new CompositeFilter();
            foreach (FilterViewModel filerViewModel in _part2ViewModel.SelectedFilters)
            {
                filterCombo.AddFilter(filerViewModel.Filter);
            }
            if (_isBatch)
            {
                _column.Resize(_part1ViewModel.From, _part1ViewModel.To, _part1ViewModel.MinSize, _part1ViewModel.MaxSize, filterCombo, _part1ViewModel.BackupDestination, sender, e, iqa);
            }
            else
            {
                _image.Resize(new ImageResizeParameters(filterCombo, iqa,_part1ViewModel.BackupDestination));
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

        private void OnCancel(Window window)
        {
            if (CurrentViewModel == _part3ViewModel)
            {
                _worker.CancelAsync();
            }
            window.Close();
        }

        private bool CanGoBack() => CurrentViewModel == _part2ViewModel;

        private bool CanConfirm()
        {
            if (CurrentViewModel == _part2ViewModel)
            {
                return _part2ViewModel.SelectedFilters.Count() > 0;
            }
            else if (CurrentViewModel == _part1ViewModel)
                return true;
            else return false;
        }
    }

}
