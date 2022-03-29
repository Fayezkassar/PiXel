﻿using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
using ImageResizingApp.Views.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Media.Imaging;

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

        public RelayCommand<DataRowView> ViewImage { get; }
        public RelayCommand<string> TableSelectedCommand { get; }
        public TableListingViewModel(DataSourceStore dataSourceStore)
        {
            _dataSourceStore = dataSourceStore;
            _tables = new ObservableCollection<ITable>(_dataSourceStore.GetTables());
            _dataSourceStore.CurrentDataSourceChanged += UpdateTables;
            TableSelectedCommand = new RelayCommand<string>(OnTableClicked);
            ResizeColumnCommand = new RelayCommand<IColumn>(OnResizeColumn, CanResizeColumn);
            ViewImage = new RelayCommand<DataRowView>(OnViewImage);
            FirstCommand = new RelayCommand(OnFirstCommand);
            PreviousCommand = new RelayCommand(OnPreviousCommand);
            NextCommand = new RelayCommand(OnNextCommand);
            LastCommand = new RelayCommand(OnLastCommand);
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
            currentTable = tableName;
            SelectedTable = _dataSourceStore.GetUpdatedTableByName(currentTable);
            TotalItems = int.Parse(SelectedTable.RecordsNumber);
            Columns = _dataSourceStore.GetColumnsByTableName(currentTable);
            Data = _dataSourceStore.GetDataByTableName(currentTable, start, itemCount);
        }

        private void OnResizeColumn(IColumn column)
        {
            ResizeConfigurationWindow window = new ResizeConfigurationWindow();
            window.DataContext = new ResizeConfigurationWindowViewModel(column);
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
        private string currentTable;
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

        private void OnFirstCommand()
        {
            if (start > 0)
            {
                Start = 0;
                Data = _dataSourceStore.GetDataByTableName(currentTable, start, itemCount);
            }
        }

        private void OnPreviousCommand()
        {
            if (start >= itemCount)
            {
                Start -= itemCount;
                Data = _dataSourceStore.GetDataByTableName(currentTable, start, itemCount);
            }
        }

        private void OnNextCommand()
        {
            if (start < totalItems - itemCount)
            {
                Start += itemCount;
                Data = _dataSourceStore.GetDataByTableName(currentTable, start, itemCount);
            }
        }

        private void OnLastCommand()
        {
            if (start < totalItems - itemCount)
            {
                Start = totalItems - itemCount;
                Data = _dataSourceStore.GetDataByTableName(currentTable, start, itemCount);
            }
        }

    }
}
