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
			// vertical win
			if (row < (gm.Height - 3)) {
				
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
			if (colNumber > gm.Width || IsGridFull(gm)) 
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

		/*
		public static int score(GameState gm){
			int score = 0;
			for (int r= 0; r < gm.Height; r++) 
			{
				if (r <= gm.Height-4) 
				{
					for (int c = 0; c < gm.Width; c++) 
					{
						score += score2(r, c, gm);
					}
				}
				else 
				{
					for (int c = 0; c <= gm.Width-4; c++) 
					{
						score += score2(r, c, gm);
					}
				}
			}

			return score;
		}
*/
		/**
 		* Helper method to get the score of a board
 		*/
		/*
		public static int score2(int row, int col, GameState gm){
			Console.Error.WriteLine ("in the score2 fuanction");
			int score = 0;
			bool unblocked = true;
			int tally = 0;
			//int r, c;
			if (row < gm.Height-4) {
				//check up
				unblocked = true;
				tally = 0;
				for (int r=row; r<row+4; r++) {

					if (gm.Grid[r][col] == gm.Player) {
						unblocked = false;
					}
					if (gm.Grid[r][col] == gm.Player) {
						tally ++;
					}
				}
				if (unblocked == true) {
					score = score + (tally*tally*tally*tally);
				}

				if (col < gm.Width-4) {
					//check up and to the right
					unblocked = true;
					tally = 0;
					for (int r=row, c=col; r<row+4; r++, c++) {
						if (gm.Grid[r][c] == gm.Player) {
							unblocked = false;
						}
						if (gm.Grid[r][c] == gm.Player) {
							tally ++;
						}
					}
					if (unblocked == true) {
						score = score + (tally*tally*tally*tally);
					}
				}
			}
			if (col < gm.Width-4) {
				//check right
				unblocked = true;
				tally = 0;
				for (int c=col; c<col+4; c++) {
					if (gm.Grid[row][c] == gm.Player) {
						unblocked = false;
					}
					if (gm.Grid[row][c] == gm.Player) {
						tally ++;
					}
				}
				if (unblocked == true) {
					score = score + (tally*tally*tally*tally);
				}

				if (row > 2) {
					//check down and to the right
					unblocked = true;
					tally = 0;
					for (int r=row, c=col; c<col+4; r--, c++) {
						if (gm.Grid[r][c] == gm.Player) {
							unblocked = false;
						}
						if (gm.Grid[r][c] == gm.Player) {
							tally ++;
						}
					}
					if (unblocked == true) {
						score = score + (tally*tally*tally*tally);
					}
				}
			}

			return score;

		}*/
	}
}