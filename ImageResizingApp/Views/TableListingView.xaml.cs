using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.ViewModels;
using System.Windows.Controls;

namespace ImageResizingApp.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableListingView : UserControl
    {
        public TableListingView()
        {
            InitializeComponent();
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListBox)sender;
            var table = (ITable)item.SelectedItem;
            await ((TableListingViewModel)DataContext).UpdateTableAsync(table);
        }
    }
}
