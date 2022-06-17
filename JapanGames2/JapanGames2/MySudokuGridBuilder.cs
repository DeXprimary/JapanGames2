using System;
using System.Collections.Generic;
using System.Text;

namespace JapanGames2
{
    public class MySudokuGridBuilder : MySudokuGrid
    {
        //Начальный шаблон для любого судоку:
        private byte[,] startGrid =
        {   { 5, 3, 8, 6, 2, 4, 1, 7, 9 },
            { 6, 2, 4, 1, 7, 9, 5, 3, 8 },
            { 1, 7, 9, 5, 3, 8, 6, 2, 4 },
            { 3, 8, 6, 2, 4, 1, 7, 9, 5 },
            { 2, 4, 1, 7, 9, 5, 3, 8, 6 },
            { 7, 9, 5, 3, 8, 6, 2, 4, 1 },
            { 8, 6, 2, 4, 1, 7, 9, 5, 3 },
            { 4, 1, 7, 9, 5, 3, 8, 6, 2 },
            { 9, 5, 3, 8, 6, 2, 4, 1, 7 }
        };
        /*
        private byte[,] startGrid =
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
        */
        private static byte[,] emptyGrid =
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

        Random random = new Random();

        public MySudokuGridBuilder() : base(emptyGrid)
        {
            int CounterIterationLargeLine = 3;

            int CounterIterationSmallLine = 3;
            
            //Тасуем большие строки
            for (int iteration = 0; iteration < CounterIterationLargeLine; iteration++)
            {
                byte[,] tempGrid = (byte[,])startGrid.Clone();

                int frst = random.Next(3);

                int scnd = frst;

                while (scnd == frst) scnd = random.Next(3);

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        startGrid[frst * 3 + i, j] = tempGrid[scnd * 3 + i, j];

                        startGrid[scnd * 3 + i, j] = tempGrid[frst * 3 + i, j];
                    }
                }
            }
            
            //Тасуем большие столбы
            for (int iteration = 0; iteration < CounterIterationLargeLine; iteration++)
            {
                byte[,] tempGrid = (byte[,])startGrid.Clone();

                int frst = random.Next(3);

                int scnd = frst;

                while (scnd == frst) scnd = random.Next(3);

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        startGrid[i, frst * 3 + j] = tempGrid[i, scnd * 3 + j];

                        startGrid[i, scnd * 3 + j] = tempGrid[i, frst * 3 + j];
                    }
                }
            }
            
            //Тасуем малые строки
            for (int iteration = 0; iteration < CounterIterationSmallLine; iteration++)
            {
                byte[,] tempGrid = (byte[,])startGrid.Clone();

                for (int bigRow = 0; bigRow < 3; bigRow++)
                {
                    int frst = random.Next(3);

                    int scnd = frst;

                    while (scnd == frst) scnd = random.Next(3);

                    for (int j = 0; j < 9; j++)
                    {
                        startGrid[bigRow * 3 + frst, j] = tempGrid[bigRow * 3 + scnd, j];

                        startGrid[bigRow * 3 + scnd, j] = tempGrid[bigRow * 3 + frst, j];
                    }
                }
            }

            //Тасуем малые строки
            for (int iteration = 0; iteration < CounterIterationSmallLine; iteration++)
            {
                byte[,] tempGrid = (byte[,])startGrid.Clone();

                for (int bigCol = 0; bigCol < 3; bigCol++)
                {
                    int frst = random.Next(3);

                    int scnd = frst;

                    while (scnd == frst) scnd = random.Next(3);

                    for (int i = 0; i < 9; i++)
                    {
                        startGrid[i, bigCol * 3 + frst] = tempGrid[i, bigCol * 3 + scnd];

                        startGrid[i, bigCol * 3 + scnd] = tempGrid[i, bigCol * 3 + frst];
                    }
                }
            }

            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    this[i, j].SolveResult = startGrid[i, j];
                }
            }
            



            /*
            byte RandomValidNumber(MySudokuCell cell)
            {
                byte num = (byte)random.Next(9);

                bool isValidNumber = false;

                while (!isValidNumber)
                {                    
                    if (cell.GetAvailableCandidate(num) != 0)
                    {
                        isValidNumber = true;
                    }
                    else
                    {
                        if (num >= 8) num = 0;

                        else num++;
                    }
                }
                return num;
            }

            //bool isNeedBreak = false;

            Console.WriteLine("gridBuilder:");

            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    if (!this[i, j].SolveResult.HasValue)
                    {
                        this.SetValueCell((byte)(RandomValidNumber(this[i, j]) + 1), i, j);

                        this.TryCleanInvalidCandidate();                                               

                        //if (isNeedBreak) break;
                    }
                    Console.Write("{0} ", this[i, j].SolveResult.GetValueOrDefault(0));
                }
                Console.WriteLine();

                //if (isNeedBreak) break;
            }
            */
        }
    }
}
