using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// Object which stores the game state data sent by the
	/// driver program
	/// </summary>
	[Serializable]
	public class GameState
	{
		public GameState() { }

		[JsonProperty("grid")]
		public List<List<int>> Grid { set; get; }
		[JsonProperty("height")]
		public int Height { set; get; }
		[JsonProperty("width")]
		public int Width { set; get; }
		[JsonProperty("player")]
		public int Player { set; get; }
		public int GridScore { set; get; }
		public int WinningScore { set; get; } = 99999999; // if we have a winning move, this this the score of the board

		/// <param name="colNumber">Col number.</param>
		public void MakeMove(int colNumber)
		{
			if (colNumber >= this.Width || GameUtilities.IsGridFull(this)) 
			{
				Console.Error.WriteLine ("MakeMove function error: column passed to function not valid or grid is full, column: {0}", colNumber);
				return;
			}
			for (int i = this.Height-1; i >= 0; i--) 
			{
				if (this.Grid [colNumber] [i] == 0) 
				{
					this.Grid [colNumber] [i] = this.Player;
					this.Player = (this.Player == 1) ? 2 : 1;
					break;
				}
			}
		}

		public bool IsGridFull() {
			bool isFull = true;
			foreach (List<int> column in this.Grid) {
				foreach (int cell in column) {
					if (cell == 0) { isFull = false; break; }
				}
				if (!isFull) { break; }
			}
			return isFull;
		}

		public bool IsCheckingDone(int searchDepth, int theScore) {
			if (searchDepth == 0 || theScore == this.GridScore || theScore == -this.GridScore || this.IsGridFull ()) {
				return true;
			}
			return false;
		}

		public bool InsertIntoGrid(int col) {
			if (col >= this.Width || this.IsGridFull() || !this.IsItValidMove(col)) 
			{
				Console.Error.WriteLine ("MakeMove function error: column passed to function not valid or grid is full, col number: {0}", col);
				return false;
			}
			/*if (col >= this.Width) 
			{
				Console.Error.WriteLine ("column {0} is greater than width", col);
				return false;
			}
			if (!this.IsItValidMove(col)) 
			{
				Console.Error.WriteLine ("not valid move");
				return false;
			}
			if (this.IsGridFull()) 
			{
				Console.Error.WriteLine ("grid is full");
				return false;
			}*/
			for (int i = this.Height-1; i >= 0; i--) 
			{
				if (this.Grid[col] [i] == 0) 
				{
					this.Grid [col] [i] = this.Player;
					this.Player = (this.Player == 1) ? 2 : 1;
					return true;
				}
			}
			return false;
		}

		public bool IsItValidMove(int col) {
			bool valid = false;
			List<int> column = this.Grid [col];
			if (column.Contains (0)) {
				valid = true;
			}
			return valid;
		}

		public int cellScore(int row, int col, GameUtilities.Direction direction) {
			int competitorPlayerNum = (GameUtilities.selfPlayerNum == 1) ? 2 : 1;
			int competitorScore = 0;
			int selfPlayerScore = 0;


			for (int i = 0; i < 4; i++) {
				if (this.Grid [col] [row] == GameUtilities.selfPlayerNum) {
					selfPlayerScore++;
				} else if (this.Grid [col] [row] == competitorPlayerNum) {
					competitorScore++;
				}

				switch (direction) {
				case (GameUtilities.Direction.Horizontal):
					col += 1;
					break;
				case (GameUtilities.Direction.Vertical):
					row -= 1;
					break;
				case (GameUtilities.Direction.RightDiagonalUp):
					col += 1;
					row -= 1;
					break;
				case (GameUtilities.Direction.RightDiagonalDown):
					col += 1;
					row += 1;
					break;
				}
			}

			if (selfPlayerScore == 4) {
				return this.WinningScore;
			} else if (competitorScore == 4) {
				return -this.WinningScore;
			} else {
				return selfPlayerScore;
			}
		}

		public int CalculateScore() {
			int totalPoints = 0;
			int tempPoints = 0;

			// checking up
			for (int row = (this.Height -1); row > 2; row--) {
				for (int col = 0; col < this.Width; col++) {
					tempPoints = this.cellScore (row, col, GameUtilities.Direction.Vertical);
					if (tempPoints == this.WinningScore) {
						return this.WinningScore;
					} else if (tempPoints == -this.WinningScore) {
						return -this.WinningScore;
					}
					totalPoints += tempPoints;
				}
			}

			// checking horizontally
			for (int col = 0; col < (this.Width - 3); col++) {
				for (int row = (this.Height - 1); row > -1; row--) {
					tempPoints = this.cellScore(row, col, GameUtilities.Direction.Horizontal);
					if (tempPoints == this.WinningScore) {
						return this.WinningScore;
					} else if (tempPoints == -this.WinningScore) {
						return -this.WinningScore;
					}
					totalPoints += tempPoints;
				}
			}

			// checking right diagonal up
			for (int row = (this.Height -1); row > 2; row--) {
				for (int col = 0; col < (this.Width - 3); col++) {
					tempPoints = this.cellScore (row, col, GameUtilities.Direction.RightDiagonalUp);
					if (tempPoints == this.WinningScore) {
						return this.WinningScore;
					} else if (tempPoints == -this.WinningScore) {
						return -this.WinningScore;
					}
					totalPoints += tempPoints;
				}
			}

			// checking right diagonal down
			for (int row = 0; row < (this.Height - 3); row++) {
				for (int col = 0; col < (this.Width - 3); col++) {
					tempPoints = this.cellScore (row, col, GameUtilities.Direction.RightDiagonalDown);
					if (tempPoints == this.WinningScore) {
						return this.WinningScore;
					} else if (tempPoints == -this.WinningScore) {
						return -this.WinningScore;
					}
					totalPoints += tempPoints;
				}
			}

			return totalPoints;
		}
	}
}

