using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsSyncViewer.Database;
using ContactsSyncViewer.Services;
using Xamarin.Forms;

namespace ContactsSyncViewer
{
    public partial class MainPage : ContentPage
    {
        // safe double click
        bool UIlocker = false;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ContactsList.ItemsSource = DBSingletone.Database.GetItems();
            base.OnAppearing();
        }
        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Contact selectedFriend = (Contact)e.SelectedItem;

        }

        private void DeleteClick(object sender, EventArgs args)
        {
            DBSingletone.Database.DeleteAll();

        }


        private async void SychClick(object sender, EventArgs args)
        {
            if (!UIlocker)
            {
                UIlocker = true;
                bool answer = await DisplayAlert("Warning", "Do You want to synchronize contacts?", "Yes", "No");

                if (answer)
                {
                    ChangeIndicatorState(true, true);

                    Indicator.Color = Color.FromHex("#2196F3");

                    ChangeSynchLabel("Synchronization", Color.DarkOrange);

                     var synchComplete = await Task.Run(() => new Synchronization().Synchronize());

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContactsList.BeginRefresh();
                        ContactsList.ItemsSource = DBSingletone.Database.GetItems();
                        ContactsList.EndRefresh();

                        ChangeIndicatorState(false, false);

                        if (synchComplete)
                            ChangeSynchLabel("Synchronization Complete", Color.Green);
                        else
                            ChangeSynchLabel("Synchronization Error", Color.Red);

                        UIlocker = false;
                    });
                }
                else
                    UIlocker = false;
            }
        }


        private void ChangeIndicatorState(bool isVisible, bool isRunning)
        {
            Indicator.IsVisible = isVisible;
            Indicator.IsRunning = isRunning;          
        }

        private void ChangeSynchLabel(string text, Color textColor)
        {
            SynchLabel.Text = text;
            SynchLabel.TextColor = textColor;
           
        }
    }
}
