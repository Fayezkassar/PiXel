using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.ViewModels;
using System.Windows;
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

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataTemplate dt = null;
            if (e.PropertyType.Name == "Byte[]")
            {
                dt = (DataTemplate)Resources["blobTemplate"];
            }
            if (dt != null)
            {
                DataGridTemplateColumn c = new DataGridTemplateColumn()
                {
                    CellTemplate = dt,
                    Header = e.Column.Header,
                    HeaderTemplate = e.Column.HeaderTemplate,
                    HeaderStringFormat = e.Column.HeaderStringFormat,
                    SortMemberPath = e.PropertyName // this is used to index into the DataRowView so it MUST be the property's name (for this implementation anyways)
                };
                e.Column = c;
            }
        }
    }
}
