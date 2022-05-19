using System;
using System.Collections.Generic;
using System.Text;

namespace JapanGames2
{
    public class MySudokuGrid
    {
		private struct MyIneffectiveSubstitutionCandidate
		{
			public byte i, j, candidate;
			public MyIneffectiveSubstitutionCandidate(byte i_Index, byte j_Index, byte candidateIndex)
			{
				i = i_Index;
				j = j_Index;
				candidate = candidateIndex;
			}
		}

		List<MyIneffectiveSubstitutionCandidate> myIneffectiveCandidates = new List<MyIneffectiveSubstitutionCandidate>();

		public MySudokuCell[,] mySudokuCells;

		public MySudokuGrid(ref MySudokuCell[,] grid)
        {
			mySudokuCells = grid;
        }

		private bool CheckFaultSolving(MySudokuCell[,] grid) //Проверка конфликтов при решении методом подстановки
		{
			bool isFault = false;

			for (byte i = 0; i < 9; i++)
				for (byte j = 0; j < 9; j++)
                {
					if (grid[i, j].SolveResult.HasValue)
                    {
						//Проверим отсутствие конфликтов в столбах
						if (!isFault)
							for (byte i_ = 0; i_ < 9; i_++)
							{
								if (i_ != i && grid[i_, j].SolveResult.HasValue)
									if (grid[i_, j].SolveResult.Value == grid[i, j].SolveResult.Value)
									{
										isFault = true;
										break;
									}
							}

						//Проверим отсутствие конфликтов в строках
						if (!isFault)
							for (byte j_ = 0; j_ < 9; j_++)
							{
								if (j_ != j && grid[i, j_].SolveResult.HasValue)
									if (grid[i, j_].SolveResult.Value == grid[i, j].SolveResult.Value)
									{
										isFault = true;
										break;
									}
							}

						//Проверим отсутствие конфликтов в квадратах
						if (!isFault)
							for (byte i_ = (byte)((i / 3) * 3); i_ < (byte)((i / 3) * 3 + 3); i_++)
								for (byte j_ = (byte)((j / 3) * 3); j_ < (byte)((j / 3) * 3 + 3); j_++)
								{
									if ((i_ != i || j_ != j) && grid[i_, j_].SolveResult.HasValue)
										if (grid[i_, j_].SolveResult.Value == grid[i, j].SolveResult.Value)
										{
											isFault = true;
											break;
										}
								}
					}
                }

			return isFault;
		}

		public bool SolvingProcedureSubstitution(MySudokuCell[,] grid) //Метод подстановки
        {
			bool isSolvingProgress = false;

			MySudokuCell[,] gridTemp = new MySudokuCell[9,9];

			byte minCand_i = 0, minCand_j = 0, minCand = 9;

			for (byte i = 0; i < 9; i++)
            {
				for (byte j = 0; j < 9; j++)
				{
					//Копируем текущее состояние сетки во временную для применения метода подстановки
					gridTemp[i, j] = (MySudokuCell)grid[i, j].Clone();

					//И ищем первую ячейку с минимальным кол-вом кандидатов
					if (!gridTemp[i, j].SolveResult.HasValue)
						if (gridTemp[i, j].CountAvailableCandidates < minCand)
						{
							minCand_i = i;
							minCand_j = j;
							minCand = gridTemp[i, j].CountAvailableCandidates;
						}
					if (gridTemp[i, j].SolveResult.HasValue)
						Console.Write("{0} ", gridTemp[i, j].SolveResult.Value);
					else Console.Write("0 ");
				}
				Console.WriteLine();
			}

			byte? currentCandIndex = null;

			//Осуществляем подстановку
			for (byte k = 0; k < 9; k++)
            {
				if (gridTemp[minCand_i, minCand_j].GetAvailableCandidate(k) != 0 
					&& !myIneffectiveCandidates.Contains(new MyIneffectiveSubstitutionCandidate(minCand_i, minCand_j, k)))
                {
					
					gridTemp[minCand_i, minCand_j].SolveResult = gridTemp[minCand_i, minCand_j].GetAvailableCandidate(k);
					currentCandIndex = k;
					break;
				}
            }

			Console.WriteLine("Coords({0},{1}) - CandForTry:{2}", minCand_i, minCand_j, currentCandIndex);

			//Пытаемся решить стандартными методами
			bool isHaveTempGridProgress = true;

			if (currentCandIndex.HasValue)
				while (isHaveTempGridProgress)
				{
					Console.WriteLine("1");
					if (!SolvingProcedure1(gridTemp))
					{
						Console.WriteLine("2");
						if (!SolvingProcedure2(gridTemp))
						{
							Console.WriteLine("3");
							if (!SolvingProcedure3(gridTemp))
							{
								Console.WriteLine("4");
								if (!SolvingProcedure4(gridTemp))
								{
									Console.WriteLine("5");
									if (!SolvingProcedure5(gridTemp))
									{
										isHaveTempGridProgress = false;
										Console.WriteLine("NO PROGRESS");
									}
								}
							}
						}
					}

					if (CheckFaultSolving(gridTemp))
					{
						grid[minCand_i, minCand_j].SetAvailableCandidate(currentCandIndex.Value, 0);
						isSolvingProgress = true;
						myIneffectiveCandidates.Clear();
						Console.WriteLine("CANDIDATE EXCLUDED");
						break;
					}

					if (!isHaveTempGridProgress)
                    {
						myIneffectiveCandidates.Add(new MyIneffectiveSubstitutionCandidate(minCand_i, minCand_j, currentCandIndex.Value));
						isSolvingProgress = true;
					}
				}


			return isSolvingProgress;
		}
				
		public bool SolvingProcedure5(MySudokuCell[,] grid) //Связанные пары (бабочка)
        {
			bool isSolvingProgress = false;

			//Проходим по кандидатам
			for (byte candidate = 0; candidate < 9; candidate++)
            {
				//Ищем пару кандидатов в строках
				for (byte i = 0; i < 9; i++)
                {
					List<byte> indexCells = new List<byte>();

					//Проходим по ячейкам и записываем индексы кандидатов
					for (byte k = 0; k < 9; k++)
                    {
						if (!grid[i, k].SolveResult.HasValue && grid[i, k].GetAvailableCandidate(candidate) != 0)
                        {
							indexCells.Add(k);
						}
                    }

					//Если в списке пара кандидатов, то ищем для неё параллельную пару 
					if (indexCells.Count == 2)
                    {
						//Блок поиска пар для удаления невозможных кандидатов в столбах
						for (byte i_ = (byte)(i + 1); i_ < 9; i_++)
                        {
							List<byte> indexCells_ = new List<byte>();

							//Проходим по ячейкам и записываем индексы кандидатов для проверки совпадения
							for (byte k = 0; k < 9; k++)
							{
								if (!grid[i_, k].SolveResult.HasValue && grid[i_, k].GetAvailableCandidate(candidate) != 0)
								{
									indexCells_.Add(k);
								}
							}
							
							//Если две параллельные пары совпадают, то удаляем кандидата из столбов
							if (indexCells_.Count == 2) 
								if (indexCells[0] == indexCells_[0] && indexCells[1] == indexCells_[1]) 
									for (byte x = 0; x < 9; x++)
                                    {
										if (x != i && x != i_)
                                        {
											foreach (byte cell in indexCells)
                                            {
												if (grid[x, cell].GetAvailableCandidate(candidate) != 0
													&& !grid[x, cell].SolveResult.HasValue)
                                                {
													grid[x, cell].SetAvailableCandidate(candidate, 0);
													isSolvingProgress = true;
												}
                                            }
                                        }
                                    }

						}

						//Блок поиска пар для удаления невозможных кандидатов в квадратах

						//Если пара относится к разным квадратам, то исследуем дальше
						if (indexCells[0] / 3 != indexCells[1] / 3) 
							//Интересуют только области рассматриваемых квадратов
							for (byte i_ = (byte)((i / 3) * 3); i_ < (byte)((i / 3) * 3 + 3); i_++)
                            {
								List<byte> indexCells_ = new List<byte>();

								if (i != i_)
                                {
									for (byte k = 0; k < 9; k++)
									{
										if (!grid[i_, k].SolveResult.HasValue && grid[i_, k].GetAvailableCandidate(candidate) != 0)
										{
											indexCells_.Add(k);
										}
									}

									//Если две обнаруженные пары удовлетворяют условиям, то удаляем кандидата из квадратов
									if (indexCells_.Count == 2)
										if (indexCells[0] / 3 == indexCells_[0] / 3 && indexCells[1] / 3 == indexCells_[1] / 3)
											for (byte i_line = (byte)((i / 3) * 3); i_line < (byte)((i / 3) * 3 + 3); i_line++) 
												if (i_line != i_ && i_line != i)
													for (byte x = 0; x < 9; x++)
													{
														if ((x / 3 == indexCells[0] / 3 || x / 3 == indexCells[1] / 3) && !grid[i_line, x].SolveResult.HasValue)
														{
															if (grid[i_line, x].GetAvailableCandidate(candidate) != 0)
															{
																grid[i_line, x].SetAvailableCandidate(candidate, 0);
																isSolvingProgress = true;
															}
														}
													}
								}
							}
					}
				}

				//Ищем пару кандидатов в столбах
				for (byte j = 0; j < 9; j++)
				{
					List<byte> indexCells = new List<byte>();

					//Проходим по ячейкам и записываем индексы кандидатов
					for (byte k = 0; k < 9; k++)
					{
						if (!grid[k, j].SolveResult.HasValue && grid[k, j].GetAvailableCandidate(candidate) != 0)
						{
							indexCells.Add(k);
						}
					}

					//Если в списке пара кандидатов, то ищем для неё параллельную пару 
					if (indexCells.Count == 2)
					{
						//Блок поиска пар для удаления невозможных кандидатов в столбах
						for (byte j_ = (byte)(j + 1); j_ < 9; j_++)
						{
							List<byte> indexCells_ = new List<byte>();

							//Проходим по ячейкам и записываем индексы кандидатов для проверки совпадения
							for (byte k = 0; k < 9; k++)
							{
								if (!grid[k, j_].SolveResult.HasValue && grid[k, j_].GetAvailableCandidate(candidate) != 0)
								{
									indexCells_.Add(k);
								}
							}

							//Если две параллельные пары совпадают, то удаляем кандидата из строк
							if (indexCells_.Count == 2)
								if (indexCells[0] == indexCells_[0] && indexCells[1] == indexCells_[1])
									for (byte x = 0; x < 9; x++)
									{
										if (x != j && x != j_)
										{
											foreach (byte cell in indexCells)
											{
												if (grid[cell, x].GetAvailableCandidate(candidate) != 0
													&& !grid[cell, x].SolveResult.HasValue)
												{
													grid[cell, x].SetAvailableCandidate(candidate, 0);
													isSolvingProgress = true;
												}
											}
										}
									}

						}

						//Блок поиска пар для удаления невозможных кандидатов в квадратах

						//Если пара относится к разным квадратам, то исследуем дальше
						if (indexCells[0] / 3 != indexCells[1] / 3)
							//Интересуют только области рассматриваемых квадратов
							for (byte j_ = (byte)((j / 3) * 3); j_ < (byte)((j / 3) * 3 + 3); j_++)
							{
								if (j != j_)
								{
									List<byte> indexCells_ = new List<byte>();

									for (byte k = 0; k < 9; k++)
									{
										if (!grid[k, j_].SolveResult.HasValue && grid[k, j_].GetAvailableCandidate(candidate) != 0)
										{
											indexCells_.Add(k);
										}
									}

									//Если две обнаруженные пары удовлетворяют условиям, то удаляем кандидата из квадратов
									if (indexCells_.Count == 2)
										if (indexCells[0] / 3 == indexCells_[0] / 3 && indexCells[1] / 3 == indexCells_[1] / 3)
											for (byte j_line = (byte)((j / 3) * 3); j_line < (byte)((j / 3) * 3 + 3); j_line++)
												if (j_line != j_ && j_line != j)
													for (byte x = 0; x < 9; x++)
													{
														if ((x / 3 == indexCells[0] / 3 || x / 3 == indexCells[1] / 3) && !grid[x, j_line].SolveResult.HasValue)
														{
															if (grid[x, j_line].GetAvailableCandidate(candidate) != 0)
															{
																grid[x, j_line].SetAvailableCandidate(candidate, 0);
																isSolvingProgress = true;
															}
														}
													}
								}
							}
					}
				}

				//Ищем пару кандидатов в квадратах
				for (byte i = 0; i < 3; i++)
					for (byte j = 0; j < 3; j++)
                    {
						List<byte> indexCells = new List<byte>();

						//Проходим по ячейкам квадрата и записываем индексы кандидатов
						for (byte k = 0; k < 9; k++)
						{
							if (!grid[i * 3 + k / 3, j * 3 + k % 3].SolveResult.HasValue 
								&& grid[i * 3 + k / 3, j * 3 + k % 3].GetAvailableCandidate(candidate) != 0)
							{
								indexCells.Add(k);
							}
						}

						//Если в списке пара кандидатов, то ищем пару в другом квадрате
						if (indexCells.Count == 2)
                        {
							for (byte i_ = 0; i_ < 3; i_++)
								for (byte j_ = 0; j_ < 3; j_++)
                                {
									if (i != i_ && j != j_)
                                    {
										if ((i == i_ && j != j_) || (i != i_ && j == j_))
                                        {
											List<byte> indexCells_ = new List<byte>();

											//Проходим по ячейкам и записываем индексы кандидатов для проверки совпадения
											for (byte k = 0; k < 9; k++)
											{
												if (!grid[i_ * 3 + k / 3, j_ * 3 + k % 3].SolveResult.HasValue 
													&& grid[i_ * 3 + k / 3, j_ * 3 + k % 3].GetAvailableCandidate(candidate) != 0)
												{
													indexCells_.Add(k);
												}
											}

											//Если две обнаруженные пары удовлетворяют условиям
											if (indexCells_.Count == 2)
                                            {
												//, то удаляем кандидата из строк
												if (i == i_)
													if (indexCells[0] / 3 != indexCells[1] / 3)
														if ((indexCells[0] / 3 == indexCells_[0] / 3 && indexCells[1] / 3 == indexCells_[1] / 3)
															|| (indexCells[0] / 3 == indexCells_[1] / 3 && indexCells[1] / 3 == indexCells_[0] / 3))
															for (byte x = 0; x < 9; x++)
															{
																if (x / 3 != j && x / 3 != j_)
																{
																	foreach (byte cell in indexCells)
																	{
																		if (grid[i * 3 + cell / 3, x].GetAvailableCandidate(candidate) != 0
																			&& !grid[i * 3 + cell / 3, x].SolveResult.HasValue)
																		{
																			grid[i * 3 + cell / 3, x].SetAvailableCandidate(candidate, 0);
																			isSolvingProgress = true;
																		}
																	}
																}
															}

												//, то удаляем кандидата из столбов
												if (j == j_)
													if (indexCells[0] % 3 != indexCells[1] % 3)
														if ((indexCells[0] % 3 == indexCells_[0] % 3 && indexCells[1] % 3 == indexCells_[1] % 3)
															|| (indexCells[0] % 3 == indexCells_[1] % 3 && indexCells[1] % 3 == indexCells_[0] % 3))
															for (byte x = 0; x < 9; x++)
															{
																if (x / 3 != i && x / 3 != i_)
																{
																	foreach (byte cell in indexCells)
																	{
																		if (grid[x, j * 3 + cell % 3].GetAvailableCandidate(candidate) != 0
																			&& !grid[x, j * 3 + cell % 3].SolveResult.HasValue)
																		{
																			grid[x, j * 3 + cell % 3].SetAvailableCandidate(candidate, 0);
																			isSolvingProgress = true;
																		}
																	}
																}
															}
											}
										}
                                    }
                                }
						}
					}
			}

			return isSolvingProgress;
		}

		public bool SolvingProcedure4(MySudokuCell[,] grid) //Скрытые группы кандидатов
        {
			bool isSolvingProgress = false;

			//Ищем в строках скрытые группы кандидатов
			for (byte i = 0; i < 9; i++)
			{
				//Выясняем размер максимально возможной группы кандидатов
				byte counter = 0;
				for (byte k = 0; k < 9; k++) if (!grid[i, k].SolveResult.HasValue) counter++;
				byte maxSize = 0;
				if (counter > 3)
					if (counter % 2 == 0) //Имеет смысл искать скрытые кандидаты с размером группы меньше половины неизвестных клеток
						maxSize = (byte)(counter / 2 - 1);
					else maxSize = (byte)(counter / 2);

				//Ищем группы кандидатов начиная от размера группы в 2е клетки
				if (maxSize >= 2)
				{
					for (byte sizeGroup = 2; sizeGroup <= maxSize; sizeGroup++)
					{
						List<byte>[] arrayIndexCells = new List<byte>[9];

						//Ищем совпадения по доступным кандидатам
						for (byte candidate = 0; candidate < 9; candidate++)
						{
							List<byte> indexCells = new List<byte>();

							//Проходим по области в поисках текущего кандидата
							for (byte j = 0; j < 9; j++)
                            {
								if (!grid[i, j].SolveResult.HasValue)
                                {
									if (grid[i, j].GetAvailableCandidate(candidate) != 0)
                                    {
										indexCells.Add(j);
									}
								}
                            }

							//Если число кандидатов меньше или равно размеру рассматриваемой группы - записываем группу индексов в массив
							if (indexCells.Count >= 2 && indexCells.Count <= sizeGroup)
                            {
								arrayIndexCells[candidate] = indexCells;
							}
						}

						//Проверяем группы кандидатов на совпадение индексов ячеек
						for (byte candidateBase = 0; candidateBase < 9; candidateBase++)
                        {
							if (arrayIndexCells[candidateBase] != null)
                            {
								List<byte> indexCellsOfGroup;
								List<byte> correctCandidateMembers = new List<byte>();

								indexCellsOfGroup = arrayIndexCells[candidateBase];
								correctCandidateMembers.Add(candidateBase);

								//Проходим по кандидатам и при удовлетворении условий добавляем их в группу
								for (byte candidateCheck = 0; candidateCheck < 9; candidateCheck++)
                                {
									if (candidateBase != candidateCheck && arrayIndexCells[candidateCheck] != null)
                                    {
										bool isCorrectMember = false;
										byte correctCandidates = 0;

										//Проверяем совпадение индексов ячеек у проверяемых кандмдатов
										foreach (byte indexCell in arrayIndexCells[candidateCheck])
										{
											if (!indexCellsOfGroup.Contains(indexCell))
											{
												if (indexCellsOfGroup.Count < sizeGroup)
												{
													indexCellsOfGroup.Add(indexCell);
													correctCandidates++;
												}
												else
												{
													isCorrectMember = false;
													break;
												}
											}
											else
											{
												correctCandidates++;
											}

											if (correctCandidates >= 2) isCorrectMember = true;
										}

										if (isCorrectMember)
                                        {
											correctCandidateMembers.Add(candidateCheck);
										}
									}
								}

								//Если выборка соответствует целевому размеру группы, тогда удаляем лишние кандидаты из обнаруженной группы
								if (correctCandidateMembers.Count == sizeGroup && indexCellsOfGroup.Count == sizeGroup)
								{
									foreach (byte index in indexCellsOfGroup)
                                    {
										for (byte k = 0; k < 9; k++)
                                        {
											if (!correctCandidateMembers.Contains(k))
                                            {
												if (grid[i, index].GetAvailableCandidate(k) != 0)
												{
													grid[i, index].SetAvailableCandidate(k, 0);
													isSolvingProgress = true;
												}
											}
										}
									}
								}
							}
                        }
					}
				}

				for (byte j = 0; j < 9; j++)
					grid[i, j].CheckSingleCandidate();
			}

			//Ищем в столбах скрытые группы кандидатов
			for (byte j = 0; j < 9; j++)
			{
				//Выясняем размер максимально возможной группы кандидатов
				byte counter = 0;
				for (byte k = 0; k < 9; k++) if (!grid[k, j].SolveResult.HasValue) counter++;
				byte maxSize = 0;
				if (counter > 3)
					if (counter % 2 == 0) //Имеет смысл искать скрытые кандидаты с размером группы меньше половины неизвестных клеток
						maxSize = (byte)(counter / 2 - 1);
					else maxSize = (byte)(counter / 2);

				//Ищем группы кандидатов начиная от размера группы в 2е клетки
				if (maxSize >= 2)
				{
					for (byte sizeGroup = 2; sizeGroup <= maxSize; sizeGroup++)
					{
						List<byte>[] arrayIndexCells = new List<byte>[9];

						//Ищем совпадения по доступным кандидатам
						for (byte candidate = 0; candidate < 9; candidate++)
						{
							List<byte> indexCells = new List<byte>();

							//Проходим по области в поисках текущего кандидата
							for (byte i = 0; i < 9; i++)
							{
								if (!grid[i, j].SolveResult.HasValue)
								{
									if (grid[i, j].GetAvailableCandidate(candidate) != 0)
									{
										indexCells.Add(i);
									}
								}
							}

							//Если число кандидатов меньше или равно размеру рассматриваемой группы - записываем группу индексов в массив
							if (indexCells.Count >= 2 && indexCells.Count <= sizeGroup)
							{
								arrayIndexCells[candidate] = indexCells;
							}
						}

						//Проверяем группы кандидатов на совпадение индексов ячеек
						for (byte candidateBase = 0; candidateBase < 9; candidateBase++)
						{
							if (arrayIndexCells[candidateBase] != null)
							{
								List<byte> indexCellsOfGroup;
								List<byte> correctCandidateMembers = new List<byte>();

								indexCellsOfGroup = arrayIndexCells[candidateBase];
								correctCandidateMembers.Add(candidateBase);

								//Проходим по кандидатам и при удовлетворении условий добавляем их в группу
								for (byte candidateCheck = 0; candidateCheck < 9; candidateCheck++)
								{
									if (candidateBase != candidateCheck && arrayIndexCells[candidateCheck] != null)
									{
										bool isCorrectMember = false;
										byte correctCandidates = 0;

										//Проверяем совпадение индексов ячеек у проверяемых кандмдатов
										foreach (byte indexCell in arrayIndexCells[candidateCheck])
										{
											if (!indexCellsOfGroup.Contains(indexCell))
											{
												if (indexCellsOfGroup.Count < sizeGroup)
												{
													indexCellsOfGroup.Add(indexCell);
													correctCandidates++;
												}
												else
												{
													isCorrectMember = false;
													break;
												}
											}
											else
											{
												correctCandidates++;
											}

											if (correctCandidates >= 2) isCorrectMember = true;
										}

										if (isCorrectMember)
										{
											correctCandidateMembers.Add(candidateCheck);
										}
									}
								}

								//Если выборка соответствует целевому размеру группы, тогда удаляем лишние кандидаты из обнаруженной группы
								if (correctCandidateMembers.Count == sizeGroup && indexCellsOfGroup.Count == sizeGroup)
								{
									foreach (byte index in indexCellsOfGroup)
									{
										for (byte k = 0; k < 9; k++)
										{
											if (!correctCandidateMembers.Contains(k))
											{
												if (grid[index, j].GetAvailableCandidate(k) != 0)
												{
													grid[index, j].SetAvailableCandidate(k, 0);
													isSolvingProgress = true;
												}
											}
										}
									}
								}
							}
						}
					}
				}

				for (byte i = 0; i < 9; i++)
					grid[i, j].CheckSingleCandidate();
			}
			
			//Ищем в квадратах скрытые группы кандидатов
			for (byte i = 0; i < 3; i++)
			{
				for (byte j = 0; j < 3; j++)
                {
					//Выясняем размер максимально возможной группы кандидатов
					byte counter = 0;
					for (byte k = 0; k < 9; k++) if (!grid[i * 3 + k / 3, j * 3 + k % 3].SolveResult.HasValue) counter++;
					byte maxSize = 0;
					if (counter > 3)
						if (counter % 2 == 0) //Имеет смысл искать скрытые кандидаты с размером группы меньше половины неизвестных клеток
							maxSize = (byte)(counter / 2 - 1);
						else maxSize = (byte)(counter / 2);

					//Ищем группы кандидатов начиная от размера группы в 2е клетки
					if (maxSize >= 2)
					{
						for (byte sizeGroup = 2; sizeGroup <= maxSize; sizeGroup++)
						{
							List<byte>[] arrayIndexCells = new List<byte>[9];

							//Ищем совпадения по доступным кандидатам
							for (byte candidate = 0; candidate < 9; candidate++)
							{
								List<byte> indexCells = new List<byte>();

								//Проходим по области в поисках текущего кандидата
								for (byte k = 0; k < 9; k++)
								{
									if (!grid[i * 3 + k / 3, j * 3 + k % 3].SolveResult.HasValue)
									{
										if (grid[i * 3 + k / 3, j * 3 + k % 3].GetAvailableCandidate(candidate) != 0)
										{
											indexCells.Add(k);
										}
									}
								}

								//Если число кандидатов меньше или равно размеру рассматриваемой группы - записываем группу индексов в массив
								if (indexCells.Count >= 2 && indexCells.Count <= sizeGroup)
								{
									arrayIndexCells[candidate] = indexCells;
								}
							}

							//Проверяем группы кандидатов на совпадение индексов ячеек
							for (byte candidateBase = 0; candidateBase < 9; candidateBase++)
							{
								if (arrayIndexCells[candidateBase] != null)
								{
									List<byte> indexCellsOfGroup;
									List<byte> correctCandidateMembers = new List<byte>();

									indexCellsOfGroup = arrayIndexCells[candidateBase];
									correctCandidateMembers.Add(candidateBase);

									//Проходим по кандидатам и при удовлетворении условий добавляем их в группу
									for (byte candidateCheck = 0; candidateCheck < 9; candidateCheck++)
									{
										if (candidateBase != candidateCheck && arrayIndexCells[candidateCheck] != null)
										{
											bool isCorrectMember = false;
											byte correctCandidates = 0;

											//Проверяем совпадение индексов ячеек у проверяемых кандмдатов
											foreach (byte indexCell in arrayIndexCells[candidateCheck])
											{
												if (!indexCellsOfGroup.Contains(indexCell))
												{
													if (indexCellsOfGroup.Count < sizeGroup)
													{
														indexCellsOfGroup.Add(indexCell);
														correctCandidates++;
													}
													else
													{
														isCorrectMember = false;
														break;
													}
												}
												else
												{
													correctCandidates++;
												}

												if (correctCandidates >= 2) isCorrectMember = true;
											}

											if (isCorrectMember)
											{
												correctCandidateMembers.Add(candidateCheck);
											}
										}
									}

									//Если выборка соответствует целевому размеру группы, тогда удаляем лишние кандидаты из обнаруженной группы
									if (correctCandidateMembers.Count == sizeGroup && indexCellsOfGroup.Count == sizeGroup)
									{
										foreach (byte index in indexCellsOfGroup)
										{
											for (byte k = 0; k < 9; k++)
											{
												if (!correctCandidateMembers.Contains(k))
												{
													if (grid[i * 3 + index / 3, j * 3 + index % 3].GetAvailableCandidate(k) != 0)
													{
														grid[i * 3 + index / 3, j * 3 + index % 3].SetAvailableCandidate(k, 0);
														isSolvingProgress = true;
													}
												}
											}
										}
									}
								}
							}
						}
					}

					for (byte k = 0; k < 9; k++)
						grid[i * 3 + k / 3, j * 3 + k % 3].CheckSingleCandidate();
				}
					
			}

			return isSolvingProgress;
		}

		public bool SolvingProcedure3(MySudokuCell[,] grid) //Очевидные группы кандидатов
		{
			bool isSolvingProgress = false;

			Console.WriteLine("3 процедура - 1");
			//Ищем в строках очевидные группы кандидатов
			for (byte i = 0; i < 9; i++)
            {
				//Выясняем размер максимально возможной группы кандидатов
				byte counter = 0;
				for (byte k = 0; k < 9; k++) if (!grid[i, k].SolveResult.HasValue) counter++;
				byte maxSize = counter--;

				//Ищем группы кандидатов начиная от размера группы в 2е клетки
				if (maxSize >= 2)
                {
					for (byte sizeGroup = 2; sizeGroup <= maxSize; sizeGroup++)
                    {
						//По очереди берем ячейку и ищем для неё подходящую группу
						for (byte j = 0; j < 9; j++) 
						{
							if (grid[i, j].CountAvailableCandidates <= sizeGroup && !grid[i, j].SolveResult.HasValue)
                            {
								List<byte> memberPositions = new List<byte>();
								List<byte> digitsInGroup = new List<byte>();

								//Сохраняем сведения основной ячейки
								memberPositions.Add(j);
								for (byte k = 0; k < 9; k++)
									if (grid[i, j].GetAvailableCandidate(k) != 0) digitsInGroup.Add(grid[i, j].GetAvailableCandidate(k));

								//Ищем подходящие ячейки для группы
								for (byte k = 0; k < 9; k++)
								{
									if (j != k && !grid[i, k].SolveResult.HasValue && grid[i, k].CountAvailableCandidates <= sizeGroup)
                                    {
										bool isCorrectMember = false;
										byte correctCandidates = 0;
										
										//Проверяем условия соответствия кандидатов для группы
										for (byte x = 0; x < 9; x++)
										{
											if (grid[i, k].GetAvailableCandidate(x) != 0)
                                            {
												if (!digitsInGroup.Contains(grid[i, k].GetAvailableCandidate(x)))
                                                {
													if (digitsInGroup.Count < sizeGroup)
                                                    {
														digitsInGroup.Add(grid[i, k].GetAvailableCandidate(x));
														correctCandidates++;
													}
													else
                                                    {
														isCorrectMember = false;
														break;
													} 
												}
												else
                                                {
													correctCandidates++;
                                                }
                                            }
											if (correctCandidates >= 2) isCorrectMember = true;
										}

										//Если член группы удовлетворил условиям - учитываем его в группе, указав его индекс в строке
										if (isCorrectMember)
                                        {
											memberPositions.Add(k);
										}
									}
								}

								//Если выборка соответствует целевому размеру группы, тогда удаляем обнаруженные кандидаты из других клеток
								if (memberPositions.Count == sizeGroup && digitsInGroup.Count == sizeGroup)
                                {
									for (byte k = 0; k < 9; k++)
                                    {
										if (!memberPositions.Contains(k) && !grid[i, k].SolveResult.HasValue)
                                        {
											foreach (byte digit in digitsInGroup)
                                            {
												if (grid[i, k].GetAvailableCandidate((byte)(digit - 1)) != 0)
                                                {
													grid[i, k].SetAvailableCandidate((byte)(digit - 1), 0);
													isSolvingProgress = true;
												}
											}
                                        }
                                    }
                                }
							}

							grid[i, j].CheckSingleCandidate();
						}
                    }
                }
            }

			//Ищем в столбах очевидные группы кандидатов
			for (byte j = 0; j < 9; j++)
			{
				//Выясняем размер максимально возможной группы кандидатов
				byte counter = 0;
				for (byte k = 0; k < 9; k++) if (!grid[k, j].SolveResult.HasValue) counter++;
				byte maxSize = counter--;

				//Ищем группы кандидатов начиная от размера группы в 2е клетки
				if (maxSize >= 2)
				{
					for (byte sizeGroup = 2; sizeGroup <= maxSize; sizeGroup++)
					{
						//По очереди берем ячейку и ищем для неё подходящую группу
						for (byte i = 0; i < 9; i++)
						{
							if (grid[i, j].CountAvailableCandidates <= sizeGroup && !grid[i, j].SolveResult.HasValue)
							{
								List<byte> memberPositions = new List<byte>();
								List<byte> digitsInGroup = new List<byte>();

								//Сохраняем сведения основной ячейки
								memberPositions.Add(i);
								for (byte k = 0; k < 9; k++)
									if (grid[i, j].GetAvailableCandidate(k) != 0) digitsInGroup.Add(grid[i, j].GetAvailableCandidate(k));

								//Ищем подходящие ячейки для группы
								for (byte k = 0; k < 9; k++)
								{
									if (i != k && !grid[k, j].SolveResult.HasValue && grid[k, j].CountAvailableCandidates <= sizeGroup)
									{
										bool isCorrectMember = false;
										byte correctCandidates = 0;

										//Проверяем условия соответствия кандидатов для группы
										for (byte x = 0; x < 9; x++)
										{
											if (grid[k, j].GetAvailableCandidate(x) != 0)
											{
												if (!digitsInGroup.Contains(grid[k, j].GetAvailableCandidate(x)))
												{
													if (digitsInGroup.Count < sizeGroup)
													{
														digitsInGroup.Add(grid[k, j].GetAvailableCandidate(x));
														correctCandidates++;
													}
													else
													{
														isCorrectMember = false;
														break;
													}
												}
												else
												{
													correctCandidates++;
												}
											}
											if (correctCandidates >= 2) isCorrectMember = true;
										}

										//Если член группы удовлетворил условиям - учитываем его в группе, указав его индекс в строке
										if (isCorrectMember)
										{
											memberPositions.Add(k);
										}
									}
								}

								//Если выборка соответствует целевому размеру группы, тогда удаляем обнаруженные кандидаты из других клеток
								if (memberPositions.Count == sizeGroup && digitsInGroup.Count == sizeGroup)
								{
									for (byte k = 0; k < 9; k++)
									{
										if (!memberPositions.Contains(k) && !grid[k, j].SolveResult.HasValue)
										{
											foreach (byte digit in digitsInGroup)
											{
												if (grid[k, j].GetAvailableCandidate((byte)(digit - 1)) != 0)
												{
													grid[k, j].SetAvailableCandidate((byte)(digit - 1), 0);
													isSolvingProgress = true;
												}
											}
										}
									}
								}
							}

							grid[i, j].CheckSingleCandidate();
						}
					}
				}
			}
			
			//Ищем в квадратах очевидные группы кандидатов
			for (byte i = 0; i < 3; i++)
            {
				for (byte j = 0; j < 3; j++)
				{
					//Выясняем размер максимально возможной группы кандидатов
					byte counter = 0;
					for (byte k = 0; k < 9; k++) if (!grid[i * 3 + k / 3, j * 3 + k % 3].SolveResult.HasValue) counter++;
					byte maxSize = counter--;

					//Ищем группы кандидатов начиная от размера группы в 2е клетки
					if (maxSize >= 2)
					{
						for (byte sizeGroup = 2; sizeGroup <= maxSize; sizeGroup++)
						{
							//По очереди берем ячейку и ищем для неё подходящую группу
							for (byte n = 0; n < 9; n++)
							{
								if (grid[i * 3 + n / 3, j * 3 + n % 3].CountAvailableCandidates <= sizeGroup && !grid[i * 3 + n / 3, j * 3 + n % 3].SolveResult.HasValue)
								{
									List<byte> memberPositions = new List<byte>();
									List<byte> digitsInGroup = new List<byte>();

									//Сохраняем сведения основной ячейки
									memberPositions.Add(n);
									for (byte k = 0; k < 9; k++)
										if (grid[i * 3 + n / 3, j * 3 + n % 3].GetAvailableCandidate(k) != 0) digitsInGroup.Add(grid[i * 3 + n / 3, j * 3 + n % 3].GetAvailableCandidate(k));

									//Ищем подходящие ячейки для группы
									for (byte k = 0; k < 9; k++)
									{
										if (n != k && !grid[i * 3 + k / 3, j * 3 + k % 3].SolveResult.HasValue && grid[i * 3 + k / 3, j * 3 + k % 3].CountAvailableCandidates <= sizeGroup)
										{
											bool isCorrectMember = false;
											byte correctCandidates = 0;

											//Проверяем условия соответствия кандидатов для группы
											for (byte x = 0; x < 9; x++)
											{
												if (grid[i * 3 + k / 3, j * 3 + k % 3].GetAvailableCandidate(x) != 0)
												{
													if (!digitsInGroup.Contains(grid[i * 3 + k / 3, j * 3 + k % 3].GetAvailableCandidate(x)))
													{
														if (digitsInGroup.Count < sizeGroup)
														{
															digitsInGroup.Add(grid[i * 3 + k / 3, j * 3 + k % 3].GetAvailableCandidate(x));
															correctCandidates++;
														}
														else
														{
															isCorrectMember = false;
															break;
														}
													}
													else
													{
														correctCandidates++;
													}
												}
												if (correctCandidates >= 2) isCorrectMember = true;
											}

											//Если член группы удовлетворил условиям - учитываем его в группе, указав его индекс в строке
											if (isCorrectMember)
											{
												memberPositions.Add(k);
											}
										}
									}

									//Если выборка соответствует целевому размеру группы, тогда удаляем обнаруженные кандидаты из других клеток
									if (memberPositions.Count == sizeGroup && digitsInGroup.Count == sizeGroup)
									{
										for (byte k = 0; k < 9; k++)
										{
											if (!memberPositions.Contains(k) && !grid[i * 3 + k / 3, j * 3 + k % 3].SolveResult.HasValue)
											{
												foreach (byte digit in digitsInGroup)
												{
													if (grid[i * 3 + k / 3, j * 3 + k % 3].GetAvailableCandidate((byte)(digit - 1)) != 0)
													{
														grid[i * 3 + k / 3, j * 3 + k % 3].SetAvailableCandidate((byte)(digit - 1), 0);
														isSolvingProgress = true;
													}
												}
											}
										}
									}
								}

								grid[i * 3 + n / 3, j * 3 + n % 3].CheckSingleCandidate();
							}
						}
					}
				}
			}
			
			return isSolvingProgress;
		}

		public bool SolvingProcedure2(MySudokuCell[,] grid) //Исключение кандидатов
		{
			bool isSolvingProgress = false;

			Console.WriteLine("2 процедура - 1");
			//Исключаем очевидно парных кандидатов квадратов из строк и столбов
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
							if (!grid[i * 3 + (k / 3), j * 3 + (k % 3)].SolveResult.HasValue
							&& grid[i * 3 + (k / 3), j * 3 + (k % 3)].GetAvailableCandidate(num) != 0)
							{
								countNumbers++;
								if (countNumbers == 1) { pos1 = k; }
								if (countNumbers == 2) { pos2 = k; }
							}
						}
						//Console.WriteLine("{0} {1} {2} {3}", num + 1, countNumbers, pos1, pos2);

						if (countNumbers == 2)
						{
							if (pos1 % 3 == pos2 % 3)
							{
								for (byte x = 0; x < 9; x++)
									if (!grid[x, j * 3 + (pos1 % 3)].SolveResult.HasValue && x / 3 != i)
                                    {
										if (grid[x, j * 3 + (pos1 % 3)].GetAvailableCandidate(num) != 0)
                                        {
											grid[x, j * 3 + (pos1 % 3)].SetAvailableCandidate(num, 0);
											//grid[x, j * 3 + (pos1 % 3)].RefreshCountAvailableCandidates();
											isSolvingProgress = true;
										}
									}
							}
							if (pos1 / 3 == pos2 / 3)
							{
								for (byte x = 0; x < 9; x++)
									if (!grid[i * 3 + (pos1 / 3), x].SolveResult.HasValue && x / 3 != j)
                                    {
										if (grid[i * 3 + (pos1 / 3), x].GetAvailableCandidate(num) != 0)
                                        {
											grid[i * 3 + (pos1 / 3), x].SetAvailableCandidate(num, 0);
											//grid[i * 3 + (pos1 / 3), x].RefreshCountAvailableCandidates();
											isSolvingProgress = true;
										}
									}
							}
						}
					}

					grid[i, j].CheckSingleCandidate();
				}
			}

			//Исключаем очевидно парных кандидатов строк из квадрата
			for (byte i = 0; i < 9; i++)
			{
				for (byte num = 0; num < 9; num++)
				{
					byte countNumbers = 0;
					byte pos1 = 0, pos2 = 0;

					for (byte k = 0; k < 9; k++)
					{
						if (!grid[i, k].SolveResult.HasValue
							&& grid[i, k].GetAvailableCandidate(num) != 0)
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
								if (!grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].SolveResult.HasValue && i % 3 != x / 3)
                                {
									if (grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].GetAvailableCandidate(num) != 0)
                                    {
										grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].SetAvailableCandidate(num, 0);
										grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].CheckSingleCandidate();
										//grid[i / 3 * 3 + x / 3, pos1 / 3 * 3 + x % 3].RefreshCountAvailableCandidates();
										isSolvingProgress = true;
									}
								}

						}
					}
				}
			}
			//Исключаем очевидно парных кандидатов столба из квадрата
			for (byte j = 0; j < 9; j++)
			{
				for (byte num = 0; num < 9; num++)
				{
					byte countNumbers = 0;
					byte pos1 = 0, pos2 = 0;

					for (byte k = 0; k < 9; k++)
					{
						if (!grid[k, j].SolveResult.HasValue
							&& grid[k, j].GetAvailableCandidate(num) != 0)
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
								if (!grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].SolveResult.HasValue && j % 3 != x % 3)
                                {
									if (grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].GetAvailableCandidate(num) != 0)
                                    {
										grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].SetAvailableCandidate(num, 0);
										grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].CheckSingleCandidate();
										//grid[pos1 / 3 * 3 + x / 3, j / 3 * 3 + x % 3].RefreshCountAvailableCandidates();
										isSolvingProgress = true;
									}
								}
									
						}
					}
				}
			}

			return isSolvingProgress;
		}

		public bool SolvingProcedure1(MySudokuCell[,] grid) // Очевидные и скрытые синглы
		{
			bool isSolvingProgress = false;

			Console.WriteLine("1 процедура - 1");
			for (byte i = 0; i < 9; i++)
			{
				for (byte j = 0; j < 9; j++)
				{

					//Убираем невозможных кандидатов с поля
					if (grid[i, j].SolveResult.HasValue)
					{
						//Проходим по строке
						for (byte k = 0; k < 9; k++)
						{
							if (k != j)
							{
								if (grid[i, k].GetAvailableCandidate((byte)(grid[i, j].SolveResult.Value - 1)) != 0)
                                {
									grid[i, k].SetAvailableCandidate((byte)(grid[i, j].SolveResult.Value - 1), 0);
									//grid[i, k].RefreshCountAvailableCandidates();
									isSolvingProgress = true;
								}
							}
						}

						//Проходим по столбцу
						for (byte k = 0; k < 9; k++)
						{
							if (k != i)
							{
								if (grid[k, j].GetAvailableCandidate((byte)(grid[i, j].SolveResult.Value - 1)) != 0)
                                {
									grid[k, j].SetAvailableCandidate((byte)(grid[i, j].SolveResult.Value - 1), 0);
									//grid[k, j].RefreshCountAvailableCandidates();
									isSolvingProgress = true;
								}
							}
						}

						//Проходим по квадрату
						for (byte k = 0; k < 9; k++)
						{
							if (i != (i / 3 * 3 + k / 3) || j != (j / 3 * 3 + k % 3))
							{
								if (grid[i / 3 * 3 + k / 3, j / 3 * 3 + k % 3].GetAvailableCandidate((byte)(grid[i, j].SolveResult.Value - 1)) != 0)
                                {
									grid[i / 3 * 3 + k / 3, j / 3 * 3 + k % 3].SetAvailableCandidate((byte)(grid[i, j].SolveResult.Value - 1), 0);
									//grid[i / 3 * 3 + k / 3, j / 3 * 3 + k % 3].RefreshCountAvailableCandidates();
									isSolvingProgress = true;
								}
							}
						}
					}

					//Ставим числа в ячейках по методу исключения
					else
					{
						//Проходим по строке
						for (byte num = 0; num < 9; num++)
						{
							byte countNotAvailableCandidates = 0;

							for (byte k = 0; k < 9; k++)
							{
								if (k != j)
								{
									if (grid[i, k].GetAvailableCandidate(num) == 0) countNotAvailableCandidates++;

								}
							}
							if (countNotAvailableCandidates == 8)
							{
								grid[i, j].SolveResult = (byte)(num + 1);
								isSolvingProgress = true;
								break;
							}
						}

						//Проходим по столбцу
						for (byte num = 0; num < 9; num++)
						{
							byte countNotAvailableCandidates = 0;

							for (byte k = 0; k < 9; k++)
							{
								if (k != i)
								{
									if (grid[k, j].GetAvailableCandidate(num) == 0) countNotAvailableCandidates++;
								}
							}
							if (countNotAvailableCandidates == 8)
							{
								grid[i, j].SolveResult = (byte)(num + 1);
								isSolvingProgress = true;
								break;
							}
						}

						//Проходим по квадрату
						for (byte num = 0; num < 9; num++)
						{
							byte countNotAvailableCandidates = 0;
							for (byte k = 0; k < 9; k++)
							{
								if (i != (i / 3 * 3 + k / 3) || j != (j / 3 * 3 + k % 3))
								{
									if (grid[i / 3 * 3 + k / 3, j / 3 * 3 + k % 3].GetAvailableCandidate(num) == 0) countNotAvailableCandidates++;

									//grid[i/3*3 + k/3, j/3*3 + k%3].availableVariantes[grid[i,j].solve - 1] = 0;
								}
							}
							if (countNotAvailableCandidates == 8)
							{
								grid[i, j].SolveResult = (byte)(num + 1);
								isSolvingProgress = true;
								break;
							}
						}
					}

					grid[i, j].CheckSingleCandidate();
				}
			}

			return isSolvingProgress;
		}
	}
}
