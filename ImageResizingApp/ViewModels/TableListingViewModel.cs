using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using ImageResizingApp.Views.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

        public TableListingViewModel(DataSourceStore dataSourceStore, Registry registry) 
        {
            _registry = registry;
            _dataSourceStore = dataSourceStore;
            _tables = new ObservableCollection<ITable>(_dataSourceStore.GetTables());
            _columns = new ObservableCollection<IColumn>();
            _dataSourceStore.CurrentDataSourceChanged += UpdateTables;
            ResizeColumnCommand = new RelayCommand<IColumn>(OnResizeColumn, CanResizeColumn);
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
            Data = await _dataSourceStore.GetTableDataAsync(table);
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

        private bool CanResizeColumn(IColumn column)
        {
            if (column != null) return column.Resizable;
            return true;
        }
    }
       
}
