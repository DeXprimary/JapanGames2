using System;
using System.Collections.Generic;
using System.Text;

namespace JapanGames2
{
	public class MyCellOld

	{

		public byte[] availableVariantes = new byte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		public byte solve = 0;
		public bool isSolved = false;

		public MyCell(byte Number)
		{
			solve = Number;
			if (solve != 0)
			{
				isSolved = true;
				clearVariantes(Number);
			}
		}

		public void clearVariantes(byte num)
		{
			for (byte i = 0; i < 9; i++)
			{
				if (i != num - 1)
					availableVariantes[i] = 0;
			}
		}
		public void refreshStatusSolving()
		{
			byte counterVariantes = 0;
			byte lastVariante = 0;
			foreach (var number in availableVariantes)

				if (number != 0)
				{
					counterVariantes++;
					lastVariante = number;
				}

			if (counterVariantes == 1)
			{
				solve = lastVariante;
				isSolved = true;
			}
		}
	}
	public class TestOld
	{
		static byte[,] fieldNumbers = new byte[9, 9];
		static MyCell[,] myCell = new MyCell[9, 9];
		static int newCounterUnknown = 0, oldCounterUnknown = 0, iteration = 0;

		static public void Main()
		{
			//Console.WriteLine ("Sudoku grid:");
			//Первый вывод сетки
			for (byte i = 0; i < 9; i++)
			{
				string inputLine = Console.ReadLine();
				for (byte j = 0; j < 9; j++)
				{
					myCell[i, j] = new MyCell(byte.Parse(inputLine.Substring(j, 1)));
					//Console.Write("{0} ", myCell[i,j].solve);
					if (myCell[i, j].solve == 0) newCounterUnknown++;
				}
				//Console.WriteLine();
			}
			//Console.WriteLine("Unknown: {0}", oldCounterUnknown);
			//Console.WriteLine("-----------------");
			//Console.WriteLine();
			byte tryCounter = 0, tryNumber = 3;

			while (newCounterUnknown != oldCounterUnknown)
			{
				oldCounterUnknown = newCounterUnknown;
				newCounterUnknown = 0;
				Console.WriteLine("Iteration: {0}", iteration++);
				solvingProcedure1(ref myCell);

				for (byte i = 0; i < 9; i++)
				{
					for (byte j = 0; j < 9; j++)
					{
						Console.Write("{0} ", myCell[i, j].solve);
						for (byte k = 0; k < 9; k++)
							Console.Write("{0}", myCell[i, j].availableVariantes[k]);
						Console.Write(" ");
						if (myCell[i, j].solve == 0) newCounterUnknown++;
					}
					Console.WriteLine();
				}
				Console.WriteLine("Unknown: {0}", newCounterUnknown);
				Console.WriteLine();

				if (newCounterUnknown == oldCounterUnknown)
				{
					solvingProcedure2(ref myCell);

					if (tryCounter < tryNumber)
					{
						oldCounterUnknown = 0;
						tryCounter++;
					}
				}
				else tryCounter = 0;
			}

			void solvingProcedure3(ref MyCell[,] grid)
			{

			}

			void solvingProcedure2(ref MyCell[,] grid)

			{
				//Исключаем очевидно парных кандидатов квадратиков из строк и столбов
				for (byte i = 0; i < 3; i++)
				{
					for (byte j = 0; j < 3; j++)
					{
						//Console.WriteLine();
						for (byte num = 0; num < 9; num++)
						{
							byte countNumbers = 0;
							byte pos1 = 0, pos2 = 0;

							for (byte k = 0; k < 9; k++)
							{
								if (!grid[i * 3 + (k / 3), j * 3 + (k % 3)].isSolved
								&& grid[i * 3 + (k / 3), j * 3 + (k % 3)].availableVariantes[num] != 0)
								{
									countNumbers++;
									if (countNumbers == 1) { pos1 = k; }
									if (countNumbers == 2) { pos2 = k; }
								}
							}
							Console.WriteLine("{0} {1} {2} {3}", num + 1, countNumbers, pos1, pos2);

							if (countNumbers == 2)
							{
								if (pos1 % 3 == pos2 % 3)
								{
									for (byte x = 0; x < 9; x++)
										if (!grid[x, j * 3 + (pos1 % 3)].isSolved && x / 3 != i)
											grid[x, j * 3 + (pos1 % 3)].availableVariantes[num] = 0;
								}
								if (pos1 / 3 == pos2 / 3)
								{
									for (byte x = 0; x < 9; x++)
										if (!grid[i * 3 + (pos1 / 3), x].isSolved && x / 3 != j)
											grid[i * 3 + (pos1 / 3), x].availableVariantes[num] = 0;
								}
							}
						}
					}
				}
				//Исключаем очевидно парных кандидатов строк из квадратика

				for (byte i = 0; i < 9; i++)
				{
					for (byte num = 0; num < 9; num++)
					{
						byte countNumbers = 0;
						byte pos1 = 0, pos2 = 0;

						for (byte k = 0; k < 9; k++)
						{
							if (!grid[i, k].isSolved
								&& grid[i, k].availableVariantes[num] != 0)
							{
								countNumbers++;
								if (countNumbers == 1) { pos1 = k; }
								if (countNumbers == 2) { pos2 = k; }
							}
						}

						if (countNumbers == 2)
						{
							if (pos1 / 3 == pos2 / 3)
							{
								for (byte x = 0; x < 9; x++)
									if (!grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].isSolved
									&& i % 3 != x / 3)
										grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].availableVariantes[num] = 0;
							}
						}
					}
				}
				//Исключаем очевидно парных кандидатов столба из квадратика
				for (byte j = 0; j < 9; j++)
				{
					for (byte num = 0; num < 9; num++)
					{
						byte countNumbers = 0;
						byte pos1 = 0, pos2 = 0;

						for (byte k = 0; k < 9; k++)
						{
							if (!grid[k, j].isSolved
								&& grid[k, j].availableVariantes[num] != 0)
							{
								countNumbers++;
								if (countNumbers == 1) { pos1 = k; }
								if (countNumbers == 2) { pos2 = k; }
							}
						}

						if (countNumbers == 2)
						{
							if (pos1 / 3 == pos2 / 3)
							{
								for (byte x = 0; x < 9; x++)
									if (!grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].isSolved
									&& j % 3 != x % 3)
										grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].availableVariantes[num] = 0;
							}
						}
					}
				}
			}

			void solvingProcedure1(ref MyCell[,] grid)
			{

				for (byte i = 0; i < 9; i++)
				{
					for (byte j = 0; j < 9; j++)
					{

						//Убираем невозможных кандидатов с поля
						if (grid[i, j].isSolved)
						{
							//Проходим по строке
							for (byte k = 0; k < 9; k++)
							{
								if (k != j)
								{
									grid[i, k].availableVariantes[grid[i, j].solve - 1] = 0;
								}
							}

							//Проходим по столбцу
							for (byte k = 0; k < 9; k++)
							{
								if (k != i)
								{
									grid[k, j].availableVariantes[grid[i, j].solve - 1] = 0;
								}
							}
							//Проходим по квадратику
							for (byte k = 0; k < 9; k++)
							{
								if (i != (i / 3 * 3 + k / 3) || j != (j / 3 * 3 + k % 3))
								{
									grid[i / 3 * 3 + k / 3, j / 3 * 3 + k % 3].availableVariantes[grid[i, j].solve - 1] = 0;
									//Console.Write("{0}", grid[i/3*3 + k/3, j/3*3 + k%3].solve);
								}

							}
							//Console.WriteLine();
						}
						//Ставим числа в ячейках по методу исключения
						else
						{
							//Проходим по строке
							for (byte num = 0; num < 9; num++)
							{
								byte countNotAvailableVariantes = 0;

								for (byte k = 0; k < 9; k++)
								{
									if (k != j)
									{
										if (grid[i, k].availableVariantes[num] == 0) countNotAvailableVariantes++;

									}
								}
								if (countNotAvailableVariantes == 8)
								{
									grid[i, j].solve = (byte)(num + 1);
									grid[i, j].isSolved = true;
									grid[i, j].clearVariantes((byte)(num + 1));
									break;
								}
							}

							//Проходим по столбцу
							for (byte num = 0; num < 9; num++)
							{
								byte countNotAvailableVariantes = 0;

								for (byte k = 0; k < 9; k++)
								{
									if (k != i)
									{
										if (grid[k, j].availableVariantes[num] == 0) countNotAvailableVariantes++;
									}
								}
								if (countNotAvailableVariantes == 8)
								{
									grid[i, j].solve = (byte)(num + 1);
									grid[i, j].isSolved = true;
									grid[i, j].clearVariantes((byte)(num + 1));
									break;
								}
							}

							//Проходим по квадратику
							for (byte num = 0; num < 9; num++)
							{
								byte countNotAvailableVariantes = 0;
								for (byte k = 0; k < 9; k++)
								{
									if (i != (i / 3 * 3 + k / 3) || j != (j / 3 * 3 + k % 3))
									{
										if (grid[i / 3 * 3 + k / 3, j / 3 * 3 + k % 3].availableVariantes[num] == 0) countNotAvailableVariantes++;

										//grid[i/3*3 + k/3, j/3*3 + k%3].availableVariantes[grid[i,j].solve - 1] = 0;
									}
								}
								if (countNotAvailableVariantes == 8)
								{
									grid[i, j].solve = (byte)(num + 1);
									grid[i, j].isSolved = true;
									grid[i, j].clearVariantes((byte)(num + 1));
									break;
								}
							}
						}

						grid[i, j].refreshStatusSolving();
						//if (grid[i,j].solve == 0) newCounterUnknown++;
						//Console.Write("{0} ", myCell[i,j].solve);
					}
					//Console.WriteLine();
				}
				//Console.WriteLine("Unknown: {0}", newCounterUnknown);
				//Console.WriteLine();
			}
			//Console.WriteLine("You entered '{0}'", input);
		}
	}
}
