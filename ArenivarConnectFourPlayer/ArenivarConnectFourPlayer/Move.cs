using System;

namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// Object that contains two properties,  a move and its corresponding value for
	/// the Alpha-Beta prunning algorithm
	/// algorithm
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

