using CommunityToolkit.Mvvm.Input;
using ImageResizingApp.Models;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Stores;
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

        private readonly ObservableCollection<ITable> _tables;
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

        private TableStats _tableStats;

        public TableStats TableStats
        {
            get { return _tableStats; }
            set { SetProperty(ref _tableStats, value, false); }
        }

        public RelayCommand<string> TableSelectedCommand { get; }
        public TableListingViewModel(DataSourceStore dataSourceStore) 
        {
            _dataSourceStore = dataSourceStore;

            _tables = new ObservableCollection<ITable>(_dataSourceStore.getTables());

            TableSelectedCommand = new RelayCommand<string>(TableClicked);

        }

        private void TableClicked(string tableName)
        {
            TableStats = _dataSourceStore.GetStatsByTableName(tableName);
            Columns = _dataSourceStore.GetColumnsStatsByTable(tableName);
            Data = _dataSourceStore.GetDataByTableName(tableName);
        }
    }
       
}
