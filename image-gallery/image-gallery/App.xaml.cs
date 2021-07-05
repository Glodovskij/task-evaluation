using image_gallery.Domain.ViewModels;
using System.Windows;

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
            window.DataContext = new PicturesSourceViewModel();
            window.Show();

            base.OnStartup(e);
        }        
    }
}
