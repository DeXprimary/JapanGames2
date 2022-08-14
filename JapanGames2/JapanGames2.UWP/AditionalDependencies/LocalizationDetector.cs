using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(JapanGames2.UWP.LocalizationDetector))]

namespace JapanGames2.UWP
{
    internal class LocalizationDetector : Interfaces.ILocalizationDetector
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return CultureInfo.CurrentUICulture;
        }
    }
}
