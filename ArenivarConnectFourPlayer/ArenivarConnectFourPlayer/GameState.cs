using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// Object which stores the game state data sent by the
	/// driver program
	/// </summary>
	public class GameState
	{
		public GameState () { }

		[JsonProperty("grid")]
		public List<List<int>> Grid { set; get; }
		[JsonProperty("height")]
		public int Height { set; get; }
		[JsonProperty("width")]
		public int Width { set; get; }
		[JsonProperty("player")]
		public int Player { set; get; }
	}
}

