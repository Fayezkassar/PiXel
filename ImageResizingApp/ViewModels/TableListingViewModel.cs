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

namespace ImageResizingApp.ViewModels
{
    public class TableListingViewModel : ViewModelBase
    {
        private readonly DataSourceStore _dataSourceStore;

        private ObservableCollection<ITable> _tables;
        public IEnumerable<ITable> Tables => _tables;

        private ObservableCollection<IColumn> _columns;

        public IEnumerable<IColumn> Columns
        {
            get { return _columns; }
            set => SetProperty(ref _columns, new ObservableCollection<IColumn>(value), false);
        }

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
            set { SetProperty(ref _selectedTable, value, false); }
        }

        public RelayCommand<IColumn> ResizeColumnCommand { get; }
        public RelayCommand<string> TableSelectedCommand { get; }
        public TableListingViewModel(DataSourceStore dataSourceStore) 
        {
            _dataSourceStore = dataSourceStore;
            _tables = new ObservableCollection<ITable>(_dataSourceStore.GetTables());
            _dataSourceStore.CurrentDataSourceChanged += UpdateTables;
            TableSelectedCommand = new RelayCommand<string>(OnTableClicked);
            ResizeColumnCommand = new RelayCommand<IColumn>(OnResizeColumn);
        }

        public void UpdateTables()
        {
            _tables.Clear();
            IEnumerable<ITable> tables = _dataSourceStore.GetTables();

            foreach (ITable table in tables)
            {
                _tables.Add(table);
            }
        }

        private void OnTableClicked(string tableName)
        {
            SelectedTable = _dataSourceStore.GetUpdatedTableByName(tableName);
            Columns = _dataSourceStore.GetColumnsByTableName(tableName);
            Data = _dataSourceStore.GetDataByTableName(tableName);
        }

        private void OnResizeColumn(IColumn column)
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel();
            window.Show();
        }

        private bool CanResizeColumn(IColumn column) => column.Resizable;
    }
       
}
