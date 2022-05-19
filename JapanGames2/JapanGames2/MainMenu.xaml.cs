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
    public partial class MainMenu : ContentPage
    {
        async void OnClickedButtonSudoku(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SudokuPage());
        }

        void OnClickedButtonJapCross(object sender, EventArgs args)
        {

        }

        void OnClickedButtonAbout(object sender, EventArgs args)
        {

        }

        public MainMenu()
        {
            InitializeComponent();
        }
    }
}