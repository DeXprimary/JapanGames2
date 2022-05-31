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
            await Navigation.PushAsync(new PageSudoku());
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