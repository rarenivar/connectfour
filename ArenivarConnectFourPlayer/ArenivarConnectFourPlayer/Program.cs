using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArenivarConnectFourPlayer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string gameStateData;
			string json = @"{
			'grid': [
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0]
					],
			'height':6,
			'player':2,
			'width':7
			}";


			/*
			GameState gm2 = JsonConvert.DeserializeObject<GameState> (json);
			GameUtilities.selfPlayerNum = gm2.Player;
			int v = gm2.CalculateScore ();
			int gridvalue = GameUtilities.getGridScore (gm2);
			GameState gm3 = GameUtilities.DeepCopy<GameState> (gm2);//new GameState (gm2.Grid, gm2.Height, gm2.Width, gm2.Player);
			Move m = GameUtilities.MaximizeValue(gm2, Config.SearchDepth, GameUtilities.AlphaInitialValue, GameUtilities.BetaInitialValue);
			bool full = gm2.IsGridFull();
			Move g = GameUtilities.calculateMove(int.MinValue, int.MaxValue, Config.SearchDepth, gm3);
			*/


		///*
			Console.Error.WriteLine ("Arenivar player starting...");

			while ((gameStateData = Console.ReadLine ()) != null && gameStateData != "") {
				
				// getting the JSON data into our GameState object
				GameState gm = JsonConvert.DeserializeObject<GameState>(gameStateData);
				GameUtilities.selfPlayerNum = gm.Player;
				Console.Error.WriteLine ("Arenivar player is player ...{0}", gm.Player);


				////////
				// checking if we're making the first move, we only need to do this once
				//if (!checkFirstMove) {
				//	checkFirstMove = true;
				//	if (GameUtilities.IsItTheFirstMove (gm)) {
				//		Console.Error.WriteLine ("Making first move");
				//	} else {
				//		Console.Error.WriteLine ("Making second move");
				//	}
				//}
				//bool isFull = GameUtilities.IsGridFull (gm);
				//Console.Error.WriteLine ("is it full? " + isFull);

				// for now, we're choosing the column randomly
				//do {
				//	chosenCol = rnd.Next(0, ( gm.Width -1 ));
				//} // do it again if the column is full!
				//while (!gm.Grid[chosenCol].Contains(0));
				/////////


				Move g = GameUtilities.MaximizeValue (gm, Config.SearchDepth, GameUtilities.AlphaInitialValue, GameUtilities.BetaInitialValue);
				// sending data to standard error and output
				Console.Error.WriteLine(JsonConvert.SerializeObject(gm));
				Console.Error.WriteLine ("{\"move\":" + g.ColumnToMoveTo + "}");
				Console.Out.WriteLine("{\"move\":" + g.ColumnToMoveTo + "}");
				Console.Out.Flush ();
		
			}  //*/
		}
	}
}
