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

		/// <param name="colNumber">Col number.</param>
		public void MakeMove(int colNumber)
		{
			if (colNumber >= this.Width || GameUtilities.IsGridFull(this)) 
			{
				Console.Error.WriteLine ("MakeMove function error: column passed to function not valid or grid is full");
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
	}
}

