using System;
using Newtonsoft.Json;
using System.Collections.Generic;

/**
 * @author Ramiro Arenivar
 * CSCI 5582 - Artificial Intelligence
 * Prof. Williams
 * ConnectFour program assignment
 * */
namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// Object which stores all the information related to the 
	/// Connect Four game, as well as several helper functions for
	/// our Alpha-Beta pruning algorithm
	/// </summary>
	[Serializable]
	public class GameState
	{
		public GameState() { }

		// Stores a list if list of integers representing the ConnectFour grid
		[JsonProperty("grid")]
		public List<List<int>> Grid { set; get; }
		// Represents the number of rows
		[JsonProperty("height")]
		public int Height { set; get; }
		// Represents the number of columns
		[JsonProperty("width")]
		public int Width { set; get; }
		// What player we are
		[JsonProperty("player")]
		public int Player { set; get; }
		// This will be the value for a winning state, as well as the losing state if
		// we just add the negative sign to it
		public int WinningScore { set; get; } = 99999999;

		/// <summary>
		/// Determines whether the Connect Four grid is full
		/// </summary>
		/// <returns><c>true</c> If the grid is full; otherwise, <c>false</c>.</returns>
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

		/// <summary>
		/// Helper function used in the maximize and minimize functions to halt the recursion if 
		/// we've reach our depth limit, the grid is full or there is a winning or losing state
		/// </summary>
		/// <returns><c>true</c> If any of the previuosly stated conditions are true; otherwise <c>false</c>.</returns>
		/// <param name="searchDepth">Search depth value</param>
		/// <param name="theScore">The current score of the grid</param>
		public bool IsCheckingDone(int searchDepth, int theScore) {
			if (searchDepth == 0 || theScore == this.WinningScore || 
					theScore == -this.WinningScore || this.IsGridFull ()) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// Helper function to make a move on the ConnectFour grid
		/// </summary>
		/// <returns><c>true</c> If we were able to make the move into the column passed, <c>false</c> otherwise.</returns>
		/// <param name="col">Col.</param>
		public bool InsertIntoGrid(int col) {
			if (col >= this.Width || this.IsGridFull() || !this.IsItValidMove(col)) {
				return false;
			}
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

		/// <summary>
		/// Determines whether a move to the column specified in the argument is valid
		/// </summary>
		/// <returns><c>true</c> If the move is valid, otherwise <c>false</c>.</returns>
		/// <param name="col">Col.</param>
		public bool IsItValidMove(int col) {
			bool valid = false;
			List<int> column = this.Grid [col];
			if (column.Contains (0)) {
				valid = true;
			}
			return valid;
		}

		/// <summary>
		/// Determines the 'value' of the current grid cell.
		/// To do this, we need to give it a row and column, along with a 
		/// direction to check. The directions can be: horizontal, vertical,
		/// right diagonal up and righ diagonal down. These are part of an enum
		/// in the GameUtilities static class
		/// </summary>
		/// <returns>An integer value, which represents how good we think this cell is
		/// in order to win the game</returns>
		/// <param name="row">Row number of cell</param>
		/// <param name="col">Column number of cell</param>
		/// <param name="direction">Direction to check</param>
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
					default:
						Console.Error.WriteLine ("Error in 'CellScore' function, invalid direction enum passed!");
						break;
				}
			}

			// We need to check if there someone can win, either our player or
			// the competitor, in which case we need to give our score the value
			// of the winning or losing score
			if (selfPlayerScore == 4) { return this.WinningScore; } 
			else if (competitorScore == 4) { return -this.WinningScore; } 
			else { return selfPlayerScore; }
		}

		/// <summary>
		/// Using our 'CellScore' helper function, we go through the entire
		/// ConnectFour grid to check the different values of its cell,
		/// then at the end we add them up and return the total value.
		/// It is important to note that if we see a win or losing state,
		/// we return immediantely the winning or losing value
		/// </summary>
		/// <returns>An integer representing the total value of the grid in 
		/// its current state</returns>
		public int CalculateScore() {
			int totalPoints = 0;
			int tempPoints = 0;
			// Checking the vertical winning connection of four
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
			// Checking the horizontal connect four
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
			// Checking the right diagonal up winning position
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
			// Checking the right diagonal down winning position
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
	} // End GameState class
}