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

        Label[,] gridCandidates = new Label[27, 27];

        BoxView[,] boxes = new BoxView[9, 9];
        byte[,] gridCells = new byte[9, 9];
        BoxView[] boxesBottomLine = new BoxView[9];
        Label[] labelsBottomLine = new Label[9];
        MySudokuGrid mySudokuGrid;
        byte Number = 0;

        TapGestureRecognizer gestureTapGridNumbers = new TapGestureRecognizer();
        TapGestureRecognizer gestureTapBottomLine = new TapGestureRecognizer();

        void OnClickedButtonSolveIt(object sender, EventArgs args)
        {
                        
        }

        void OnClickedButtonHint(object sender, EventArgs args)
        {

        }

        void OnClickedButtonClear(object sender, EventArgs args)
        {

        }

        void OnTappedGridNumbers(object sender, EventArgs args)
        {
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

        void OnTappedBottomLine(object sender, EventArgs args)
        {
            Label tmpLabel = sender as Label;
            Number = byte.Parse(tmpLabel.Text);
            if (boxesBottomLine[Number - 1].BackgroundColor == Color.White)
            {
                foreach (var box in boxesBottomLine)
                    if (box.BackgroundColor != Color.White) box.BackgroundColor = Color.White;
                boxesBottomLine[Number - 1].BackgroundColor = Color.Khaki;
            }
            else
            {
                boxesBottomLine[Number - 1].BackgroundColor = Color.White;
                Number = 0;
            }
        }

        public PageSudoku()
        {
            InitializeComponent();

            Label[,] mainField = new Label[9, 9];

            Label[] numpadField = new Label[9];

            Grid[,] gridChild = new Grid[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    Frame frameGridChildFirst = new Frame
                    {
                        Padding = 1,
                        BackgroundColor = Color.Black,
                        CornerRadius = 0,
                        HasShadow = false
                    };
                    
                    gridMain.Children.Add(frameGridChildFirst, j, i);
                    
                    Frame frameGridChildSecond = new Frame 
                    {
                        Padding = 0,
                        BackgroundColor = Color.Gray,
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
                                Text = "0",
                                FontSize = 28,
                                TextColor = Color.Black,
                                BackgroundColor = Color.FromHex("#eff5ff"),
                                VerticalTextAlignment = TextAlignment.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                            };

                            label.GestureRecognizers.Add(gestureTapGridNumbers);

                            gridChild[i, j].Children.Add(label, m, n);

                            mainField[i * 3 + n, j * 3 + m] = label;
                        }
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                Label label = new Label
                {
                    Text = (i + 1).ToString(),
                    FontSize = 28,
                    TextColor = Color.Black,
                    BackgroundColor = Color.FromHex("#eff5ff"),
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                gridNumPad.Children.Add(label, i, 0);         

            }



            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mainField[i, j].Text = (i + j).ToString();
                }
            }











                    /*

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            BoxView box = new BoxView();
                            box.BackgroundColor = Color.White;
                            box.CornerRadius = 3;
                            boxes[i, j] = box;
                            gridNumbers.Children.Add(box, j, i);

                            MySudokuCell cell = new MySudokuCell();
                            cell.Text = "";
                            cell.FontSize = 25;
                            cell.FontAttributes = FontAttributes.Bold;
                            cell.VerticalTextAlignment = TextAlignment.Center;
                            cell.HorizontalTextAlignment = TextAlignment.Center;
                            cell.GestureRecognizers.Add(gestureTapGridNumbers);
                            gridCells[i, j] = cell;
                            gridNumbers.Children.Add(cell, j, i);

                            //absoluteLayout.Children.Add(box, new Point(i*10, j*10));
                        }

                        BoxView boxBottomLine = new BoxView();
                        boxBottomLine.BackgroundColor = Color.White;
                        boxBottomLine.CornerRadius = 3;
                        boxesBottomLine[i] = boxBottomLine;
                        gridNumbersBottomRow.Children.Add(boxBottomLine, i, 0);

                        Label labelForPrint = new Label();
                        labelForPrint.Text = (i + 1).ToString();
                        labelForPrint.FontSize = 25;
                        labelForPrint.FontAttributes = FontAttributes.Bold;
                        labelForPrint.VerticalTextAlignment = TextAlignment.Center;
                        labelForPrint.HorizontalTextAlignment = TextAlignment.Center;
                        labelForPrint.GestureRecognizers.Add(gestureTapBottomLine);
                        labelsBottomLine[i] = labelForPrint;
                        gridNumbersBottomRow.Children.Add(labelForPrint, i, 0);
                    }

                    // Set grid for candidates
                    for (int i = 0; i < 27; i++)
                    {
                        gridOfCandidate.RowDefinitions.Add(new RowDefinition { Height = 10 });
                        gridOfCandidate.ColumnDefinitions.Add(new ColumnDefinition { Width = 10 });
                    }

                    // Load Test Example with grid for candidates
                    for (int i = 0; i < 9; i++) 
                        for (int j = 0; j < 9; j++)
                        {
                            if (TestExample[i, j] != 0)
                            {
                                gridCells[i, j].Text = TestExample[i, j].ToString();
                                gridCells[i, j].SolveResult = TestExample[i, j];
                            }
                        }


                    mySudokuGrid = new MySudokuGrid(ref gridCells);

                    */



                    //boxes[0, 0].BackgroundColor = Color.Green;
                    //myBox.GestureRecognizers.Add(gesture);
                    //GestureRecognizers.Add(gesture);
                    //gridNumbersBottomRow.GestureRecognizers.Add(gesture);
                    gestureTapGridNumbers.Tapped += OnTappedGridNumbers;
            gestureTapBottomLine.Tapped += OnTappedBottomLine;

            //Title title1 = new Title

        }
    }
}
