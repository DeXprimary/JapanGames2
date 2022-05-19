using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JapanGames2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainMenu())
            {
                BarBackgroundColor = Color.FromHex("#afcfff"),
                Padding = new Thickness(0, 0, 0, 0),
                BarTextColor = Color.FromHex("#000001"),
                HeightRequest = 10,
                IsVisible = true
                //BarBackground = Brush.DeepPink

            };

            

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
