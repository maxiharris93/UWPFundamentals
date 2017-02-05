using MH_UWPFundamentals.ViewModels;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
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
    public sealed partial class MyLessons : Page
    {
        //for validation
        private bool isLessonValid = true;
        private Color validBrush = Colors.BlanchedAlmond;
        private Color invalidBrush = Colors.Red;
        private string validationMessage = "Please review the rules below, and correct the above highlighted fields.";

        //for the toast notification
        private string selectedLessonDateString;

        public MyLessons()
        {
            this.InitializeComponent();
            TBPageTitle.Text = "My Lessons";

            DateTime startDate = new DateTime(2017, 1, 1);
            dpLessonDate.MinYear = new DateTimeOffset(startDate);
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
            selectedLessonDateString = dpLessonDate.Date.ToString();
            InitializeTile();
            InitializeToast();
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
        private void txtFocus_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateUserInput();
        }

        //PREVENT USER FROM ENTERING NON-NUMBERS
        private void txtCost_KeyDown(object sender, KeyRoutedEventArgs e)
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

        //VALIDATES USER INPUT
        private void ValidateUserInput()
        {
            //checks if the user is ready to save
            if (IsReadyToSave.IsChecked == true)
            {
                isLessonValid = true;

                //validate number
                string cost = txtCost.Text;
                int validCost;
                bool isCostValid;
                isCostValid = int.TryParse(cost, out validCost);

                //checks input controls
                if (isCostValid == false)
                {
                    isLessonValid = false;
                    txtCost.BorderBrush = new SolidColorBrush(color: invalidBrush);
                }
                else
                {
                    txtCost.BorderBrush = new SolidColorBrush(color: validBrush);
                }

                if (txtFocus.Text == "" || txtFocus.Text == null || txtFocus.Text.Count() > 65)
                {
                    isLessonValid = false;
                    txtFocus.BorderBrush = new SolidColorBrush(color: invalidBrush);
                }
                else
                {
                    txtFocus.BorderBrush = new SolidColorBrush(color: validBrush);
                }

                //apply validation if necessary
                if (isLessonValid == false)
                {
                    //lesson is invalid
                    IsReadyToSave.IsChecked = false;
                    DisplayValidationMessage.Visibility = Visibility.Visible;
                    TBValidation.Text = validationMessage.ToString();
                }
                else
                {
                    //lesson is valid
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
            txtCost.BorderBrush = new SolidColorBrush(color: validBrush);
            txtFocus.BorderBrush = new SolidColorBrush(color: validBrush);
        }

        //INITIALIZE TILE
        private void InitializeTile()
        {
            //tile visual
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = "Next riding lesson:"
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = selectedLessonDateString
                                    }
                                }
                        }
                    }
                }
            };
            var tileNotification = new TileNotification(content.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        //INITIALIZE TOAST NOTIFICATION
        private void InitializeToast()
        {
            //toast visual
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "This notification is supposed to show at the following time and date:"
                            },
                            new AdaptiveText()
                            {
                                Text = selectedLessonDateString
                            }
                        }
                }
            };
            ToastContent toastContent = new ToastContent() { Visual = visual };

            ScheduledToastNotification toast = new ScheduledToastNotification(toastContent.GetXml(), DateTimeOffset.Now.AddSeconds(5));
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
        }
    }
}
