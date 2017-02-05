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
using MH_UWPFundamentals.ViewModels;
using Windows.UI;
using MH_UWPFundamentals.Models;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MH_UWPFundamentals.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyHorses : Page
    {
        //for validation
        private bool isHorseValid = true;
        private Color validBrush = Colors.BlanchedAlmond;
        private Color invalidBrush = Colors.Red;
        private string validationMessage = "Please review the rules below, and correct the above highlighted fields.";

        //COLLECTION VIEW SOURCE
        private MyHorsesViewModel horsesFromViewModel = new MyHorsesViewModel();

        public MyHorses()
        {
            this.InitializeComponent();
            TBPageTitle.Text = "My Horses";
        }

        //BUTTONS
        //back
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        //profile page
        private void GoToProfile_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Profile));
        }

        //group horses by discipline
        private void GroupHorses_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideGroupedHorse();

            var result =
                from t in horsesFromViewModel.Horses
                group t by t.Discipline into g
                orderby g.Key
                select g;

            horsesByDiscipline.Source = result;
        }
        //show or hide discipline grouping
        private void ShowOrHideGroupedHorse()
        {
            if (GroupedSection.Visibility == Visibility.Collapsed)
            {
                MasterSection.Visibility = Visibility.Collapsed;
                GroupedSection.Visibility = Visibility.Visible;
                AddButton.IsEnabled = false;
            }
            else
            {
                MasterSection.Visibility = Visibility.Visible;
                GroupedSection.Visibility = Visibility.Collapsed;
                AddButton.IsEnabled = true;
            }
        }


        //save
        private void IsReadyToSave_Click(object sender, RoutedEventArgs e)
        {
            ValidateUserInput();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClearValidation();
            IsReadyToSave.IsChecked = false;
        }

        //cancel
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearValidation();
            IsReadyToSave.IsChecked = false;
        }

        //EVENTS
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateUserInput();
        }
        private void txtDiscipline_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateUserInput();
        }

        //VALIDATE USER INPUT
        private void ValidateUserInput()
        {
            //checks if the user is ready to save
            if (IsReadyToSave.IsChecked == true)
            {
                isHorseValid = true;

                //checks input controls
                if (txtName.Text == "" || txtName.Text == null || txtName.Text.Count() > 65)
                {
                    isHorseValid = false;
                    txtName.BorderBrush = new SolidColorBrush(color: invalidBrush);
                }
                else
                {
                    txtName.BorderBrush = new SolidColorBrush(color: validBrush);
                }

                if (txtDiscipline.Text == "" || txtDiscipline.Text == null || txtDiscipline.Text.Count() > 65)
                {
                    isHorseValid = false;
                    txtDiscipline.BorderBrush = new SolidColorBrush(color: invalidBrush);
                }
                else
                {
                    txtDiscipline.BorderBrush = new SolidColorBrush(color: validBrush);
                }

                //apply validation if necessary
                if (isHorseValid == false)
                {
                    //horse is invalid
                    IsReadyToSave.IsChecked = false;
                    DisplayValidationMessage.Visibility = Visibility.Visible;
                    TBValidation.Text = validationMessage.ToString();
                }
                else
                {
                    //horse is valid
                    ClearValidation();
                }
            }
            else
            {
                //wait for user to validate
                SaveButton.IsEnabled = false;
            }
        }

        //CLEARS VALIDATION MESSAGES
        private void ClearValidation()
        {
            DisplayValidationMessage.Visibility = Visibility.Collapsed;
            SaveButton.IsEnabled = true;
            txtName.BorderBrush = new SolidColorBrush(color: validBrush);
        }
    }
}
