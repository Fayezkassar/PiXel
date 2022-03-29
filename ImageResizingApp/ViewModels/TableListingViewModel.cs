using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using ImageResizingApp.Views.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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

        public RelayCommand<DataRowView> ViewImage { get; }
        public RelayCommand<string> TableSelectedCommand { get; }
        public TableListingViewModel(DataSourceStore dataSourceStore, Registry registry) 
        {
            _registry = registry;
            _dataSourceStore = dataSourceStore;
            _tables = new ObservableCollection<ITable>(_dataSourceStore.GetTables());
            _columns = new ObservableCollection<IColumn>();
            _dataSourceStore.CurrentDataSourceChanged += UpdateTables;
            ResizeColumnCommand = new RelayCommand<IColumn>(OnResizeColumn, CanResizeColumn);
            ViewImage = new RelayCommand<DataRowView>(OnViewImage);
            FirstCommand = new RelayCommand(OnFirstCommand);
            PreviousCommand = new RelayCommand(OnPreviousCommand);
            NextCommand = new RelayCommand(OnNextCommand);
            LastCommand = new RelayCommand(OnLastCommandAsync);
        }
        private void UpdateTables()
        {
            _tables.Clear();
            IEnumerable<ITable> tables = _dataSourceStore.GetTables();

            foreach (ITable table in tables)
            {
                _tables.Add(table);
            }
        }

        private async Task UpdateColumnsAsync(ITable table)
        {
            _columns.Clear();
            IEnumerable<IColumn> columns = await _dataSourceStore.GetTableColumns(table);

            foreach (IColumn column in columns)
            {
                _columns.Add(column);
            }
        }

        private async Task UpdateDataAsync(ITable table)
        {
            Data?.Clear();
            Data = await _dataSourceStore.GetTableDataAsync(table, start, itemCount);
        }

        private async Task UpdateTableInfoAsync(ITable table)
        {
            RecordsNumber = null;
            TableSize = null;
            await _dataSourceStore.UpdateTableInfoAsync(table);
            RecordsNumber = table.RecordsNumber;
            TableSize = table.TableSize;
        }

        public async Task UpdateTableAsync(ITable table)
        {
            currentTable = table;
            TotalItems = int.Parse(SelectedTable.RecordsNumber);
            await UpdateColumnsAsync(table);
            await UpdateDataAsync(table);
            await UpdateTableInfoAsync(table);
        }

        private void OnResizeColumn(IColumn column)
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel(column, _registry);
            window.ShowDialog();
        }

        private void OnViewImage(DataRowView row)
        {
            BitmapImage data = _dataSourceStore.GetBitmapImage(currentTable, row);
            ImageWindow window = new ImageWindow();
            window.DataContext = new ImageWindowViewModel(data);
            window.ShowDialog();
        }

        private bool CanResizeColumn(IColumn column)
        {
            if (column != null) return column.Resizable;
            return true;
        }



        // FOR PAGINATION

        private int start = 0;
        private ITable currentTable;
        private int itemCount = 10;
        private int totalItems = 0;
        public RelayCommand FirstCommand { get; }
        public RelayCommand PreviousCommand { get; }
        public RelayCommand NextCommand { get; }
        public RelayCommand LastCommand { get; }

        public int Start { 
            get { return start; }
            set => SetProperty(ref start, value, false);
        }

        public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }

        public int TotalItems { 
            get { return totalItems; }
            set => SetProperty(ref totalItems, value, false);
        }

        private async void OnFirstCommand()
        {
            if (start > 0)
            {
                Start = 0;
                await UpdateDataAsync(currentTable);
            }
        }

        private async void OnPreviousCommand()
        {
            if (start >= itemCount)
            {
                Start -= itemCount;
                await UpdateDataAsync(currentTable);
            }
        }

        private async void OnNextCommand()
        {
            if (start < totalItems - itemCount)
            {
                Start += itemCount;
                await UpdateDataAsync(currentTable);
            }
        }

        private async void OnLastCommandAsync()
        {
            if (start < totalItems - itemCount)
            {
                Start = totalItems - itemCount;
                await UpdateDataAsync(currentTable);
            }
        }

    }
}
