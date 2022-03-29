using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Models.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationWindowViewModel : ViewModelBase
    {
        private readonly IColumn _column;
        private readonly Registry _registry;
        public RelayCommand<Window> ConfirmCommand { get; }
        public RelayCommand PreviousCommand { get; }

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

        private string _confirmButtonContent = "Next";
        public string ConfirmButtonContent
        {
            get { return _confirmButtonContent; }
            set { SetProperty(ref _confirmButtonContent, value, false); }
        }

        public ResizeConfigurationWindowViewModel(IColumn column, Registry registry)
        {
            _column = column;
            _registry = registry;

            ConfirmCommand = new RelayCommand<Window>(OnConfirm, CanConfirm);
            PreviousCommand = new RelayCommand(OnPrevious, CanGoBack);


            _part1ViewModel = new ResizeConfigurationPart1ViewModel();
            _part2ViewModel = new ResizeConfigurationPart2ViewModel(registry, ConfirmCommand);
            CurrentViewModel = _part1ViewModel;
        }

        private void OnConfirm(Window connectWindow)
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
                        CompositeFilter filterCombo = new CompositeFilter();
                        foreach (FilterViewModel filerViewModel in _part2ViewModel.SelectedFilters)
                        {
                            filterCombo.AddFilter(filerViewModel.Filter);
                        }
                        _column.Resize(_part1ViewModel.From, _part1ViewModel.To, _part1ViewModel.MinSize, _part1ViewModel.MaxSize, filterCombo, _part1ViewModel.BackupDestination);
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

        private bool CanGoBack() => CurrentViewModel == _part2ViewModel;

        private bool CanConfirm(Window connectWindow)
        {
            if(CurrentViewModel == _part2ViewModel)
            {
                return _part2ViewModel.SelectedFilters.Count() > 0;
            }
            return true;
        }
    }

}
