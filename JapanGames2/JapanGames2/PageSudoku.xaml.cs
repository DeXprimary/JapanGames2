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
        private struct NumActionInfo
        {
            public byte i;
            public byte j;
            public byte value;

            public NumActionInfo(byte i, byte j, byte value)
            {
                this.i = i;
                this.j = j;
                this.value = value;
            }
        }

        private class Label_Coords : Label
        {
            public int i;
            public int j;

            public Label_Coords(int i, int j) : base()
            {
                this.i = i;
                this.j = j;
            }
        }

        public enum Difficulty
        {
            NoSolve = 0,
            VeryEasy = 1,
            Easy = 2,
            Normal = 3,
            Hard = 4,
            VeryHard = 5,
            Insane = 6
        }

        byte[,] newGameTemplate =
        {   { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
            { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
            { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
            { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
            { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
            { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
            { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
        };
        /*
        byte[,] beginCondition =
        {   { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        */
        byte[,] beginCondition =
        {   { 0, 7, 0, 0, 3, 0, 0, 5, 0 },
            { 0, 0, 0, 0, 0, 0, 7, 0, 0 },
            { 0, 3, 0, 0, 0, 4, 0, 0, 1 },
            { 0, 4, 0, 0, 0, 1, 9, 0, 2 },
            { 0, 0, 6, 0, 4, 0, 0, 0, 5 },
            { 8, 0, 0, 0, 5, 6, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 6 },
            { 0, 0, 0, 0, 0, 5, 0, 0, 4 },
            { 2, 0, 0, 0, 8, 0, 0, 0, 0 }
        }; 
        
        int[] tableWorkedMethods = new int[5] { 0, 0, 0, 0, 0 };

        Random random = new Random();

        Label_Coords[,] mainField = new Label_Coords[9, 9];

        Button[] numpadField = new Button[9];

        Grid[,] gridChild = new Grid[3, 3];

        Label_Coords selectedCell;

        bool isSelectedCell = false;

        byte? selectedNumber = null;

        MySudokuGrid mySudokuGrid;

        TapGestureRecognizer gestureTapMainGrid = new TapGestureRecognizer();

        TapGestureRecognizer gestureTapNumPad = new TapGestureRecognizer();



        void OnMethodHadWorked(int methodID)
        {
            if (methodID == 3 || methodID == 4 || methodID == 5) tableWorkedMethods[3]++;

            else if (methodID == 6) tableWorkedMethods[4]++; 

            else tableWorkedMethods[methodID]++;
        }

        void ResetWorkedMetods()
        {
            for (int i = 0; i < tableWorkedMethods.Length; i++)
            {
                tableWorkedMethods[i] = 0;
            }
        }

        async void OnClickedButtonSolveIt(object sender, EventArgs args)
        {
            if (await DisplayAlert("SOLVE IT", "Really solve it?", "Solve now!", "Cancel"))
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        mainField[i, j].Text = mySudokuGrid[i, j].SolveResult.GetValueOrDefault(0).ToString();
                    }
                }
            }
        }

        void OnClickedButtonHint(object sender, EventArgs args)
        {
            if (isSelectedCell)
            {
                mainField[selectedCell.i, selectedCell.j].Text = mySudokuGrid[selectedCell.i, selectedCell.j].SolveResult.GetValueOrDefault(0).ToString();
                mainField[selectedCell.i, selectedCell.j].BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                buttonErase.IsEnabled = false;
                isSelectedCell = false;
            }
            else
            {
                int counter = 0;

                foreach (var cell in mainField)
                {
                    if (cell.Text == "") counter++;
                }

                if (counter > 0)
                {
                    bool needBreak = false;
                    

                    while (!needBreak)
                    {
                        int x = random.Next(0, 81);

                        if (mainField[x / 9, x % 9].Text == "")
                        {
                            mainField[x / 9, x % 9].Text = mySudokuGrid[x / 9, x % 9].SolveResult.GetValueOrDefault(0).ToString();
                            needBreak = true;
                        }
                    }
                }
            }
        }

        void OnClickedButtonErase(object sender, EventArgs args)
        {
            Button but = sender as Button;

            if (isSelectedCell)
            {
                if (beginCondition[selectedCell.i, selectedCell.j] == 0)
                {
                    mainField[selectedCell.i, selectedCell.j].Text = "";
                }
                mainField[selectedCell.i, selectedCell.j].BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                buttonErase.IsEnabled = false;
                isSelectedCell = false;
            }
        }

        async void OnClickedButtonClear(object sender, EventArgs args)
        {
            if (await DisplayAlert("CLEAR", "Really clear field?", "Clear now!", "Cancel"))
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (beginCondition[i, j] != 0)
                            mainField[i, j].Text = beginCondition[i, j].ToString();
                        else
                            mainField[i, j].Text = "";
                    }
                }
            }
        }

        void OnTappedMainGrid(object sender, EventArgs args)
        {
            Label_Coords cell = sender as Label_Coords;

            if (selectedNumber.HasValue)
            {
                cell.Text = selectedNumber.ToString();
            }
            else
            {
                if (cell.BackgroundColor == (Color)Application.Current.Resources["Color_BGMarker"])
                {
                    cell.BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                    buttonErase.IsEnabled = false;
                    isSelectedCell = false;
                }
                else
                {
                    if (selectedCell != null)
                    {
                        mainField[selectedCell.i, selectedCell.j].BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                    }
                    cell.BackgroundColor = (Color)Application.Current.Resources["Color_BGMarker"];
                    buttonErase.IsEnabled = true;
                    isSelectedCell = true;
                    selectedCell = cell;
                }
            }
        }

        void OnClickedNumPad(object sender, EventArgs args)
        {
            Button but = sender as Button;

            if (isSelectedCell)
            {
                selectedCell.Text = but.Text;
                selectedCell.BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                isSelectedCell = false;
            }
            else
            {
                if (but.BackgroundColor == (Color)Application.Current.Resources["Color_ActiveButton"])
                {
                    but.BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                    selectedNumber = null;
                }
                else
                {
                    foreach (var button in numpadField)
                    {
                        if (button.BackgroundColor != (Color)Application.Current.Resources["Color_BGFiller"])
                            button.BackgroundColor = (Color)Application.Current.Resources["Color_BGFiller"];
                    }
                    but.BackgroundColor = (Color)Application.Current.Resources["Color_ActiveButton"];
                    selectedNumber = byte.Parse(but.Text);
                }
            }
        }

        void OnClickedButtonNewGame(object sender, EventArgs args)
        {

        }        

        int NewTrySolve()
        {
            ResetWorkedMetods();

            //mySudokuGrid = new MySudokuGrid(beginCondition);

            //mySudokuGrid.MethodWorkedWithProgress += OnMethodHadWorked;

            mySudokuGrid.SetNewGrid(beginCondition);

            int diff = mySudokuGrid.TrySolve();
            /*
            foreach (var k in tableWorkedMethods) Console.Write(k.ToString() + " | ");

            Console.WriteLine(" = " + diff);

            debugLabel.Text = "!" + diff + "!" + "/n";

            debugLabel2.Text = tableWorkedMethods[1] + "|" + tableWorkedMethods[2] + "|" +
                               tableWorkedMethods[3] + "|" + tableWorkedMethods[4];

            debugLabel3.Text = "Fault?:" + mySudokuGrid.CheckGridForFault();
            */
            return diff;
        }

        public PageSudoku(int targetDifficulty) : this()
        {
            MySudokuGridBuilder startField = new MySudokuGridBuilder();

            int currentPos;

            List<int> cellsDone = new List<int>();
            
            //Вывод в консоль начального поля
            Console.WriteLine("startField");
                        
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    beginCondition[i, j] = startField[i, j].SolveResult.GetValueOrDefault(0);

                    //Console.Write("{0} ", beginCondition[i, j]);
                }
                //Console.WriteLine();
            }
            
            //Подготовка поля для новой игры

            int currentDifficulty = 1;
            
            while (currentDifficulty != targetDifficulty)
            {
                if (currentDifficulty == 0)
                {
                    ChangeRandomLikeMember(beginCondition, true, out int position);
                }
                else if (currentDifficulty < targetDifficulty)
                {
                    ChangeRandomLikeMember(beginCondition, false, out int position);
                }
                else
                {
                    if (tableWorkedMethods[targetDifficulty] == 0)
                    {
                        ChangeRandomLikeMember(beginCondition, false, out int position);
                    }
                    else
                    {
                        //Console.WriteLine("Worked " + tableWorkedMethods[targetDifficulty]);

                        cellsDone.Clear();

                        bool isNeedAdd = true;

                        do
                        {
                            currentPos = random.Next(0, 81);

                            if (!cellsDone.Contains(currentPos))
                            {
                                if (beginCondition[currentPos / 9, currentPos % 9] == 0)
                                {
                                    beginCondition[currentPos / 9, currentPos % 9] = startField[currentPos / 9, currentPos % 9].SolveResult.GetValueOrDefault(0);

                                    currentDifficulty = NewTrySolve();

                                    if (currentDifficulty == targetDifficulty) break;

                                    if (tableWorkedMethods[targetDifficulty] == 0) beginCondition[currentPos / 9, currentPos % 9] = 0;

                                    else isNeedAdd = false;
                                }

                                cellsDone.Add(currentPos);
                            }
                        }
                        while (cellsDone.Count < 81);

                        if (isNeedAdd) ChangeRandomLikeMember(beginCondition, true, out int position);

                        //Console.WriteLine("Done" + tableWorkedMethods[targetDifficulty]);
                    }
                }

                currentDifficulty = NewTrySolve();
            }

            //Чистка поля от ненужных чисел
            cellsDone.Clear();     

            do
            {
                currentPos = random.Next(0, 81);

                if (!cellsDone.Contains(currentPos))
                {
                    if (beginCondition[currentPos / 9, currentPos % 9] != 0)
                    {
                        beginCondition[currentPos / 9, currentPos % 9] = 0;

                        //Console.WriteLine("Cleaned " + currentPos);

                        currentDifficulty = NewTrySolve();

                        if (currentDifficulty != targetDifficulty)

                            beginCondition[currentPos / 9, currentPos % 9] = startField[currentPos / 9, currentPos % 9].SolveResult.GetValueOrDefault(0);
                    }

                    cellsDone.Add(currentPos);
                }
            }
            while (cellsDone.Count < 81);
            
            NewTrySolve();

            //Вывод сетки на экран
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (beginCondition[i, j] != 0) mainField[i, j].Text = beginCondition[i, j].ToString();
                    else mainField[i, j].Text = "";
                }
            }                        

            //Удаление (добавление) рандомного числа из сетки
            void ChangeRandomLikeMember(byte[,] grid, bool needAdd, out int pos)
            {
                bool isFounded = false;

                pos = random.Next(0, 81);

                //Console.WriteLine("rand: {0}   Add: {1}", pos, needAdd);

                if (needAdd)
                {
                    if (grid[pos / 9, pos % 9] == 0) grid[pos / 9, pos % 9] = startField[pos / 9, pos % 9].SolveResult.GetValueOrDefault(0);

                    else

                        while (!isFounded)
                        {
                            if (pos < 80) pos++; else pos = 0;

                            if (grid[pos / 9, pos % 9] == 0)
                            {
                                grid[pos / 9, pos % 9] = startField[pos / 9, pos % 9].SolveResult.GetValueOrDefault(0);
                                
                                isFounded = true;
                            }
                        }
                }
                else
                {
                    if (grid[pos / 9, pos % 9] != 0) grid[pos / 9, pos % 9] = 0;

                    else

                        while (!isFounded)
                        {
                            if (pos < 80) pos++; else pos = 0;

                            if (grid[pos / 9, pos % 9] != 0)
                            {
                                grid[pos / 9, pos % 9] = 0;

                                isFounded = true;
                            }
                        }
                }                
            }




            /*
            mySudokuGrid = new MySudokuGrid(beginCondition);

            debugLabel.Text += "!" + mySudokuGrid.TrySolve().ToString() + "!";

            debugLabel.Text += mySudokuGrid.IsSolved.ToString();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (beginCondition[i, j] != 0)
                    {
                        mainField[i, j].Text = beginCondition[i, j].ToString();
                    }
                    else
                    {
                        mainField[i, j].Text = "";
                        mainField[i, j].TextColor = (Color)Application.Current.Resources["Color_ValidText"];
                    } 
                }
            }
            */
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
                        HasShadow = false,
                    };
                    
                    gridMain.Children.Add(frameGridChildFirst, j, i);
                    
                    Frame frameGridChildSecond = new Frame 
                    {
                        Padding = 0,
                        BackgroundColor = (Color)Application.Current.Resources["Color_BGMarker"],
                        CornerRadius = 0,
                        HasShadow = false,
                    };

                    frameGridChildFirst.Content = frameGridChildSecond;
                    
                    gridChild[i, j] = new Grid 
                    {
                        ColumnSpacing = 1,
                        RowSpacing = 1,
                        Padding = 0,
                        Margin = 0
                    };

                    frameGridChildSecond.Content = gridChild[i, j];

                    gridChild[i, j].ColumnDefinitions.Add(new ColumnDefinition() { Width = 40 });
                    gridChild[i, j].ColumnDefinitions.Add(new ColumnDefinition() { Width = 40 });
                    gridChild[i, j].ColumnDefinitions.Add(new ColumnDefinition() { Width = 40 });
                    gridChild[i, j].RowDefinitions.Add(new RowDefinition() { Height = 40,  });
                    gridChild[i, j].RowDefinitions.Add(new RowDefinition() { Height = 40 });
                    gridChild[i, j].RowDefinitions.Add(new RowDefinition() { Height = 40 });

                    for (int n = 0; n < 3; n++)
                    {
                        for (int m = 0; m < 3; m++)
                        {
                            Label_Coords label = new Label_Coords(i * 3 + n, j * 3 + m)
                            {
                                Style = (Style)Application.Current.Resources["PageSudoku_MainGrid"],
                                HorizontalOptions = LayoutOptions.Fill,
                                VerticalOptions = LayoutOptions.Fill,
                                Padding = 0,
                                Margin = 0
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

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (beginCondition[i, j] != 0) mainField[i, j].Text = beginCondition[i, j].ToString();
                    else mainField[i, j].Text = "";
                }
            }

            mySudokuGrid = new MySudokuGrid(beginCondition);

            mySudokuGrid.MethodWorkedWithProgress += OnMethodHadWorked;
            /*
            debugLabel.Text += "(" + mySudokuGrid.TrySolve().ToString() + ")";
            
            debugLabel2.Text = tableWorkedMethods[1] + "|" + tableWorkedMethods[2] + "|" +
                               tableWorkedMethods[3] + "|" + tableWorkedMethods[4];

            debugLabel3.Text = "Fault?:" + mySudokuGrid.CheckGridForFault();
            */
        }
    }
}
