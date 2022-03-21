using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ImageResizingApp.ViewModels
{
    public class ResizeConfigurationWindowViewModel : ViewModelBase
    {
        private readonly IColumn _column;
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

        public ResizeConfigurationWindowViewModel(IColumn column)
        {
            _column = column;

            _part1ViewModel = new ResizeConfigurationPart1ViewModel();
            _part2ViewModel = new ResizeConfigurationPart2ViewModel();
            CurrentViewModel = _part1ViewModel;

            ConfirmCommand = new RelayCommand<Window>(OnConfirm);
            PreviousCommand = new RelayCommand(OnPrevious, CanGoBack);
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
                }
                else
                {
                    _column.Resize(_part1ViewModel.From,_part1ViewModel.To,_part1ViewModel.MinSize,_part1ViewModel.MaxSize);
                }
            }
        }
        private void OnPrevious()
        {
            ConfirmButtonContent = "Next";
            CurrentViewModel = _part1ViewModel;
        }

        private bool CanGoBack() => CurrentViewModel == _part2ViewModel;
    }

}
