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

            List<string> asd = new List<string>();
            asd.Add("bgFilling");

            MainPage = new NavigationPage(new PageMainMenu())
            {
                //BarBackgroundColor = Color.FromHex("#eff5ff"),
                //StyleClass = asd,
                Padding = new Thickness(0, 0, 0, 0),
                BarTextColor = Color.Black,
                HeightRequest = 10,
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
