using ImageResizingApp.HostBuilders;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Models.Oracle;
<<<<<<< HEAD
using ImageResizingApp.Models.PostgreSQL;
=======
using ImageResizingApp.Stores;
>>>>>>> FYP-21
using ImageResizingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ImageResizingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices(services =>

            {
                services.AddSingleton(new DataSourceRegistry());
                services.AddSingleton(new DataSourceStore());
                services.AddSingleton(s => new MainWindow(s.GetRequiredService<DataSourceRegistry>())
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            }).Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

<<<<<<< HEAD
            _host.Services.GetRequiredService<DataSourceRegistry>().AddDataSource("SQL Server", new SQLServerDataSource());
            _host.Services.GetRequiredService<DataSourceRegistry>().AddDataSource("PostgreSQL", new PostgreSQLDataSource());
=======
            List<string> connectionParameters = new List<string>();
            connectionParameters.Add("Username");
            connectionParameters.Add("Password");
            connectionParameters.Add("InitialCatalog");
            _host.Services.GetRequiredService<DataSourceRegistry>().AddDataSource("SQL Server", new SQLServerDataSource(connectionParameters));
>>>>>>> FYP-21
            _host.Services.GetRequiredService<MainWindow>().Show();

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {

            _host.Dispose();

            base.OnExit(e);
        }
    }
}
