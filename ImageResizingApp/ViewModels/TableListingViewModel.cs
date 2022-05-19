using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using ImageResizingApp.Views.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Linq;
using System;
using System.ComponentModel;

namespace ImageResizingApp.ViewModels
{
    public class TableListingViewModel : ViewModelBase
    {
        private readonly DataSourceStore _dataSourceStore;
        private readonly Registry _registry;

        private readonly ObservableCollection<ITable> _tables;
        public IEnumerable<ITable> Tables => _tables;

        private readonly ObservableCollection<IColumn> _columns;
        public IEnumerable<IColumn> Columns => _columns;

        private DataTable _data;
        public DataTable Data
        {
            get { return _data; }
            set => SetProperty(ref _data, value, false);
        }

        private ITable _selectedTable;
        public ITable SelectedTable
        {
            get { return _selectedTable; }
            set {
                SetProperty(ref _selectedTable, value, false);
            }
        }

        private string _tableSize;
        public string TableSize
        {
            get { return _tableSize; }
            set
            {
                SetProperty(ref _tableSize, value, false);
            }
        }

        private string _recordsNumber;
        public string RecordsNumber
        {
            get { return _recordsNumber; }
            set
            {
                SetProperty(ref _recordsNumber, value, false);
            }
        }

        public RelayCommand<IColumn> ResizeColumnCommand { get; }
        public RelayCommand<DataGridCell> ViewImage { get; }
        public RelayCommand<string> TableSelectedCommand { get; }

        // FOR PAGINATION

        private int start = 0;
        private int itemCount = 15;
        private int end = 15;
        private int totalItems = 0;

        public int Start
        {
            get { return start; }
            set => SetProperty(ref start, value, false);
        }

        public int End
        {
            get { return end; }
            set => SetProperty(ref end, value, false);
        }

        public int TotalItems
        {
            get { return totalItems; }
            set => SetProperty(ref totalItems, value, false);
        }
        public RelayCommand FirstCommand { get; }
        public RelayCommand PreviousCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand LastCommand { get; }

        public TableListingViewModel(DataSourceStore dataSourceStore, Registry registry) 
        {
            _registry = registry;
            _dataSourceStore = dataSourceStore;

            _tables = new ObservableCollection<ITable>();
            _columns = new ObservableCollection<IColumn>();

            _dataSourceStore.OnCurrentDataSourceChanged = UpdateTables;
            _dataSourceStore.OnCurrentDataSourceChanged();

            ResizeColumnCommand = new RelayCommand<IColumn>(OnResizeColumn, CanResizeColumn);
            ViewImage = new RelayCommand<DataGridCell>(OnViewImage);
            FirstCommand = new RelayCommand(OnFirstCommand);
            PreviousCommand = new RelayCommand(OnPreviousCommand);
            NextCommand = new RelayCommand(OnNextCommand);
            LastCommand = new RelayCommand(OnLastCommandAsync);
        }
        private async Task UpdateTables()
        {
            _tables.Clear();
            IEnumerable<ITable> tables = await _dataSourceStore.GetTablesAsync();

            foreach (ITable table in tables)
            {
                _tables.Add(table);
            }
        }

        private async Task UpdateColumnsAsync()
        {
            _columns.Clear();
            IEnumerable<IColumn> columns = await _dataSourceStore.GetTableColumns(SelectedTable);

            foreach (IColumn column in columns)
            {
                _columns.Add(column);
            }
        }

        private async Task UpdateDataAsync()
        {
            Data?.Clear();
            Data = await _dataSourceStore.GetTableDataAsync(SelectedTable, start, itemCount);
        }

        private async Task UpdateTableInfoAsync()
        {
            RecordsNumber = null;
            TableSize = null;
            await _dataSourceStore.UpdateTableInfoAsync(SelectedTable);
            RecordsNumber = SelectedTable.RecordsNumber;
            TableSize = SelectedTable.TableSize;
        }

        public async Task UpdateSelectedTableAsync()
        {
            await UpdateColumnsAsync();
            await UpdateDataAsync();
            await UpdateTableInfoAsync();
            TotalItems = int.Parse(SelectedTable.RecordsNumber);
        }

        private void OnResizeColumn(IColumn column)
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel(column, _registry);
            window.Closed += OnWindowClosed;
            window.ShowDialog();

        }

        private async void OnWindowClosed(object sender, EventArgs e)
        {
            await UpdateTableInfoAsync();
        }

        private void OnViewImage(DataGridCell cell)
        {
            DataRowView drv = cell.DataContext as DataRowView;
            List<string> pks = new List<string>();
            foreach (string key in SelectedTable.PrimaryKeys)
            {
                int index = Data.Columns.IndexOf(key);
                string value = drv.Row.ItemArray[index].ToString();
                pks.Add(value);
            }
            IColumn column = Columns.FirstOrDefault(x => x.Name == cell.Column.Header.ToString());
            IImage image= column.GetImageForPrimaryKeysValues(pks);
            ViewImageWindow window = new ViewImageWindow();
            window.DataContext = new ViewImageWindowViewModel(image, _registry);
            window.ShowDialog();
        }

        private bool CanResizeColumn(IColumn column)
        {
            if (column != null) return column.CanResize;
            return true;
        }

        // FOR PAGINATION

        private async void OnFirstCommand()
        {
            if (start > 0)
            {
                Start = 0;
                End = start + itemCount < totalItems ? start + itemCount : totalItems;
                await UpdateDataAsync();
            }
        }

        private async void OnPreviousCommand()
        {
            if (start >= itemCount)
            {
                Start -= itemCount;
                End = start + itemCount < totalItems ? start + itemCount : totalItems;
                await UpdateDataAsync();
            }
        }

        private async void OnNextCommand()
        {
            if (start < totalItems - itemCount)
            {
                Start += itemCount;
                End = start + itemCount < totalItems ? start + itemCount : totalItems;
                await UpdateDataAsync();
            }
        }

        private async void OnLastCommandAsync()
        {
            if (start < totalItems - itemCount)
            {
                Start = totalItems - itemCount;
                End = start + itemCount < totalItems ? start + itemCount : totalItems;
                await UpdateDataAsync();
            }
        }

    }
}
