﻿using System;

/**
 * @author Ramiro Arenivar
 * CSCI 5582 - Artificial Intelligence
 * Prof. Williams
 * ConnectFour program assignment
 * */
namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// Object that contains two properties, a move and its corresponding value for
	/// the Alpha-Beta prunning algorithm
	/// </summary>
	public class Move
	{
		public Move (int move, int value)
		{
			ColumnToMoveTo = move;
			MoveValue = value;
		}

		public int ColumnToMoveTo { get; set; }
		public int MoveValue { get; set; }
	}
}