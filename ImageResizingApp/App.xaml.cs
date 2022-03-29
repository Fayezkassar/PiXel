using ImageResizingApp.HostBuilders;
using ImageResizingApp.Models.DataSources.Oracle;
using ImageResizingApp.Models.DataSources.PostgreSQL;
using ImageResizingApp.Models.DataSources.SQLServer;
using ImageResizingApp.Models.Filters;
using ImageResizingApp.Stores;
using ImageResizingApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                services.AddSingleton<Registry>();
                services.AddSingleton<DataSourceStore>();
                services.AddSingleton(s => new MainWindow(s.GetRequiredService<Registry>(), s.GetRequiredService<DataSourceStore>())
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            }).Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            _host.Services.GetRequiredService<Registry>().AddFilter("Scale", new ScaleFilter("Scale"));
            _host.Services.GetRequiredService<Registry>().AddFilter("Resize", new ResizeFilter("Resize"));
            _host.Services.GetRequiredService<Registry>().AddFilter("Sample", new SampleFilter("Sample"));
            _host.Services.GetRequiredService<Registry>().AddFilter("Liquid Rescale", new LiquidRescaleFilter("Liquid Rescale"));

            _host.Services.GetRequiredService<Registry>().AddDataSource("SQL Server", new SQLServerDataSource());
            _host.Services.GetRequiredService<Registry>().AddDataSource("PostgreSQL", new PostgreSQLDataSource());
            _host.Services.GetRequiredService<Registry>().AddDataSource("Oracle", new OracleDataSource());
            _host.Services.GetRequiredService<MainWindow>().Show();

            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {

            _host.Services.GetRequiredService<DataSourceStore>().CloseDataSourceConnectionIfAny();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
