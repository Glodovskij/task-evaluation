using DAL.EF;
using image_gallery.Domain.Services;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;

namespace image_gallery.Domain.ViewModels
{
    public class PictureViewModel : BaseViewModel
    {
        private readonly PicturesContext _dbContext;
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
                if (_dbContext.Pictures.Find(this.Id).Rate != value)
                {
                    _dbContext.Pictures.Find(this.Id).Rate = value;
                    _dbContext.SaveChanges();
                }

            }
        }

        public PictureViewModel()
        {
            _dbContext = ServiceContainer.Services.GetService<PicturesContext>();
        }

    }
}
