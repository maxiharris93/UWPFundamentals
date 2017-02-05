using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MH_UWPFundamentals.Models;
using Windows.UI.Xaml.Media;
using MH_UWPFundamentals.Mvvm;
using MH_UWPFundamentals.Extensions;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace MH_UWPFundamentals.ViewModels
{
    internal class HorseViewModel : ViewModelBase
    {
        private Horse model;
        private ImageSource picture = null;
        private DelegateCommand uploadImageCommand;

        public HorseViewModel(Horse model)
        {
            this.model = model;
            this.uploadImageCommand = new DelegateCommand(this.UploadImage_Executed);
        }

        public Horse Model
        {
            get
            {
                return this.model;
            }

            set
            {
                this.SetProperty(ref this.model, value);
                this.picture = null;
                this.OnPropertyChanged(string.Empty);
            }
        }

        public int Id
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.Id;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Id = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Name;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Name = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public int Age
        {
            get
            {
                if (this.model == null)
                {
                    return 0;
                }

                return this.model.Age;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Age = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Discipline
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Discipline;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Discipline = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Notes
        {
            get
            {
                if (this.model == null)
                {
                    return string.Empty;
                }

                return this.model.Notes;
            }

            set
            {
                if (this.model != null)
                {
                    this.model.Notes = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                if (this.model != null && this.picture == null)
                {
                    this.picture = this.model.Picture.AsBitmapImage();
                }

                return this.picture;
            }
        }

        public byte[] Picture
        {
            set
            {
                this.picture = null;
                this.model.Picture = value;
                this.OnPropertyChanged("ImageSource");
            }
        }

        public ICommand UploadImageCommand
        {
            get { return this.uploadImageCommand; }
        }

        private async void UploadImage_Executed()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile imgFile = await openPicker.PickSingleFileAsync();
            if (imgFile != null)
            {
                this.Picture = await imgFile.AsByteArray();
            }
        }
    }
}
