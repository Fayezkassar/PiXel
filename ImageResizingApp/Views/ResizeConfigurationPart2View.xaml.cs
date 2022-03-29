using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageResizingApp.Views
{
    /// <summary>
    /// Interaction logic for ResizeConfigurationPart2View.xaml
    /// </summary>
    public partial class ResizeConfigurationPart2View : UserControl
    {
        public ResizeConfigurationPart2View()
        {
            InitializeComponent();
        }
        object sourceData = null;
        Point? potentialDragStartPoint = null;

        private void ListBox2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (potentialDragStartPoint == null)
            {
                ListBox parent = (ListBox)sender;
                potentialDragStartPoint = e.GetPosition(parent);
            }
        }

        private void ListBox2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            potentialDragStartPoint = null;
        }

        private void ListBox2_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (potentialDragStartPoint == null) { return; }

            ListBox parent = (ListBox)sender;
            var dragPoint = e.GetPosition(parent);

            Vector potentialDragLength = dragPoint - potentialDragStartPoint.Value;
            if (potentialDragLength.Length > 5)
            {
                sourceData = GetDataFromListBox(parent, e.GetPosition(parent));

                if (sourceData != null)
                {
                    DragDrop.DoDragDrop(parent, sourceData, DragDropEffects.Move);
                    potentialDragStartPoint = null;
                }
            }
        }

        private void ListBox1_PreviewMouseLeftButtonDown(object sender, MouseEventArgs e)
        {

            ListBox parent = (ListBox)sender;
            object data = GetDataFromListBox(parent, e.GetPosition(parent));

                if (data != null)
                {
                    DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
                }
            
        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ResizeConfigurationPart2ViewModel context = DataContext as ResizeConfigurationPart2ViewModel;
            ListBox parent = (ListBox)sender;
            string filterName = e.Data.GetData(typeof(string)) as string;
            if(filterName != null) {
                ((ObservableCollection<FilterViewModel>)parent.ItemsSource).Add(new FilterViewModel(context.Registry.GetFilterFromKey(filterName)));
            }
            else
            {
                object data = sourceData;
                object target = GetDataFromListBox(parent, e.GetPosition(parent));

                int removedIdx = parent.Items.IndexOf(data);
                int targetIdx = parent.Items.IndexOf(target);
                if(targetIdx!=-1 && removedIdx != -1)
                {
                    ((ObservableCollection<FilterViewModel>)parent.ItemsSource).Move(removedIdx, targetIdx);
                }
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
