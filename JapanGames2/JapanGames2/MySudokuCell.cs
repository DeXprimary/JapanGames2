using System;
using System.Collections.Generic;
using System.Text;

namespace JapanGames2
{
	public class MySudokuCell : ICloneable
	{
		private byte countAvailableCandidates = 9;
		public byte CountAvailableCandidates
		{
			get => countAvailableCandidates;
		}

		private byte[] availableCandidates = new byte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		/*
		// Судя по всему при обращении к элементу массива Сеттер не задействовался
		// Решил реализовать геттер и сеттер через методы
		public byte[] AvailableCandidates
		{ 
			get => availableCandidates;

			set 
			{
					availableCandidates = value;
					RefreshCountAvailableCandidates();
			} 
		}
		*/

		private byte? solveResult = null;
		public byte? SolveResult
		{
			get => solveResult;

			// Пока отдельно не выделил поле solveResult логер андройда кидал почему-то:
			// [libc] Fatal signal 11 (SIGSEGV), code 2 (SEGV_ACCERR), fault addr 0x7ffee4938ff0 in tid 18384 (ame.japangames2), pid 18384 (ame.japangames2)

			set
			{
				if (value == 0 || !value.HasValue)
                {
					solveResult = null;
					ReplaceCandidates();
				}
				else if (value >= 1 && value <= 9)
                {
					solveResult = value;
					ClearCandidates(solveResult.Value);
				}
			}
		}

		public MySudokuCell(byte? digit = null)
		{
			if (digit != 0) solveResult = digit;
			else solveResult = null;
			if (solveResult.HasValue) ClearCandidates(solveResult.Value);
		}

		//Реализация метода копирования ячейки (Сохранение копии для метода подстановки)
		public object Clone()
		{
			return new MySudokuCell
			{
				countAvailableCandidates = this.countAvailableCandidates,
				availableCandidates = (byte[])this.availableCandidates.Clone(),
				solveResult = this.solveResult
			};
		}

		//Обновление числа кандидатов ячейки
		private void RefreshCountAvailableCandidates()
		{
			byte counter = 0;
			for (byte k = 0; k < 9; k++) if (availableCandidates[k] != 0) counter++;
			countAvailableCandidates = counter;
		}

		//Проверить конкретный кандидат
		public byte GetAvailableCandidate(byte index)
		{
			return availableCandidates[index];
		}

		//Установить конкретный кандидат
		public void SetAvailableCandidate(byte index, byte value)
		{
			availableCandidates[index] = value;
			RefreshCountAvailableCandidates();
			CheckSingleCandidate();
		}

		public void SetAvailableCandidateLiteForProc2(byte index, byte value)
		{
			availableCandidates[index] = value;
			RefreshCountAvailableCandidates();
			//CheckSingleCandidate(); - ломает работу 2 процедуры решения
		}

		//Проверка на единственного кандидата
		public void CheckSingleCandidate()
		{
			byte counterCandidates = 0;
			byte lastCandidate = 0;
			foreach (var digit in availableCandidates)

				if (digit != 0)
				{
					counterCandidates++;
					lastCandidate = digit;
				}

			if (counterCandidates == 1)
			{
				solveResult = lastCandidate;
				if (solveResult.HasValue) ClearCandidates(solveResult.Value);
			}
		}


		private void ReplaceCandidates()
		{
			for (byte i = 0; i < 9; i++)
			{
				availableCandidates[i] = (byte)(i + 1);
			}
			RefreshCountAvailableCandidates();
		}

		private void ClearCandidates(byte digit)
		{
			for (byte i = 0; i < 9; i++)
			{
				if (i != digit - 1) availableCandidates[i] = 0;

				else availableCandidates[i] = digit;
			}
			RefreshCountAvailableCandidates();
		}
	}
}
