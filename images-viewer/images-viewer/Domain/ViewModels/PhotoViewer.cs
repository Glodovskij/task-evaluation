using images_viewer.DAL.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace images_viewer.Domain.ViewModels
{
    public class PhotoViewer : Base
    {
        public ObservableCollection<GalleryObject> Pictures { get; set; }

        private GalleryObject _selectedPicture;

        public GalleryObject SelectedPicture
        {
            get { return _selectedPicture; }
            set { _selectedPicture = value; OnPropertyChanged(); }
        }

        private RelayCommand _starClicked;

        public RelayCommand StarClickedCommand
        {
            get { return _starClicked ?? (_starClicked = new RelayCommand(StarClickedCommandHandler)); }
        }

        private RelayCommand _leftButtonCommand;

        public RelayCommand LeftButtonCommand
        {
            get { return _leftButtonCommand ?? (_leftButtonCommand = new RelayCommand(LeftButtonCommandHandler)); }
        }

        private RelayCommand _rightButtonCommand;

        public RelayCommand RightButtonCommand
        {
            get { return _rightButtonCommand ?? (_rightButtonCommand = new RelayCommand(RightButtonCommandHandler)); }
        }


        private IUnitOfWork _unitOfWork;

        public PhotoViewer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Pictures = new();
        }

        private void LeftButtonCommandHandler(object obj)
        {
            if (Pictures.IndexOf(SelectedPicture) - 1 < 0)
            {
                SelectedPicture = Pictures.Last();
            }
            else
            {
                SelectedPicture = Pictures[Pictures.IndexOf(SelectedPicture) - 1];
            }
        }
        private void RightButtonCommandHandler(object obj)
        {
            if (Pictures.IndexOf(SelectedPicture) + 1 > Pictures.Count - 1)
            {
                SelectedPicture = Pictures.First();
            }
            else
            {
                SelectedPicture = Pictures[Pictures.IndexOf(SelectedPicture) + 1];
            }
        }

        private void StarClickedCommandHandler(object obj)
        {
            var repository = _unitOfWork.GetRepository<DAL.Entities.Picture>();
            var picture = repository.Get(pic => pic.DiscPath == SelectedPicture.Path).ToList();
            if (picture.Count != 0)
            {
                picture.First().Rate = (int)obj;
            }
            else
            {
                repository.Add(new DAL.Entities.Picture()
                {
                    Rate = (int)obj,
                    DiscPath = SelectedPicture.Path
                });
            }

            _unitOfWork.Commit();
        }
    }
}
