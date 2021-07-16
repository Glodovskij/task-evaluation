using images_viewer.DAL.Interfaces;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace images_viewer.Domain.ViewModels
{
    public class Gallery : Base
    {
        public ObservableCollection<GalleryObject> GalleryObjects { get; private set; }
        public ObservableCollection<TreeViewNode> FolderTreeView { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        private GalleryObject _selectedPicture;

        public GalleryObject SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                if (value is Picture)
                {
                    _selectedPicture = value;
                    OnPropertyChanged();
                }
            }
        }

        private TreeViewNode _treeViewItem;

        public TreeViewNode SelectedNode
        {
            get { return _treeViewItem; }
            set { _treeViewItem = value; OnPropertyChanged(); }
        }

        private Base _currentVm;

        public Base CurrentViewModel
        {
            get { return _currentVm; }
            set { _currentVm = value; OnPropertyChanged(); }
        }

        private RelayCommand _openImageCommand;

        public RelayCommand OpenImageCommand
        {
            get { return _openImageCommand ?? (_openImageCommand = new(OpenImageCommandHandler)); }
        }

        private RelayCommand _backToGalleryCommand;

        public RelayCommand BackToGalleryCommand
        {
            get { return _backToGalleryCommand ?? (_backToGalleryCommand = new(BackToGalleryCommandHandler)); }
        }

        private RelayCommand _viewPhotoCommand;

        public RelayCommand ViewPhotoCommand
        {
            get { return _viewPhotoCommand ?? (_viewPhotoCommand = new(ViewPhotoCommandHandler)); }
        }



        public Gallery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            GalleryObjects = new();

            FolderTreeView = new();
            FolderTreeView.Add(new TreeViewNode()
            {
                Header = "Этот компьютер",
                Path = null,
            });

            foreach (var folder in Directory.GetLogicalDrives())
            {
                FolderTreeView.FirstOrDefault().Nodes.Add(new() { Header = folder, Path = folder, Nodes = new ObservableCollection<TreeViewNode>() { null } });
            }

            TreeViewNode.Expanded += TreeViewNode_Expanded;

            CurrentViewModel = this;
        }

        private void TreeViewNode_Expanded(string path)
        {
            LoadImagesFromFolder(path);
        }

        private void LoadImagesFromFolder(string path)
        {
            GalleryObjects.Clear();
            Task.Run(() =>
            {
                foreach (var folder in Directory.GetDirectories(path))
                {
                    string fileName = folder;
                    App.Current.Dispatcher.Invoke(() =>
                    GalleryObjects.Add(new Folder()
                    {
                        Path = folder,
                        Name = folder.Substring(folder.LastIndexOf("\\") + 1),
                    }));
                }

                var filesQuery = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".gif") || s.EndsWith(".ico")
                || s.EndsWith(".png") || s.EndsWith(".wdp") || s.EndsWith(".tiff"));

                foreach (string file in filesQuery)
                {
                    Picture pictureFromFolder = new Picture()
                    {
                        Path = file,
                        Name = Path.GetFileName(file),
                    };

                    try
                    {
                        pictureFromFolder.Rate = _unitOfWork.GetRepository<DAL.Entities.Picture>().Get(pic => pic.DiscPath == file).First().Rate;
                    }
                    catch
                    {
                        pictureFromFolder.Rate = 0;
                    }

                    App.Current.Dispatcher.Invoke(() => GalleryObjects.Add(pictureFromFolder));
                }
            });
        }

        private void OpenImageCommandHandler(object obj)
        {
            if (obj is Folder)
            {
                LoadImagesFromFolder((obj as Folder).Path);
            }
            else
            {
                InitializePhotoViewerVIewModel();
            }
        }

        private void BackToGalleryCommandHandler(object obj)
        {
            CurrentViewModel = this;
            SelectedPicture = null;
        }

        private void ViewPhotoCommandHandler(object obj)
        {
            if (SelectedPicture == null || !GalleryObjects.Contains(SelectedPicture))
            {
                MessageBox.Show("You need to pick image to view it");
            }
            else
            {
                InitializePhotoViewerVIewModel();
            }
        }

        private void InitializePhotoViewerVIewModel()
        {
            PhotoViewer photoViewer = new PhotoViewer(_unitOfWork)
            {
                Pictures = new ObservableCollection<GalleryObject>(GalleryObjects.Where(pic => pic is Picture).ToList()),
                SelectedPicture = this.SelectedPicture
            };

            CurrentViewModel = photoViewer;
        }
    }
}
