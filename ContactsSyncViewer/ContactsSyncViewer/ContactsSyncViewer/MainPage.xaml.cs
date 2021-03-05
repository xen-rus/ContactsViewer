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
                bool answer = await DisplayAlert("Внимание", "Хотите ли Вы синхронизировать контакты?", "Да", "Нет");

                if (answer)
                {
                    Indicator.IsVisible = true;
                    Indicator.IsRunning = true;
                    Indicator.Color = Color.FromHex("#2196F3");
                    SynchLabel.Text = "Синхронизация";
                    SynchLabel.TextColor = Color.DarkOrange;

                     var synchComplete = await Task.Run(() => new Synchronization().Synchronize());

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContactsList.BeginRefresh();
                        ContactsList.ItemsSource = DBSingletone.Database.GetItems();
                        ContactsList.EndRefresh();

                        Indicator.IsVisible = false;
                        Indicator.IsRunning = false;

                        if (synchComplite)
                        {
                            SynchLabel.Text = "Синхронизация Завершена";
                            SynchLabel.TextColor = Color.Green;
                        }
                        else
                        {
                            SynchLabel.Text = "Ошибка Синхронизации";
                            SynchLabel.TextColor = Color.Red;
                        }

                        UIlocker = false;
                    });
                }
                else
                    UIlocker = false;
            }
        }
    }
}
