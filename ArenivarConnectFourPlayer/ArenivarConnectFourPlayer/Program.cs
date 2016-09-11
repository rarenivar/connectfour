using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArenivarConnectFourPlayer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// we only need to check once if we're making the first move
			bool checkFirstMove = false;
			string gameStateData;
			/* TODO: only a temporary variable, remove after logic for
			 * the alpha-pruning algorithm is implemented!
			 * */
			Random rnd = new Random();
			int chosenCol;


			//Config.DepthSearch = 4;
			//int m = int.MaxValue;
			//int mi = int.MinValue;
			//Console.Error.WriteLine ("max value = {0}  ---   min value = {1} and deotp = {2}", m, mi, Config.DepthSearch);

			string json = @"{
			'grid': [
					[0,0,0,3,0,0],
					[0,0,0,3,0,0],
					[0,0,0,3,0,0],
					[0,0,0,3,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0]
					],
			'height':6,
			'player':2,
			'width':7
			}";
			GameState gm2 = JsonConvert.DeserializeObject<GameState> (json);
			Console.Error.WriteLine ("is it full? " + GameUtilities.IsGridFull(gm2));
			//GameState gm3 = GameUtilities.MakeMove (gm2, 0);
			bool gameover = GameUtilities.CheckGameOver (gm2);
			Console.Error.WriteLine ("is it full? " + GameUtilities.IsGridFull(gm2));
		/*
			Console.Error.WriteLine ("Arenivar player starting...");

			while ((gameStateData = Console.ReadLine ()) != null && gameStateData != "") {

				// getting the JSON data into our GameState object
				GameState gm = JsonConvert.DeserializeObject<GameState>(gameStateData);

				// checking if we're making the first move, we only need to do this once
				if (!checkFirstMove) {
					checkFirstMove = true;
					if (GameUtilities.IsItTheFirstMove (gm)) {
						Console.Error.WriteLine ("Making first move");
					} else {
						Console.Error.WriteLine ("Making second move");
					}
				}
				bool isFull = GameUtilities.IsGridFull (gm);
				Console.Error.WriteLine ("is it full? " + isFull);

				// for now, we're choosing the column randomly
				do {
					chosenCol = rnd.Next(0, ( gm.Width -1 ));
				} // do it again if the column is full!
				while (!gm.Grid[chosenCol].Contains(0));

				// sending data to standard error and output
				Console.Error.WriteLine(JsonConvert.SerializeObject(gm));
				Console.Error.WriteLine ("{\"move\":" + chosenCol + "}");
				Console.Out.WriteLine("{\"move\":" + chosenCol + "}");
				Console.Out.Flush ();
			}*/
		}
	}
}
