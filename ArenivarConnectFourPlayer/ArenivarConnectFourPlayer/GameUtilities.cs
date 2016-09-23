using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/**
 * @author Ramiro Arenivar
 * CSCI 5582 - Artificial Intelligence
 * Prof. Williams
 * ConnectFour program assignment
 * */
namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// A static class with the maximizer and minimizer functions as well as other
	/// helper functions
	/// </summary>
	public static class GameUtilities
	{
		// Need to keep reference of the player number given by the driver program
		public static int selfPlayerNum { set; get; }

		// At the beginning of the Alpha-Beta algoritm, Alpha is negative infinity
		// and Beta is positive infinity. For our purposes, we will represent these
		// values by the minimum and maximum values of the int class
		public const int AlphaInitialValue = int.MinValue;
		public const int BetaInitialValue = int.MaxValue;

		// How many levels down the tree we will go down. The algorithm will make 'better'
		// moves the higher this number gets, but the higher the number the more processing
		// time it takes to calculate a move
		public const int SearchDepth = 1;

		// Enum use to differenciate the different ways we need to check each grid cell
		// to figure out its value
		public enum Direction { Vertical, Horizontal, RightDiagonalUp, RightDiagonalDown }
	
		/// <summary>
		/// Helper function use to create a 'deep' copy of an object.
		/// This method is use in the maximize and minimize functions when
		/// we're evaluating each move
		/// </summary>
		/// <returns>A copy of the object passed</returns>
		/// <param name="obj">Object to copy</param>
		/// <typeparam name="T">Generic object type</typeparam>
		public static T DeepCopy<T>(T obj)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, obj);
				stream.Position = 0;
				return (T)formatter.Deserialize(stream);
			}
		}

		/// <summary>
		/// Function to maximize the current values in the subtree
		/// </summary>
		/// <returns>Highest value in the subtree</returns>
		/// <param name="gm">Contains the state of the game</param>
		/// <param name="searchDepth">How many levels to look down</param>
		/// <param name="alpha">Alpha value</param>
		/// <param name="beta">Beta value</param>
		public static Move MaximizeValue(GameState gm, int searchDepth, int alpha, int beta){
			Move maxMove;
			// Keeps track of how valuable this grib state is
			int score = gm.CalculateScore ();
			// If the grid is full, we have a winning score or we've reach our depth limit, 
			//return a this move
			if (gm.IsCheckingDone (searchDepth, score)) {
				return new Move (-1, score);
			}
			// Initiliazing our max move to negative infinity, anything is better than
			// this move
			maxMove = new Move (-1, int.MinValue);
			// Need to go through all columns
			for (int col = 0; col < gm.Width; col++) {
				GameState tempgm = GameUtilities.DeepCopy<GameState> (gm);
				// If we can insert into this column, we'll call the minimize function and 
				// update the alpha and maxMove if needed
				if (tempgm.InsertIntoGrid (col)) {
					Move tempMove = MinimizeValue (tempgm, (searchDepth - 1), alpha, beta);
					if (tempMove.MoveValue > maxMove.MoveValue || 
															maxMove.ColumnToMoveTo == -1) {
						alpha = tempMove.MoveValue;
						maxMove.ColumnToMoveTo = col;
						maxMove.MoveValue = tempMove.MoveValue;
					}
					if (alpha >= beta) { return maxMove; }
				}
			}
			return maxMove;
		}

		/// <summary>
		/// Function to minimize the values of the subtree
		/// </summary>
		/// <returns>Minimal value in the subtree</returns>
		/// <param name="gm">Contains the state of the game</param>
		/// <param name="searchDepth">How many levels to look down</param>
		/// <param name="alpha">Alpha value</param>
		/// <param name="beta">Beta value</param>
		public static Move MinimizeValue(GameState gm, int searchDepth, 
																int alpha, int beta) {
			Move minMove;
			// Keeps track of how valuable this grib state is
			int score = gm.CalculateScore ();
			// If the grid is full, we have a winning score or we've reach our depth limit, 
			// return a this move
			if (gm.IsCheckingDone(searchDepth, score)) {
				return new Move(-1, score);
			}
			// Initiliazing our min move to positive infinity, anything is better than
			// this move 
			minMove = new Move (-1, int.MaxValue);
			// Need to go through all columns
			for (int col = 0; col < gm.Width; col++) {
				GameState tempgm = GameUtilities.DeepCopy<GameState> (gm);
				// If we can insert into this column, we'll call the maximize function and 
				// update the beta and minMove if needed
				if (tempgm.InsertIntoGrid (col)) {
					Move tempMove = MaximizeValue (tempgm, (searchDepth - 1), alpha, beta);
					if (tempMove.MoveValue < minMove.MoveValue || 
															minMove.ColumnToMoveTo == -1) {
						beta = tempMove.MoveValue;
						minMove.ColumnToMoveTo = col;
						minMove.MoveValue = tempMove.MoveValue;
					}
					if (alpha >= beta) { return minMove; }
				}
			}
			return minMove;
		}

	} // End GameUtilities static class
}