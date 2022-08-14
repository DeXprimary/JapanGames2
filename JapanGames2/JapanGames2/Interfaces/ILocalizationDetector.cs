using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JapanGames2.Interfaces
{
    public interface ILocalizationDetector
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
