using Foundation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(JapanGames2.iOS.LocalizationDetector))]

namespace JapanGames2.iOS
{
    internal class LocalizationDetector : Interfaces.ILocalizationDetector
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var shortCulture = "en";

            var defaultCulture = "en-US";

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var lang = NSLocale.PreferredLanguages[0];

                shortCulture = lang.Replace("_", "-");
            }

            CultureInfo culture = null;

            try
            {
                culture = new CultureInfo(shortCulture);
            }
            catch
            {
                culture = new CultureInfo(defaultCulture);
            }

            return culture;
        }
    }
}