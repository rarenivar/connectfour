using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArenivarConnectFourPlayer
{
	/// <summary>
	/// Main class for Connect Four player
	/// </summary>
	class MainClass
	{
		public static void Main (string[] args)
		{
			// We'll store JSON data in this string before we can deserialize it into an object
			string gameStateData;
			Console.Error.WriteLine ("Arenivar player starting...");

			while ((gameStateData = Console.ReadLine ()) != null && gameStateData != "") {
				// Storing the JSON data from the driver into a GameState object so we can process it
				GameState gm = JsonConvert.DeserializeObject<GameState>(gameStateData);
				// We need to keep track of what player number we are
				GameUtilities.selfPlayerNum = gm.Player;
				// Calling the MaximizeValue function, which will return a Move object that contains
				// the best move according to our algorithm
				Move bestMove = GameUtilities.MaximizeValue (gm, GameUtilities.SearchDepth, 
									GameUtilities.AlphaInitialValue, GameUtilities.BetaInitialValue);

				// For debugging, print the JSON object received, and the move we'll be
				// sending to the driver program
				Console.Error.WriteLine(JsonConvert.SerializeObject(gm));
				Console.Error.WriteLine ("{\"move\":" + bestMove.ColumnToMoveTo + "}");
				// Sending the best move according to our algorithm to the driver program
				Console.Out.WriteLine("{\"move\":" + bestMove.ColumnToMoveTo + "}");
				Console.Out.Flush ();
			}
		}
	}
}
