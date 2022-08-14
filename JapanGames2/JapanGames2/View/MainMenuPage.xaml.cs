using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JapanGames2.Localization;

namespace JapanGames2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();            
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
            Button button = (Button)sender;

            button.IsEnabled = false;            

            await Navigation.PushAsync(new SudokuPage(), true);

            button.IsEnabled = true;
        }

        private async void OnButtonClickedAbout(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            button.IsEnabled = false;

            await Navigation.PushAsync(new AboutPage(), true);

            button.IsEnabled = true;
        }
    }
}