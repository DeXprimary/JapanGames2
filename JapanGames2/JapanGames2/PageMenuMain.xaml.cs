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
            switch (await DisplayActionSheet("Difficulty", "Cancel", null, "Very easy", "Easy", "Normal", "Hard", "Very hard", "Other source"))
            {
                case "Very easy":
                    difficulty = 1;
                    break;

                case "Easy":
                    difficulty = 2;
                    break;

                case "Normal":
                    difficulty = 3;
                    break;

                case "Hard":
                    difficulty = 4;
                    break;

                case "Very hard":
                    difficulty = 5;
                    break;

                default:
                    difficulty = 0;
                    break;
            }
            await Navigation.PushAsync(new PageSudoku(difficulty));
        }

        void OnClickedButtonJapCross(object sender, EventArgs args)
        {
            
        }

        async void OnClickedButtonAbout(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PageAbout());
        }

        public PageMainMenu()
        {
            InitializeComponent();                       
        }
    }
}