using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JapanGames2
{
    public partial class PageSudoku : ContentPage
    {
        byte[,] TestExample =  
        {   { 0, 0, 0, 8, 0, 0, 0, 0, 5 },
            { 4, 0, 0, 0, 3, 5, 0, 2, 0 },
            { 0, 3, 0, 7, 0, 0, 0, 0, 0 },
            { 0, 0, 6, 0, 0, 0, 0, 4, 0 },
            { 0, 8, 0, 0, 9, 1, 0, 0, 2 },
            { 0, 0, 0, 5, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 7, 0, 0, 0, 0 },
            { 0, 9, 0, 0, 2, 3, 0, 0, 1 },
            { 8, 0, 0, 0, 0, 0, 9, 0, 0 } 
        };

        Label[,] mainField = new Label[9, 9];

        Button[] numpadField = new Button[9];

        Grid[,] gridChild = new Grid[3, 3];


        //===---
        Label[,] gridCandidates = new Label[27, 27];

        BoxView[,] boxes = new BoxView[9, 9];
        byte[,] gridCells = new byte[9, 9];
        BoxView[] boxesBottomLine = new BoxView[9];
        Label[] labelsBottomLine = new Label[9];
        MySudokuGrid mySudokuGrid;
        byte? SelectedNumber = null;
        //===---


        TapGestureRecognizer gestureTapMainGrid = new TapGestureRecognizer();
        TapGestureRecognizer gestureTapNumPad = new TapGestureRecognizer();

        void OnClickedButtonSolveIt(object sender, EventArgs args)
        {
                        
        }

        void OnClickedButtonHint(object sender, EventArgs args)
        {

        }

        void OnClickedButtonClear(object sender, EventArgs args)
        {

        }

        void OnTappedMainGrid(object sender, EventArgs args)
        {
            debugLabel.Text = "+++";

            /*
            MySudokuCell label = sender as MySudokuCell;
            if (Number != 0)
            {
                label.Text = Number.ToString();
                label.SolveResult = Number;
            }
            else
            {
                label.Text = "";
                label.SolveResult = null;
            }
            
            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                    for (byte k = 0; k < 9; k++)
                    {
                        gridCandidates[i * 3 + k % 3, j * 3 + k / 3].Text = mySudokuGrid.mySudokuCells[i, j].GetAvailableCandidate(k).ToString();
                    }
            */
            //Label1.Text = sender.GetType().ToString();
            //boxes[0,0].BackgroundColor = Color.Red;
        }

        void OnClickedNumPad(object sender, EventArgs args)
        {
            Button but = sender as Button;

            if (but.BackgroundColor == (Color)Application.Current.Resources["Color_BGMarker"])
            {
                but.BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                SelectedNumber = null;
            }
            else
            {
                foreach (var button in numpadField)
                {
                    button.BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                }
                but.BackgroundColor = (Color)Application.Current.Resources["Color_BGMarker"];
                SelectedNumber = byte.Parse(but.Text);
            }
        }

        void OnClickedButtonNewGame(object sender, EventArgs args)
        {

        }

        public PageSudoku()
        {
            InitializeComponent();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    Frame frameGridChildFirst = new Frame
                    {
                        Padding = 1,
                        BackgroundColor = (Color)Application.Current.Resources["Color_MenuText"],
                        CornerRadius = 0,
                        HasShadow = false
                    };
                    
                    gridMain.Children.Add(frameGridChildFirst, j, i);
                    
                    Frame frameGridChildSecond = new Frame 
                    {
                        Padding = 0,
                        BackgroundColor = (Color)Application.Current.Resources["Color_BGMarker"],
                        CornerRadius = 0,
                        HasShadow = false
                    };

                    frameGridChildFirst.Content = frameGridChildSecond;
                    
                    gridChild[i, j] = new Grid 
                    {
                        ColumnSpacing = 1,
                        RowSpacing = 1,
                        Padding = 0
                    };

                    frameGridChildSecond.Content = gridChild[i, j];

                    gridChild[i, j].ColumnDefinitions.Add(new ColumnDefinition() { Width = 40 });
                    gridChild[i, j].ColumnDefinitions.Add(new ColumnDefinition() { Width = 40 });
                    gridChild[i, j].ColumnDefinitions.Add(new ColumnDefinition() { Width = 40 });
                    gridChild[i, j].RowDefinitions.Add(new RowDefinition() { Height = 40 });
                    gridChild[i, j].RowDefinitions.Add(new RowDefinition() { Height = 40 });
                    gridChild[i, j].RowDefinitions.Add(new RowDefinition() { Height = 40 });                                   
                    
                    for (int n = 0; n < 3; n++)
                    {
                        for (int m = 0; m < 3; m++)
                        {
                            Label label = new Label
                            {
                                Style = (Style)Application.Current.Resources["PageSudoku_MainGrid"],
                                Text = "0"
                            };

                            label.GestureRecognizers.Add(gestureTapMainGrid);

                            gridChild[i, j].Children.Add(label, m, n);

                            mainField[i * 3 + n, j * 3 + m] = label;
                        }
                    }
                }
            }                      

            for (int i = 0; i < 9; i++)
            {
                /*
                Label label = new Label
                {
                    Text = (i + 1).ToString(),
                    //FontSize = 28,
                    //TextColor = Color.Black,
                    //BackgroundColor = Color.FromHex("#eff5ff"),
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    StyleClass = styleClassGridNumber
                };

                Frame frame = new Frame
                {
                    CornerRadius = 5,
                    StyleClass = styleClassGridNumber
                };
                */

                Button button = new Button
                {
                    Style = (Style)Application.Current.Resources["PageSudoku_NumPad"],
                    Text = (i + 1).ToString()
                };

                gridNumPad.Children.Add(button, i, 0);

                button.Clicked += OnClickedNumPad;

                button.GestureRecognizers.Add(gestureTapNumPad);

                numpadField[i] = button;
            }

            gestureTapMainGrid.Tapped += OnTappedMainGrid;
        }
    }
}
