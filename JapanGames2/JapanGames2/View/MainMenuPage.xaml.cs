using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JapanGames2.Localization;
using System.Globalization;
using Xamarin.Essentials;

namespace JapanGames2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();

            langPicker.Title = CultureInfo.CurrentUICulture.ToString();

            //ResourceLang.ResourceManager.

            //CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en", "EN");
            
            //CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en", "EN");

            langPicker.ItemsSource = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

            var temp = CultureInfo.GetCultures(CultureTypes.NeutralCultures);

            var index = 7;

            welcomeLabel.Text =
                DeviceInfo.DeviceType + Environment.NewLine +
                DeviceInfo.Idiom + Environment.NewLine +
                DeviceInfo.Manufacturer + Environment.NewLine +
                DeviceInfo.Model + Environment.NewLine +
                DeviceInfo.Name + Environment.NewLine +
                DeviceInfo.Platform + Environment.NewLine +
                DeviceInfo.Version + Environment.NewLine +
                DeviceInfo.VersionString + Environment.NewLine;
        }

        private async void OnButtonClickedNewGameSudoku(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            button.IsEnabled = false;

            int difficulty = 0;

            string choiceResult = await DisplayActionSheet(
            ResourceLang.ActionSheet_Difficulty, ResourceLang.Common_Cancel, null,
            ResourceLang.ActionSheet_Easy, ResourceLang.ActionSheet_Normal,
            ResourceLang.ActionSheet_Hard, ResourceLang.ActionSheet_Insane);

            if (choiceResult == ResourceLang.ActionSheet_Easy) difficulty = 1;

            else if (choiceResult == ResourceLang.ActionSheet_Normal) difficulty = 2;

            else if (choiceResult == ResourceLang.ActionSheet_Hard) difficulty = 3;

            else if (choiceResult == ResourceLang.ActionSheet_Insane) difficulty = 4;

            if (difficulty != 0) await Navigation.PushAsync(new SudokuPage(), true);

            button.IsEnabled = true;
        }

        private async void OnButtonClickedEmptySudoku(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

            await Navigation.PushAsync(new MainMenuPage());

            /*
            Button button = (Button)sender;

            button.IsEnabled = false;            

            await Navigation.PushAsync(new SudokuPage(), true);

            button.IsEnabled = true;
            */
        }

        private async void OnButtonClickedAbout(object sender, EventArgs e)
        {
            if (Localization.ResourceLang.Culture != CultureInfo.GetCultureInfo("en"))

                Localization.ResourceLang.Culture = CultureInfo.GetCultureInfo("en");

            else Localization.ResourceLang.Culture = CultureInfo.GetCultureInfo("ru");

            

            /*
            Button button = (Button)sender;

            button.IsEnabled = false;

            await Navigation.PushAsync(new AboutPage(), true);

            button.IsEnabled = true;
            */
        }
    }
}