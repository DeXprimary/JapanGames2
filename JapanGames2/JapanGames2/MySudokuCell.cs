using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JapanGames2
{
	public class MySudokuCell : Label, ICloneable

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

			// Пока не выделил поле solveResult логер андройда кидал почему-то:
			// [libc] Fatal signal 11 (SIGSEGV), code 2 (SEGV_ACCERR), fault addr 0x7ffee4938ff0 in tid 18384 (ame.japangames2), pid 18384 (ame.japangames2)

			set
			{
				solveResult = value;
				if (value.HasValue) ClearCandidates(SolveResult.Value);
				else ReplaceCandidates();
			} 
		}

		public MySudokuCell(byte? digit = null) : base()
		{
			solveResult = digit;
			if (solveResult.HasValue) ClearCandidates(solveResult.Value);
		}
		
		public object Clone()
        {
			return new MySudokuCell 
			{ 
				countAvailableCandidates = this.countAvailableCandidates,
				availableCandidates = (byte[])this.availableCandidates.Clone(),
				solveResult = this.solveResult
			};
        }
		
		public void RefreshCountAvailableCandidates()
		{
			byte counter = 0;
			for (byte k = 0; k < 9; k++) if (availableCandidates[k] != 0) counter++;
			countAvailableCandidates = counter;
		}

		public byte GetAvailableCandidate (byte index)
        {
			return availableCandidates[index];
        }

		public void SetAvailableCandidate(byte index, byte value)
        {
			availableCandidates[index] = value;
			RefreshCountAvailableCandidates();
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
				if (i != digit - 1)
					availableCandidates[i] = 0;
			}
			RefreshCountAvailableCandidates();
		}

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
	}
	
}
