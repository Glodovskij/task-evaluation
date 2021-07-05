using DAL.EF;
using image_gallery.Domain.Services;
using image_gallery.Domain.ViewModels;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;

namespace image_gallery.Domain.ViewModels
{
    public class PictureViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public BitmapImage BitmapImage { get; set; }
        public string Name { get; set; }

        private int _rate;

        public int Rate
        {
            get { return _rate; }
            set 
            { 
                _rate = value; OnPropertyChanged();
                PicturesContext picContext = ServiceContainer.Services.GetService<PicturesContext>();
                if (picContext.Pictures.Find(this.Id).Rate != value)
                {
                    picContext.Pictures.Find(this.Id).Rate = value;
                    picContext.SaveChanges();
                }

            }
        }

    }
}
