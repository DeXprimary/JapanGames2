using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(JapanGames2.Droid.LocalizationDetector))]

namespace JapanGames2.Droid
{
    internal class LocalizationDetector : Interfaces.ILocalizationDetector
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var currentCulture = Java.Util.Locale.Default;
                      
            return new CultureInfo(currentCulture.ToLanguageTag());
        }
    }
}