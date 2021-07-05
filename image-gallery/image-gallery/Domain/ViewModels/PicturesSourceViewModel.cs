using DAL.EF;
using image_gallery.Domain.Commands;
using image_gallery.Domain.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Media.Imaging;

namespace image_gallery.Domain.ViewModels
{
    public class PicturesSourceViewModel : BaseViewModel
    {
        public ObservableCollection<PictureViewModel> PictureModels { get; set; }

        private PictureViewModel _selectedPicture;

        public PictureViewModel SelectedPicture
        {
            get { return _selectedPicture; }
            set { _selectedPicture = value; OnPropertyChanged(); }
        }


        private RelayCommand _addImageCommand;

        public RelayCommand AddImageCommand
        {
            get
            {
                return _addImageCommand ?? (_addImageCommand = new(LoadImagesDialog));
            }
        }

        private RelayCommand _removeImageCommand;

        public RelayCommand RemoveImageCommand
        {
            get 
            { 
                return _removeImageCommand ?? (_removeImageCommand = new(RemoveImage)); 
            }
        }

        private RelayCommand _openImageCommand;

        public RelayCommand OpenImageCommand
        {
            get 
            {
                return _openImageCommand ?? (_openImageCommand = new RelayCommand(OpenSelectedImage)); 
            }
        }

        private int _columnAmount;

        public int ColumnAmount
        {
            get { return _columnAmount; }
            set { _columnAmount = value; OnPropertyChanged(); }
        }


        public PicturesSourceViewModel()
        {
            PicturesContext dbContext = ServiceContainer.Services.GetService<PicturesContext>();

            var pics = dbContext.Pictures.Select(dbPic => new PictureViewModel()
            {
                BitmapImage = Base64Helper.Decode(dbPic.Content),
                Id = dbPic.ID,
                Name = dbPic.Name,
                Rate = dbPic.Rate
            }).ToList();

            PictureModels = new ObservableCollection<PictureViewModel>(pics);
        }

        //Загрузка фоток из папки
        //private void LoadImagesFromFolder(object obj)
        //{
        //    using (var dialog = new FolderBrowserDialog())
        //    {
        //        DialogResult result = dialog.ShowDialog();
        //        if (result == DialogResult.OK && dialog.SelectedPath != _currentFolderPath)
        //        {
        //            _currentFolderPath = dialog.SelectedPath;
        //            string[] fileEntries = Directory.GetFiles(_currentFolderPath, "*.*", SearchOption.TopDirectoryOnly)
        //                .Where(s => s.EndsWith(".jpg", System.StringComparison.OrdinalIgnoreCase)).ToArray();

        //            PictureModels.Clear(); 
        //            foreach (string fileName in fileEntries)
        //            {
        //                PictureModels.Add(new PictureModel
        //                {
        //                    Uri = fileName,
        //                    Name = Path.GetFileName(fileName),
        //                });
        //            }
        //        }
        //    }
        //}

        private void LoadImagesDialog(object obj)
        {
            using OpenFileDialog dialog = new();
            dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            dialog.Multiselect = true;
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                PicturesContext dbContext = ServiceContainer.Services.GetService<PicturesContext>();
                foreach (string imagePath in dialog.FileNames)
                {
                    string fileName = Path.GetFileName(imagePath);

                    var newEntity = dbContext.Pictures.Add(new DAL.Entities.Picture
                    {
                        Content = Base64Helper.Encode(imagePath),
                        Name = fileName,
                        Rate = 0
                    }).Entity;

                    dbContext.SaveChanges();

                    PictureModels.Add(new PictureViewModel()
                    {
                        BitmapImage = new(new System.Uri(imagePath)),
                        Id = newEntity.ID,
                        Name = fileName,
                        Rate = 0
                    });
                }
            }
        }

        private void RemoveImage(object obj)
        {
            if (SelectedPicture == null)
            {
                return;
            }
            
            if(MessageBox.Show("Уверены, что хотите удалить?", "Внимание", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            PicturesContext dbContext = ServiceContainer.Services.GetService<PicturesContext>();

            dbContext.Pictures.Remove(dbContext.Pictures.Find(SelectedPicture.Id));
            dbContext.SaveChanges();

            PictureModels.Remove(SelectedPicture);
        }


        private void OpenSelectedImage(object obj)
        {
            ImageViewer imageViewerWindow = new();
            imageViewerWindow.Image.Source = obj as BitmapImage;

            imageViewerWindow.Show();
        }

        // Работает, но если есть выбранная фотка
        //private void UpdateRating(object obj)
        //{
        //    if(SelectedPicture == null)
        //    {
        //        return;
        //    }
        //    SelectedPicture.Rate = (int)obj;
        //    PicturesContext dbContext = ServiceContainer.Services.GetService<PicturesContext>();

        //    dbContext.Pictures.Find(SelectedPicture.Id).Rate = (int)obj;

        //    dbContext.SaveChanges();
        //}


    }
}
