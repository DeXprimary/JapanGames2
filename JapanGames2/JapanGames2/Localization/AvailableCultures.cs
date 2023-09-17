using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JapanGames2.Localization
{
    public static class AvailableCultures
    {
        public static CultureInfo[] cultures = new CultureInfo[]
        {
            CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(obj => obj.EnglishName == "English").First(),
            CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(obj => obj.EnglishName == "Russian").First()
        };
    }
}
