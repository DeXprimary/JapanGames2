using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JapanGames2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
             
            MainPage = new NavigationPage(new PageMainMenu())
            {
                BarTextColor = (Color)Application.Current.Resources["Color_MenuText"],
                BarBackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"],
                BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"],
                Padding = new Thickness(0, 0, 0, 0)
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
