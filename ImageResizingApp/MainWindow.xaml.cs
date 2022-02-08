using ImageResizingApp.Stores;
using ImageResizingApp.ViewModels;
using ImageResizingApp.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageResizingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataSourceRegistry _dataSourceRegistry;
        private readonly DataSourceStore _dataSourceStore;
        public MainWindow(DataSourceRegistry dataSourceRegistry, DataSourceStore dataSourceStore)
        {
            InitializeComponent();
            _dataSourceStore = dataSourceStore;
            _dataSourceRegistry = dataSourceRegistry;
            OpenConnectToDataSourceDialog();
        
        }

        private void OpenConnectToDataSourceDialog()
        {
            ConnectDataSourceWindow connectDialog = new ConnectDataSourceWindow();
            //connectDialog.ShowInTaskbar = false;
            //connectDialog.Owner = this;
            connectDialog.DataContext = new ConnectDataSourceWindowViewModel(_dataSourceRegistry, _dataSourceStore);
            connectDialog.ShowDialog();
        }
    }
}
