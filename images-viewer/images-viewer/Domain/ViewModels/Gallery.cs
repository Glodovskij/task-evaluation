using images_viewer.DAL.Interfaces;
using images_viewer.Domain.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace images_viewer.Domain.ViewModels
{
    public class Gallery : Base
    {
        public ObservableCollection<Picture> Pictures { get; private set; }
        public ObservableCollection<TreeViewNode> FolderTreeView { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        private Picture _selectedPicture;

        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set { _selectedPicture = value; OnPropertyChanged(); }
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


        private RelayCommand _expandTreeViewCommand;

        public RelayCommand ExpandTreeViewCommand
        {
            get { return _expandTreeViewCommand ?? (_expandTreeViewCommand = new(ExpandTreeViewCommandHandler)); }
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
            Pictures = new();

            FolderTreeView = new();
            FolderTreeView.Add(new TreeViewNode()
            {
                Header = "Этот компьютер",
                Path = null,
            });

            foreach (var folder in Directory.GetLogicalDrives())
            {
                FolderTreeView.FirstOrDefault().Nodes.Add(new() { Header = folder, Path = folder });
            }

            CurrentViewModel = this;
        }

        private void ExpandTreeViewCommandHandler(object obj)
        {
            TreeViewItem treeViewItem = obj as TreeViewItem;
            if(treeViewItem.Tag == null)
            {
                return;
            }
            try
            {
                LoadImagesFromFolder(treeViewItem.Tag.ToString());

                TreeViewNode expandedNode = TreeViewNode.GetNode(FolderTreeView.FirstOrDefault(), treeViewItem.Tag.ToString());

                foreach (var folder in Directory.GetDirectories(treeViewItem.Tag.ToString()))
                {
                    TreeViewNode folderNode = new TreeViewNode()
                    {
                        Header = folder.Substring(folder.LastIndexOf("\\") + 1),
                        Path = folder
                    };

                    expandedNode.Nodes.Add(folderNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                //Часть папок невозможно открыть из-за безопасности
            }
        }

        private void LoadImagesFromFolder(string path)
        {
            Pictures.Clear();
            foreach (var folder in Directory.GetDirectories(path))
            {
                string fileName = folder;
                Pictures.Add(new Picture()
                {
                    FolderPath = folder,
                    Path = AppDomain.CurrentDomain.BaseDirectory + "//folder.png",
                    Name = folder.Substring(folder.LastIndexOf("\\") + 1),
                    Rate = 0
                });
            }
            var filesQuery = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".bmp") || s.EndsWith(".jpg") || s.EndsWith(".gif") || s.EndsWith(".ico")
            || s.EndsWith(".png") || s.EndsWith(".wdp") || s.EndsWith(".tiff"));
            foreach (var file in filesQuery)
            {
                string fileName = Path.GetFileName(file);
                Picture pictureFromFolder = new Picture()
                {
                    Path = file,
                    Name = fileName,
                };

                try
                {
                    pictureFromFolder.Rate = _unitOfWork.GetRepository<DAL.Entities.Picture>().Get(pic => pic.DiscPath == file).First().Rate;
                }
                catch
                {
                    pictureFromFolder.Rate = 0;
                }

                Pictures.Add(pictureFromFolder);
            }
        }

        private void OpenImageCommandHandler(object obj)
        {
            string pngFolderPath = AppDomain.CurrentDomain.BaseDirectory + "//folder.png";
            Picture chosenPic = obj as Picture;
            if (chosenPic.Path == AppDomain.CurrentDomain.BaseDirectory + "//folder.png")
            {
                try
                {
                    LoadImagesFromFolder((obj as Picture).FolderPath);
                }
                catch (UnauthorizedAccessException)
                {
                    //Часть папок невозможно открыть из-за безопасности
                }
            }
            else
            {
                var photoViewer = new PhotoViewer(_unitOfWork)
                {
                    Pictures = new ObservableCollection<Picture>(this.Pictures.Where(pic => pic.Path.EndsWith(pngFolderPath) == false).ToList()),
                    SelectedPicture = this.SelectedPicture
                };
                CurrentViewModel = photoViewer;
            }
        }

        private void BackToGalleryCommandHandler(object obj)
        {
            CurrentViewModel = this;
        }

        private void ViewPhotoCommandHandler(object obj)
        {
            string pngFolderPath = AppDomain.CurrentDomain.BaseDirectory + "//folder.png";
            if (SelectedPicture == null || SelectedPicture.Path == pngFolderPath || !Pictures.Contains(SelectedPicture))
            {
                MessageBox.Show("You need to pick image to view it");
            }
            else
            {
                var photoViewer = new PhotoViewer(_unitOfWork)
                {
                    Pictures = new ObservableCollection<Picture>(this.Pictures.Where(pic => pic.Path.EndsWith(pngFolderPath) == false).ToList()),
                    SelectedPicture = this.SelectedPicture
                };
                CurrentViewModel = photoViewer;
            }
        }
    }
}
