using MH_UWPFundamentals.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MH_UWPFundamentals.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        public Profile()
        {
            this.InitializeComponent();
            TBPageTitle.Text = "Academy Profile";

            //retrieve settings
            UserName.Text = App.AcademyUserName;

            if (App.AcademyUserName.ToString() == "")
            {
                UserName.Text = "Anonymous";
            }

            //ui cleanup
            HideEditSection();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        //INITIATE UPDATE
        private void EditDetails_Click(object sender, RoutedEventArgs e)
        {
            HideViewSection();
            ShowEditSection();
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            App.AcademyUserName = EditUserName.Text;
            UserName.Text = App.AcademyUserName;
            App.SaveAppState();
            HideEditSection();
            ShowViewSection();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            HideEditSection();
            ShowViewSection();
        }

        /*HIDE OR SHOW SECTIONS*/
        private void HideViewSection()
        {
            ViewSection.Visibility = Visibility.Collapsed;
        }

        private void ShowViewSection()
        {
            ViewSection.Visibility = Visibility.Visible;
        }

        private void HideEditSection()
        {
            EditSection.Visibility = Visibility.Collapsed;
        }

        private void ShowEditSection()
        {
            EditSection.Visibility = Visibility.Visible;
            if (App.AcademyUserName != null)
            {
                UserName.Text = App.AcademyUserName;
            }
            else
            {
                UserName.Text = "Anonymous";
            }
        }

        private async void ResetProfile_Click(object sender, RoutedEventArgs e)
        {
            await DataAccessLayer.CreateDatabase();
            App.AcademyUserName = "Anonymous";
            App.SaveAppState();
            Frame.Navigate(typeof(Dash));
        }
    }
}
