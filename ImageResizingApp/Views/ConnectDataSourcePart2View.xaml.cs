using ImageResizingApp.Stores;
using ImageResizingApp.ViewModels;
using ImageResizingApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for ConnectDataSourcePart2View.xaml
    /// </summary>
    public partial class ConnectDataSourcePart2View : UserControl
    {
        public ConnectDataSourcePart2View()
        {
            
            InitializeComponent();
        }

        void PasswordChangedHandler(Object sender, RoutedEventArgs args)
        {
            ((ConnectDataSourcePart2ViewModel)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
