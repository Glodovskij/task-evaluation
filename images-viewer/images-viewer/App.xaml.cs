using images_viewer.DAL;
using images_viewer.DAL.EF;
using images_viewer.DAL.Interfaces;
using images_viewer.DAL.Repositories;
using images_viewer.Domain.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace images_viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {

            ServiceCollection services = new ();
            services.AddScoped<PicDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRepository<DAL.Entities.Picture>, Repository<DAL.Entities.Picture>>();
            services.AddSingleton<Gallery>();

            _serviceProvider = services.BuildServiceProvider();

            Window window = new MainWindow();
            window.DataContext = _serviceProvider.GetService<Gallery>();
            window.Show();


            base.OnStartup(e);
        }
    }
}
