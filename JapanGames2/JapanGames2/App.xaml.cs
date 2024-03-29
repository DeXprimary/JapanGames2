﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JapanGames2
{
    public partial class App : Application
    {
        public event EventHandler Sleeped = delegate { };

        public event EventHandler Resumed = delegate { };

        public App()
        {
            if (!App.Current.Properties.ContainsKey("CurrentLanguage")) 
                
                App.Current.Properties.Add("CurrentLanguage", DependencyService.Get<Interfaces.ILocalizationDetector>().GetCurrentCultureInfo());

            Localization.ResourceLang.Culture = (CultureInfo)App.Current.Properties["CurrentLanguage"];
                       
            InitializeComponent();
            
            MainPage = new NavigationPage(new View.MainMenuPage())
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
            Sleeped.Invoke(this, null);
        }

        protected override void OnResume()
        {
            Resumed.Invoke(this, null);
        }
    }
}
