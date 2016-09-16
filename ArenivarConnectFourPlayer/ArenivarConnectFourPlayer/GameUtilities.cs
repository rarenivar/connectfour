using System;
using System.Collections.Generic;

namespace ArenivarConnectFourPlayer
{
	public static class GameUtilities
	{
		
		/// <summary>
		/// Determines if the player will make the first move
		/// </summary>
		/// <returns><c>true</c> if is it the first move the specified gm; otherwise, <c>false</c>.</returns>
		/// <param name="gm">GameState object</param>
		public static bool IsItTheFirstMove(GameState gm) {	
			// assume we're making the move unless proven otherwise
			bool firstPlayer = true;
			// checking if the grid is all 0s
			foreach (List<int> column in gm.Grid) {
				// only need to check the bottom row to check for an empty grid
				// since the driver will not allow invalid moves!
				if (column [gm.Height - 1] != 0) { firstPlayer = false; }
			}
			return firstPlayer;
		}

		public static bool IsGameOver(GameState gm) {
			if (IsGridFull (gm)) { return true; }
			return false;
		}

		public static bool CheckGameOver(GameState gm) {
			bool gameWon = false;
			for (int i = 0; i < gm.Width && gameWon == false; i++) {
				for (int j = 0; j < gm.Height && gameWon == false; j++) {
					gameWon = CheckCellForConnectFour (gm, i, j);
				}
			}
			return gameWon;
		}

		/// <summary>
		/// Checks to see if there is a game winner
		/// </summary>
		/// <returns><c>true</c> if is connect four the specified gm; otherwise, <c>false</c>.</returns>
		/// <param name="gm">Gm.</param>
		public static bool CheckCellForConnectFour (GameState gm, int col, int row) {
			bool gameWon = false;
			if (row < (gm.Height - 3)) {
				// vertical win
				gameWon = true;
				for (int i = row; i < (row + 4); i++) {
					if (gm.Grid [col] [i] != gm.Player) {
						gameWon = false;
						break;
					}
				}
				// right diagonal win
				if (col < (gm.Width - 3) && gameWon == false) { 
					gameWon = true; 
					for (int i = row, j = col; i < (row + 4); i++, j++) {
						if (gm.Grid [j] [i] != gm.Player) {
							gameWon = false;
							break;
						}
					}
				}
				// left diagonal
				if (col > 2 && gameWon == false) { 
					gameWon = true; 
					for (int i = row, j = col; i < (row + 4); i++, j--) {
						if (gm.Grid [j] [i] != gm.Player) {
							gameWon = false;
							break;
						}
					}
				}
			}
			// horizontal win
			if (col < (gm.Width - 3) && gameWon == false) {
				gameWon = true;
				for (int j = col; j < (col + 4); j++) {
					if (gm.Grid [j] [row] != gm.Player) {
						gameWon = false;
						break;
					}
				}
			}
			return gameWon;
		}

		public static bool IsGridFull(GameState gm) {
			bool isFull = true;
			foreach (List<int> column in gm.Grid) {
				foreach (int cell in column) {
					if (cell == 0) { isFull = false; }
				}
			}
			return isFull;
		}

		/// <summary>
		/// Returns a new GameState object with the new move, if the move is not possible
		/// returns null
		/// </summary>
		/// <returns>GameState object with the new moved</returns>
		/// <param name="gm">Gm.</param>
		/// <param name="colNumber">Col number.</param>
		public static GameState MakeMove(GameState gm, int colNumber)
		{
			if (colNumber >= gm.Width || IsGridFull(gm)) 
			{
				Console.Error.WriteLine ("MakeMove function error: column passed to function not valid or grid is full");
				return null;
			}
			for (int i = gm.Height-1; i >= 0; i--) 
			{
				if (gm.Grid [colNumber] [i] == 0) 
				{
					gm.Grid [colNumber] [i] = gm.Player;
					return gm;
				}
			}
			Console.Error.WriteLine ("MakeMove function error: column {0} is full, cannot make move", colNumber);
			return null;

		}

		public static int calculateScore(int totalScore, int value) {
			int theTotalScore = totalScore;
			if (value > 2) {
				theTotalScore = totalScore + (int)Math.Pow (value, 3);
			} else if (value == 2) {
				theTotalScore = totalScore + (int)Math.Pow (value, 2);
			} else if (value == 1) {
				theTotalScore = totalScore + value;
			}
			return theTotalScore;
		}

		/// <summary>
		/// Gets the grid score.
		/// </summary>
		/// <returns>The grid score.</returns>
		/// <param name="gm">Gm.</param>
		public static int getGridScore(GameState gm) {
			int totalValue = 0;
			for (int j = 0; j < (gm.Width - 3); j++) {
				for (int i = (gm.Height - 1); i >= 0; i--) {
					totalValue += GameUtilities.getCellScore (gm, j, i);
				}
			}
			return totalValue;
		}

		/// <summary>
		/// Goes through the board to see how many ways there are to win
		/// </summary>
		/// <returns>The board value.</returns>
		/// <param name="gm">Gm.</param>
		public static int getCellScore(GameState gm, int col, int row) {

			// players
			int currentPlayer = (gm.Player == 1) ? 1 : 2;
			int competitor = (currentPlayer == 1) ? 2 : 1;

			bool isItWinnable = true;
			// keeping track of the cell value
			int totalScore = 0;
			int value = 0;

			if (col < (gm.Width - 3)) {
				// check horizontal connect four
				for (int j = col; j < (col + 4); j++) {
					if (gm.Grid [j] [row] == currentPlayer) {
						value++;
					}
					if (gm.Grid [j] [row] == competitor) {
						isItWinnable = false;
						break;
					}
				} // end check horizontal connect four
				if (isItWinnable) {
					totalScore = calculateScore (totalScore, value);
				}
				// check connect four down to the right diagonally
				if (row  < (gm.Height - 3)) {
					isItWinnable = true;
					value = 0;
					for (int j = col, i = row; j < (col + 4); j++, i++) {
						if (gm.Grid [j] [i] == currentPlayer) {
							value++;
						}
						if (gm.Grid [j] [i] == competitor) {
							isItWinnable = false;
							break;
						}
					}
					if (isItWinnable) {
						totalScore = calculateScore (totalScore, value);
					}
				}
			} // end col < (gm.Width -3)
				
			if (row >= 3) {
				isItWinnable = true;
				value = 0;
				// check cells above
				for (int i = row; i > (row - 4); i--) {
					if (gm.Grid [col] [i] == currentPlayer) {
						value++;
					}
					// check if the competitor is on top of the cell
					if (gm.Grid [col] [i] == competitor) {
						isItWinnable = false;
						break;
					}
				} // end checks cells above
				if (isItWinnable) {
					totalScore = calculateScore (totalScore, value);
				}
				// check cells to the right diagonally
				if (col < (gm.Width - 3)) {
					isItWinnable = true;
					value = 0;

					for (int i = row, j = col; i > (row - 4); i--, j++) {
						if (gm.Grid [j] [i] == currentPlayer) {
							value++;
						}
						if (gm.Grid [j] [i] == competitor) {
							isItWinnable = false;
							break;
						}
					}
					if (isItWinnable) {
						totalScore = calculateScore (totalScore, value);
					}
				} // end check cells to the right diagonally
			} // end row >= 3
			return totalScore;
		} // end getCellScore function

	}
}