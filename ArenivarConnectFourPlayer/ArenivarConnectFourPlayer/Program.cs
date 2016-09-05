using System;
using Newtonsoft.Json;

namespace ArenivarConnectFourPlayer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/*
			 //////////
			string jsonstring = "";
			string line;
			/////////
			*/

/*
			string json = @"{
			'grid': [
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,2],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0],
					[0,0,0,0,0,0]
					],
			'height':6,
			'player':1,
			'width':7
			}";
*/

			//GameState gs = JsonConvert.DeserializeObject<GameState>(json);
			//Console.Out.WriteLine("the height is " + gs.Height);
			//Console.Out.WriteLine("the width is " + gs.Width);
			//Console.Out.WriteLine("the player is " + gs.Player);
			//Console.Out.WriteLine("the column 4th column has a value of " + gs.Grid[3][5]);


			Console.Error.WriteLine ("Arenivar player...");
			string gameStateData;
			Random rnd = new Random();
			int chosenCol;
			while ((gameStateData = Console.ReadLine ()) != null && gameStateData != "") {
				GameState gm = JsonConvert.DeserializeObject<GameState>(gameStateData);

				do {
					chosenCol = rnd.Next(0, ( gm.Width -1 ));
				}
				while (!gm.Grid[chosenCol].Contains(0));

				Console.Error.WriteLine(JsonConvert.SerializeObject(gm));
				Console.Error.WriteLine ("{\"move\":" + chosenCol + "}");
				Console.Out.WriteLine("{\"move\":" + chosenCol + "}");
				Console.Out.Flush ();
				
			}








			/*line = Console.ReadLine ();
			jsonstring = line;
			Console.Error.WriteLine (jsonstring);
			//string theOutput = @"{'move':3}";
			//Console.Out.WriteLine(JsonConvert.Se

			Console.Out.WriteLine("{\"move\":3}");
			Console.Out.Flush ();
*/


			/*
			/////////
			while ((line = Console.ReadLine ()) != null && line != "") {
				//jsonstring = line;
				//GameState game = JsonConvert.DeserializeObject<GameState> (jsonstring);
				//Console.Error.WriteLine (jsonstring);
				Console.Out.WriteLine("{\"move\":3}");
				Console.Out.Flush ();

			}
			////////////
			*/


			//Console.Error.WriteLine ("done");
			//Console.Out.WriteLine ("Hello World!");
			//Console.Error.WriteLine (jsonstring);
			//Console.Out.Flush ();

		}
	}
}
