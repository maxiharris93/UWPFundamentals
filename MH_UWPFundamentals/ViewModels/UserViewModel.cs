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
    internal class UserViewModel : ViewModelBase
    {
        //will use this when I add more fields to the User
        /*private User user;

        public UserViewModel(User user)
        {
            this.user = user;
        }

        public User Model
        {
            get
            {
                return this.user;
            }

            set
            {
                this.SetProperty(ref this.user, value);
                this.OnPropertyChanged(string.Empty);
            }
        }

        public int Id
        {
            get
            {
                if (this.user == null)
                {
                    return 0;
                }

                return this.user.Id;
            }

            set
            {
                if (this.user != null)
                {
                    this.user.Id = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                if (this.user == null)
                {
                    return string.Empty;
                }

                return this.user.Name;
            }

            set
            {
                if (this.user != null)
                {
                    this.user.Name = value;
                    this.OnPropertyChanged();
                }
            }
        }*/
    }
}
