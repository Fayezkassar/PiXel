using ImageResizingApp.Models.PostgreSQL;
using Npgsql;
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
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();

            PostgreSQLDataSource dataSource = new PostgreSQLDataSource();

            Dictionary<string, string> connectionParametersMap = new Dictionary<string, string>();
            connectionParametersMap.Add("Host", "localhost:5432");
            connectionParametersMap.Add("Username", "postgres");
            connectionParametersMap.Add("Password", "mypass");
            connectionParametersMap.Add("Database", "impact");
            if (dataSource.Open(connectionParametersMap))
            {
                List<string> items = new List<string>();


                foreach (PostgreSQLTable table in dataSource.Tables)
                {
                    items.Add(table.Name);
                }

                lvDataBinding.ItemsSource = items;
            }

        }
    }
}
