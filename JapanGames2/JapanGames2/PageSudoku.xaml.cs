using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
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

        Random random = new Random();

        Label_Coords[,] mainField = new Label_Coords[9, 9];

        Button[] numpadField = new Button[9];

        Grid[,] gridChild = new Grid[3, 3];

        Label_Coords selectedCell;

        bool isSelectedCell = false;

        byte? selectedNumber = null;

        MySudokuGrid mySudokuGrid;

        CancellationTokenSource cancelTokenSourse;

        TapGestureRecognizer gestureTapMainGrid = new TapGestureRecognizer();

        TapGestureRecognizer gestureTapNumPad = new TapGestureRecognizer();        

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
            if (cancelTokenSourse != null) 

                cancelTokenSourse.Cancel();
        }                    

        void OnBuilded(object sender, EventArgs args)
        {
            if (cancelTokenSourse != null)

                cancelTokenSourse.Cancel();

            var grid = sender as MySudokuGridBuilder;

            //Вывод сетки на экран
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid.NewGameCondition[i, j] != 0)
                        mainField[i, j].Text = grid.NewGameCondition[i, j].ToString();
                    else 
                    mainField[i, j].Text = "";
                }
            }
        }

        private async void CreateGameAsync(int diff)
        {
            cancelTokenSourse = new CancellationTokenSource();

            var cancelToken = cancelTokenSourse.Token;

            var tasks = new Task<MySudokuGridBuilder>[Environment.ProcessorCount * 1];

            Console.WriteLine(Environment.ProcessorCount);

            try
            {         
                for (int k = 0; k < tasks.Length; k++)
                {
                    tasks[k] = MySudokuGridBuilder.Build((byte)diff, cancelToken);
                }

                //int indexCompleteTask = Task.WaitAny(tasks);

                var fastestTask = await Task<MySudokuGridBuilder>.WhenAny(tasks);

                Console.WriteLine("Status: " + fastestTask.Status);

                foreach (var k in tasks) { Console.WriteLine("Task Info: " + k.Id + " | " + k.Status); }

                Console.WriteLine("Ex stage 1");

                OnBuilded(await fastestTask, null);

                Console.WriteLine("Ex stage 2");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Outer Handler Worked When Excepted" + ex.StackTrace);
            }/*
            catch (AggregateException aggEx)
            {
                Console.WriteLine("Ex stage 3");

                aggEx.Flatten().Handle(ex =>
                {
                    string message = ex is OperationCanceledException ? "Task is canceled" : ex.Message;
                    
                    Console.WriteLine("Ex message: " + message + " | " + ex.StackTrace);

                    return true;
                });

                Console.WriteLine("Ex stage 4");
            }*/
            /*
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Ex: OpCanceled - " + ex.Message);
                Console.WriteLine("Ex: OpCanceled - " + ex.StackTrace);
                Debug.WriteLine("Ex: OpCanceled - " + ex.Message);
                Debug.WriteLine("Ex: OpCanceled - " + ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex: Other - " + ex.Message);
            }
            */

            finally
            {
                //Console.WriteLine("Status: " + tasks[0].Status);

                cancelTokenSourse.Dispose();
            }

            //var temp = MySudokuGridBuilder.Build((byte)diff);

            //Console.WriteLine("Status: " + temp.Status);                        

            //MySudokuGridBuilder startField = await MySudokuGridBuilder.Build((byte)diff);

            //Console.WriteLine("Status: " + temp.Status);

            //OnBuilded(startField, null);

            //var startField = new MySudokuGridBuilder((byte)diff);

            //startField.Builded += OnBuilded;

            //await Task.Run(() => startField.BuildNewGame((byte)diff));

            //startField.BuildNewGame((byte)diff);

            //var startField = MySudokuGridBuilder.Build((byte)diff);

            //startField
        }

        public PageSudoku(int targetDifficulty) : this()
        {          
            CreateGameAsync(targetDifficulty);

            //MySudokuGridBuilder startField = new MySudokuGridBuilder((byte)targetDifficulty);

            //var startField = MySudokuGridBuilder.Build((byte)targetDifficulty);

            //startField.

            //startField.Builded += OnBuilded;

            //startField.BuildNewGame((byte)targetDifficulty);

                                     
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

            /*
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (beginCondition[i, j] != 0) mainField[i, j].Text = beginCondition[i, j].ToString();
                    else mainField[i, j].Text = "";
                }
            }

            mySudokuGrid = new MySudokuGrid(beginCondition);

            debugLabel.Text = mySudokuGrid.TrySolve().ToString();
            */

            /*
            debugLabel.Text += "(" + mySudokuGrid.TrySolve().ToString() + ")";
            
            debugLabel2.Text = tableWorkedMethods[1] + "|" + tableWorkedMethods[2] + "|" +
                               tableWorkedMethods[3] + "|" + tableWorkedMethods[4];

            debugLabel3.Text = "Fault?:" + mySudokuGrid.CheckGridForFault();
            */
        }
    }
}
