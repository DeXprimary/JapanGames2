using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            switch (await DisplayActionSheet("Difficulty", "Cancel", null, "Easy", "Normal", "Hard", "Insane"))
            {
                case "Easy":    difficulty = 1; break;

                case "Normal":  difficulty = 2; break;

                case "Hard":    difficulty = 3; break;

                case "Insane":  difficulty = 4; break;

                default:        difficulty = 0; break;
            }            

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


        }
    }
}