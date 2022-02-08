using ImageResizingApp.HostBuilders;
using ImageResizingApp.Models.Interfaces;
using ImageResizingApp.Models.Oracle;
using ImageResizingApp.Models.PostgreSQL;
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
                services.AddSingleton(s => new MainWindow(s.GetRequiredService<DataSourceRegistry>())
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            }).Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            _host.Services.GetRequiredService<DataSourceRegistry>().AddDataSource("SQL Server", new SQLServerDataSource());
            _host.Services.GetRequiredService<DataSourceRegistry>().AddDataSource("PostgreSQL", new PostgreSQLDataSource());
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
