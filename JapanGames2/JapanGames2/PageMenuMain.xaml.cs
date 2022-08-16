using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JapanGames2.Localization;

namespace JapanGames2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMainMenu : ContentPage
    {
        async void OnClickedButtonSudoku(object sender, EventArgs args)
        {
            byte difficulty;

            Button button = sender as Button;

            button.IsEnabled = false;

            string resultOfChoice = await DisplayActionSheet(
                ResourceLang.ActionSheet_Difficulty, ResourceLang.Common_Cancel, null,
                ResourceLang.ActionSheet_Easy, ResourceLang.ActionSheet_Normal,
                ResourceLang.ActionSheet_Hard, ResourceLang.ActionSheet_Insane);

            if (resultOfChoice == ResourceLang.ActionSheet_Easy) difficulty = 1;

            else if (resultOfChoice == ResourceLang.ActionSheet_Normal) difficulty = 2;

            else if (resultOfChoice == ResourceLang.ActionSheet_Hard) difficulty = 3;

            else if (resultOfChoice == ResourceLang.ActionSheet_Insane) difficulty = 4;

            else difficulty = 0;

            /*
            switch (await DisplayActionSheet(ResourceLang.ActionSheet_Difficulty, ResourceLang.Common_Cancel, null, 
                ResourceLang.ActionSheet_Easy, ResourceLang.ActionSheet_Normal,
                ResourceLang.ActionSheet_Hard, ResourceLang.ActionSheet_Insane))
            {
                case ResourceLang.ActionSheet_Easy:    difficulty = 1; break;

                case "Normal":  difficulty = 2; break;

                case "Hard":    difficulty = 3; break;

                case "Insane":  difficulty = 4; break;

                default:        difficulty = 0; break;
            }            
            */

            if (difficulty != 0) 

                await Navigation.PushAsync(new PageSudoku(difficulty), true);

            button.IsEnabled = true;
        }

        async void OnClickedButtonJapCross(object sender, EventArgs args)
        {
            Button button = sender as Button;

            button.IsEnabled = false;

            await Navigation.PushAsync(new PageSudoku(), true);

            button.IsEnabled = true;
        }

        async void OnClickedButtonAbout(object sender, EventArgs args)
        {
            Button button = sender as Button;

            button.IsEnabled = false;

            await Navigation.PushAsync(new PageAbout(), true);
            
            button.IsEnabled = true;
        }

        public PageMainMenu()
        {
            InitializeComponent();
            /*
            langPicker.Title = CultureInfo.CurrentUICulture.ToString();

            //ResourceLang.ResourceManager.

            //CultureInfo.CurrentUICulture = new CultureInfo("en-EN");

            langPicker.ItemsSource = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            
            var temp = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

            var index = 5;
            
            welcomeLabel.Text =
                temp[index].DisplayName + Environment.NewLine +
                temp[index].EnglishName + Environment.NewLine +
                temp[index].IetfLanguageTag + Environment.NewLine +
                temp[index].LCID + Environment.NewLine +
                temp[index].Name + Environment.NewLine +
                temp[index].NativeName + Environment.NewLine +
                temp[index].TextInfo + Environment.NewLine +
                temp[index].ThreeLetterISOLanguageName + Environment.NewLine +
                temp[index].ThreeLetterWindowsLanguageName + Environment.NewLine +
                temp[index].TwoLetterISOLanguageName + Environment.NewLine;*/
                //CultureInfo.GetCultureInfo("Russian").NativeName + CultureInfo.GetCultureInfo("Russian").LCID;
        }
    }
}