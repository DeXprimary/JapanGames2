using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace JapanGames2
{
    public class MySudokuGridBuilder : MySudokuGrid
    {
        //Начальный шаблон для новой игры:
        private byte[,] templateGrid =
        {   { 4, 7, 9, 1, 3, 2, 6, 5, 8 },
            { 1, 6, 2, 5, 9, 8, 7, 4, 3 },
            { 5, 3, 8, 7, 6, 4, 2, 9, 1 },
            { 3, 4, 5, 8, 7, 1, 9, 6, 2 },
            { 7, 2, 6, 3, 4, 9, 8, 1, 5 },
            { 8, 9, 1, 2, 5, 6, 4, 3, 7 },
            { 9, 1, 3, 4, 2, 7, 5, 8, 6 },
            { 6, 8, 7, 9, 1, 5, 3, 2, 4 },
            { 2, 5, 4, 6, 8, 3, 1, 7, 9 }
        };

        //Начальное условие судоку:
        private byte[,] currentCondition =
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

        public byte[,] NewGameCondition
        {
            get => currentCondition;
        }

        int[] tableWorkedMethods = new int[5] { 0, 0, 0, 0, 0 };

        Random random = new Random();

        public event EventHandler Builded = delegate { };

        private MySudokuGridBuilder() : base(null)
        {
            MethodWorkedWithProgress += OnMethodHadWorked;
                        
            Console.WriteLine(Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId + " Stage 1 ");
        }
                
        public static async Task<MySudokuGridBuilder> Build(byte difficulty, CancellationToken cancelToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var mySudokuGridBuilder = new MySudokuGridBuilder();

            await Task.Run(() => mySudokuGridBuilder.BuildNewGame(difficulty, cancelToken), cancelToken);
            
            /*
            try
            {
                await Task.Run(() => mySudokuGridBuilder.BuildNewGame(difficulty, cancelToken), cancelToken);
            }
            catch (Exception ex)
            {              
                Console.WriteLine(ex.Message);
            }            
            */

            mySudokuGridBuilder.Builded.Invoke(mySudokuGridBuilder, null);

            stopwatch.Stop();

            Console.WriteLine(Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId + " Stage 2: " + stopwatch.ElapsedMilliseconds + "ms");

            return mySudokuGridBuilder;
        }
        

        /*
        private async void BuildNewGameAsync(byte targetDifficulty)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId + " Stage 1 ");

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            await Task.Run(() => BuildNewGame(targetDifficulty));

            //BuildNewGame(targetDifficulty);

            stopwatch.Stop();            

            Console.WriteLine(Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId + " Stage 2: " + stopwatch.ElapsedMilliseconds + "ms");

            Builded.Invoke(this, null);
        } 
        */

        public void BuildNewGame(byte targetDifficulty, CancellationToken cancelToken)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId + " Stage 3 ");

            CreateGridBasedTemplate();

            ShuffleStartGrid(2);

            int currentPos;                        

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    currentCondition[i, j] = templateGrid[i, j];

                    //Console.Write("{0} ", beginCondition[i, j]);
                }
                //Console.WriteLine();
            }

            //Подготовка поля для новой игры
            List<int> cellsDone = new List<int>();

            int iteration = 0;

            int currentDifficulty = 1;

            //throw new Exception("MY EXCEPTION");

            while (currentDifficulty != targetDifficulty)
            {                               
                if (cancelToken.IsCancellationRequested)
                {
                    Console.WriteLine("<My Task Cancelled>");

                    cancelToken.ThrowIfCancellationRequested();

                    //throw new OperationCanceledException(cancelToken);               

                    //return;
                }

                iteration++;

                if (iteration > 60000)
                {
                    Console.WriteLine("REFRESH GRID!!!");

                    iteration = 0;

                    currentDifficulty = 1;

                    CreateGridBasedTemplate();

                    ShuffleStartGrid(2);

                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            currentCondition[i, j] = templateGrid[i, j];

                            //Console.Write("{0} ", beginCondition[i, j]);
                        }
                        //Console.WriteLine();
                    }
                }

                if (currentDifficulty == 0)
                {
                    ChangeRandomLikeMember(currentCondition, true);
                }
                else if (currentDifficulty < targetDifficulty)
                {
                    ChangeRandomLikeMember(currentCondition, false);
                }
                else
                {
                    if (tableWorkedMethods[targetDifficulty] == 0)
                    {
                        ChangeRandomLikeMember(currentCondition, false);
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
                                if (currentCondition[currentPos / 9, currentPos % 9] == 0)
                                {
                                    currentCondition[currentPos / 9, currentPos % 9] = templateGrid[currentPos / 9, currentPos % 9];

                                    currentDifficulty = TrySolve(targetDifficulty);

                                    if (currentDifficulty == targetDifficulty) break;

                                    if (tableWorkedMethods[targetDifficulty] == 0) currentCondition[currentPos / 9, currentPos % 9] = 0;

                                    else isNeedAdd = false;
                                }

                                cellsDone.Add(currentPos);
                            }
                        }
                        while (cellsDone.Count < 81);

                        if (isNeedAdd) ChangeRandomLikeMember(currentCondition, true);

                        //Console.WriteLine("Done" + tableWorkedMethods[targetDifficulty]);
                    }
                }

                currentDifficulty = TrySolve(targetDifficulty);
            }

            Console.WriteLine("Iteration: " + iteration);

            //Чистка поля от ненужных чисел
            cellsDone.Clear();

            do
            {
                currentPos = random.Next(0, 81);

                if (!cellsDone.Contains(currentPos))
                {
                    if (currentCondition[currentPos / 9, currentPos % 9] != 0)
                    {
                        currentCondition[currentPos / 9, currentPos % 9] = 0;

                        //Console.WriteLine("Cleaned " + currentPos);

                        currentDifficulty = TrySolve(targetDifficulty);

                        if (currentDifficulty != targetDifficulty)

                            currentCondition[currentPos / 9, currentPos % 9] = templateGrid[currentPos / 9, currentPos % 9];
                    }

                    cellsDone.Add(currentPos);
                }
            }
            while (cellsDone.Count < 81);

            TrySolve(targetDifficulty);
        }

        private void OnMethodHadWorked(int methodID)
        {
            if (methodID == 3 || methodID == 4 || methodID == 5) tableWorkedMethods[3]++;

            else if (methodID == 6) tableWorkedMethods[4]++;

            else tableWorkedMethods[methodID]++;
        }

        private void ChangeRandomLikeMember(byte[,] grid, bool needAdd)
        {
            //Удаление (добавление) псевдо-рандомного числа из сетки

            bool isFounded = false;

            int pos = random.Next(0, 81);

            //Console.WriteLine("rand: {0}   Add: {1}", pos, needAdd);

            if (needAdd)
            {
                if (grid[pos / 9, pos % 9] == 0) grid[pos / 9, pos % 9] = templateGrid[pos / 9, pos % 9];

                else

                    while (!isFounded)
                    {
                        if (pos < 80) pos++; else pos = 0;

                        if (grid[pos / 9, pos % 9] == 0)
                        {
                            grid[pos / 9, pos % 9] = templateGrid[pos / 9, pos % 9];

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

        private void CreateGridBasedTemplate()
        {
            List<byte> digits = new List<byte> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            //Создаём случайный набор цифр
            for (byte k = 0; k < 9; k++)
            {
                do
                {
                    byte temp = (byte)random.Next(0, 10);

                    if (!digits.Contains(temp)) digits[k] = temp;
                }
                while (digits[k] == 0);
            }

            //Заменяем цифры из шаблона нашими случайными
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    templateGrid[i, j] = digits[templateGrid[i, j] - 1];
                }
            }
        }

        private void ShuffleStartGrid(int countIteration)
        {            
            //Тасуем большие строки
            for (int iteration = 0; iteration < countIteration; iteration++)
            {
                byte[,] tempGrid = (byte[,])templateGrid.Clone();

                int frst = random.Next(3);

                int scnd = frst;

                while (scnd == frst) scnd = random.Next(3);

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        templateGrid[frst * 3 + i, j] = tempGrid[scnd * 3 + i, j];

                        templateGrid[scnd * 3 + i, j] = tempGrid[frst * 3 + i, j];
                    }
                }
            }

            //Тасуем большие столбы
            for (int iteration = 0; iteration < countIteration; iteration++)
            {
                byte[,] tempGrid = (byte[,])templateGrid.Clone();

                int frst = random.Next(3);

                int scnd = frst;

                while (scnd == frst) scnd = random.Next(3);

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        templateGrid[i, frst * 3 + j] = tempGrid[i, scnd * 3 + j];

                        templateGrid[i, scnd * 3 + j] = tempGrid[i, frst * 3 + j];
                    }
                }
            }

            //Тасуем малые строки
            for (int iteration = 0; iteration < countIteration; iteration++)
            {
                byte[,] tempGrid = (byte[,])templateGrid.Clone();

                for (int bigRow = 0; bigRow < 3; bigRow++)
                {
                    int frst = random.Next(3);

                    int scnd = frst;

                    while (scnd == frst) scnd = random.Next(3);

                    for (int j = 0; j < 9; j++)
                    {
                        templateGrid[bigRow * 3 + frst, j] = tempGrid[bigRow * 3 + scnd, j];

                        templateGrid[bigRow * 3 + scnd, j] = tempGrid[bigRow * 3 + frst, j];
                    }
                }
            }

            //Тасуем малые строки
            for (int iteration = 0; iteration < countIteration; iteration++)
            {
                byte[,] tempGrid = (byte[,])templateGrid.Clone();

                for (int bigCol = 0; bigCol < 3; bigCol++)
                {
                    int frst = random.Next(3);

                    int scnd = frst;

                    while (scnd == frst) scnd = random.Next(3);

                    for (int i = 0; i < 9; i++)
                    {
                        templateGrid[i, bigCol * 3 + frst] = tempGrid[i, bigCol * 3 + scnd];

                        templateGrid[i, bigCol * 3 + scnd] = tempGrid[i, bigCol * 3 + frst];
                    }
                }
            }


        }

        public override byte TrySolve(byte tryDifficulty = 4)
        {
            for (int i = 0; i < tableWorkedMethods.Length; i++)
            {
                tableWorkedMethods[i] = 0;
            }

            SetNewGrid(currentCondition);

            return base.TrySolve(tryDifficulty);
        }
    }
}
