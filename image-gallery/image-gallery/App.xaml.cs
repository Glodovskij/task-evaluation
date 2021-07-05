using DAL.EF;
using image_gallery.Domain.ViewModels;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using image_gallery.Domain.Services;

namespace image_gallery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow();
            window.DataContext = new PicturesSourceViewModel(ServiceContainer.Services.GetService<PicturesContext>());
            window.Show();

            base.OnStartup(e);
        }        
    }
}
