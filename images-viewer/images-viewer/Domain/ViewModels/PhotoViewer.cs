﻿using images_viewer.DAL.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace images_viewer.Domain.ViewModels
{
    public class PhotoViewer : Base
    {
        public ObservableCollection<Picture> Pictures { get; set; }

        private Picture _selectedPicture;

        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set { _selectedPicture = value; OnPropertyChanged(); }
        }

        private RelayCommand _starClicked;

        public RelayCommand StarClickedCommand
        {
            get { return _starClicked ?? (_starClicked = new RelayCommand(StarClickedCommandHandler)); }
        }


        private IUnitOfWork _unitOfWork;

        public PhotoViewer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Pictures = new();
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
