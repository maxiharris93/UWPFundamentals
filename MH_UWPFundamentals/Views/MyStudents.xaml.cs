using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class MyStudents : Page
    {
        //for validation
        private bool isStudentValid = true;
        private Color validBrush = Colors.BlanchedAlmond;
        private Color invalidBrush = Colors.Red;
        private string validationMessage = "Please review the rules below, and correct the above highlighted fields.";

        public MyStudents()
        {
            this.InitializeComponent();
            TBPageTitle.Text = "My Students";
        }

        //BACK BUTTON
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        //PROFILE PAGE
        private void GoToProfile_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Profile));
        }

        //SAVE
        private void IsReadyToSave_Click(object sender, RoutedEventArgs e)
        {
            ValidateUserInput();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClearValidation();
            IsReadyToSave.IsChecked = false;
        }

        //CANCEL
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearValidation();
            IsReadyToSave.IsChecked = false;
        }

        //HANDLE CHANGE TO INPUT
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateUserInput();
        }
        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateUserInput();
        }

        //PREVENT USER FROM ENTERING NON-NUMBERS
        private void txtNumber_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.ToString().Equals("Back"))
            {
                e.Handled = false;
                return;
            }
            for (int i = 0; i < 10; i++)
            {
                if (e.Key.ToString() == string.Format("Number{0}", i))
                {
                    e.Handled = false;
                    return;
                }
            }
            e.Handled = true;
        }

        //VALIDATE USER INPUT
        private void ValidateUserInput()
        {
            //checks if the user is ready to save
            if (IsReadyToSave.IsChecked == true)
            {
                isStudentValid = true;

                //validate number
                string number = txtNumber.Text;
                int validNumber;
                bool isNumberValid;
                isNumberValid = int.TryParse(number, out validNumber);

                //checks input controls
                if (txtName.Text == "" || txtName.Text == null || txtName.Text.Count() > 65)
                {
                    isStudentValid = false;
                    txtName.BorderBrush = new SolidColorBrush(color: invalidBrush);
                }
                else
                {
                    txtName.BorderBrush = new SolidColorBrush(color: validBrush);
                }

                if (txtNumber.Text == "" || txtNumber.Text == null || txtNumber.Text.Count() > 11 || isNumberValid == false)
                {
                    isStudentValid = false;
                    txtNumber.BorderBrush = new SolidColorBrush(color: invalidBrush);
                }
                else
                {
                    txtNumber.BorderBrush = new SolidColorBrush(color: validBrush);
                }

                //apply validation if necessary
                if (isStudentValid == false)
                {
                    //student is invalid
                    IsReadyToSave.IsChecked = false;
                    DisplayValidationMessage.Visibility = Visibility.Visible;
                    TBValidation.Text = validationMessage.ToString();
                }
                else
                {
                    //student is valid
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
            txtNumber.BorderBrush = new SolidColorBrush(color: validBrush);
        }
    }
}
