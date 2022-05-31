using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JapanGames2
{
    public partial class SudokuPageOld : ContentPage
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
            { 8, 0, 0, 0, 0, 0, 9, 0, 0 } };

        Label[,] gridCandidates = new Label[27, 27];

        BoxView[,] boxes = new BoxView[9, 9];
        MySudokuCell[,] gridCells = new MySudokuCell[9, 9];
        BoxView[] boxesBottomLine = new BoxView[9];
        Label[] labelsBottomLine = new Label[9];
        MySudokuGrid mySudokuGrid;
        byte Number = 0;

        TapGestureRecognizer gestureTapGridNumbers = new TapGestureRecognizer();
        TapGestureRecognizer gestureTapBottomLine = new TapGestureRecognizer();

        void OnClickedButtonSolveIt(object sender, EventArgs args)
        {
            bool hasProgress = true;

            while (hasProgress)
            {
                if (!mySudokuGrid.SolvingProcedure1(gridCells))
                    if (!mySudokuGrid.SolvingProcedure2(gridCells))
                        if (!mySudokuGrid.SolvingProcedure3(gridCells))
                            if (!mySudokuGrid.SolvingProcedure4(gridCells))
                                if (!mySudokuGrid.SolvingProcedure5(gridCells))
                                    if (!mySudokuGrid.SolvingProcedureSubstitution(gridCells))
                                    {
                                        hasProgress = false;
                                        foreach (MySudokuCell cell in mySudokuGrid.mySudokuCells)
                                        {
                                            if (cell.SolveResult.HasValue)
                                                cell.Text = cell.SolveResult.Value.ToString();
                                            else cell.Text = "";
                                        }
                                    }
            }
        }

        void OnClickedButtonHint(object sender, EventArgs args)
        {
            byte usedProc = 0;
            if (!mySudokuGrid.SolvingProcedure1(gridCells))
            {
                usedProc++;
                if (!mySudokuGrid.SolvingProcedure2(gridCells))
                {
                    usedProc++;
                    if (!mySudokuGrid.SolvingProcedure3(gridCells))
                    {
                        usedProc++;
                        if (!mySudokuGrid.SolvingProcedure4(gridCells))
                        {
                            usedProc++;
                            if (!mySudokuGrid.SolvingProcedure5(gridCells))
                            {
                                usedProc++;
                                if (!mySudokuGrid.SolvingProcedureSubstitution(gridCells))
                                {
                                    usedProc++;
                                    //mySudokuGrid.SolvingProcedureSubstitution(gridCells);
                                }
                            }
                        }
                    }
                }                    
            }
            usedProc++;

            foreach (MySudokuCell cell in mySudokuGrid.mySudokuCells)
            {
                if (cell.SolveResult.HasValue)
                    cell.Text = cell.SolveResult.Value.ToString();
                else cell.Text = "";
            }

            //Блок отладки
            Label1.Text = usedProc.ToString() + " "
                + mySudokuGrid.mySudokuCells[0, 0].CountAvailableCandidates.ToString()
                + mySudokuGrid.mySudokuCells[1, 0].CountAvailableCandidates.ToString()
                + mySudokuGrid.mySudokuCells[2, 0].CountAvailableCandidates.ToString()
                + mySudokuGrid.mySudokuCells[3, 0].CountAvailableCandidates.ToString()
                + mySudokuGrid.mySudokuCells[4, 0].CountAvailableCandidates.ToString()
                + mySudokuGrid.mySudokuCells[5, 0].CountAvailableCandidates.ToString();
            /*
            for (byte i = 0; i < 9; i++) 
                for (byte j = 0; j < 9; j++)
                    for (byte k = 0; k < 9; k++)
                    {
                        gridCandidates[i * 3 + k % 3, j * 3 + k / 3].Text = mySudokuGrid.mySudokuCells[i, j].GetAvailableCandidate(k).ToString();
                    }
            */
        }

        void OnClickedButtonClear(object sender, EventArgs args)
        {
            foreach (var cell in gridCells)
            {
                if (cell.Text != "") 
                { 
                    cell.Text = ""; cell.SolveResult = null; 
                }

            }
            /*
            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                    for (byte k = 0; k < 9; k++)
                    {
                        gridCandidates[i * 3 + k % 3, j * 3 + k / 3].Text = mySudokuGrid.mySudokuCells[i, j].GetAvailableCandidate(k).ToString();
                    }
            */
        }

        void OnTappedGridNumbers(object sender, EventArgs args)
        {
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
            /*
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

        public SudokuPageOld()
        {
            InitializeComponent();

            
            //gesture.Tapped += OnTapGestureTapped;

            //boxes[0, 0].GestureRecognizers.Add(gesture);

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
                    /*
                    for (int k = 0; k < 9; k++)
                    {
                        Label label = new Label();
                        label.FontSize = 8;
                        label.Text = "0";
                        gridCandidates[i * 3 + k % 3, j * 3 + k / 3] = label;
                        gridOfCandidate.Children.Add(label, j * 3 + k % 3, i * 3 + k / 3);
                    }
                    */
                }
                    

            mySudokuGrid = new MySudokuGrid(ref gridCells);

            



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
