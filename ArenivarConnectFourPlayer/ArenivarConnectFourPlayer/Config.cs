using System;

namespace ArenivarConnectFourPlayer
{
	public static class Config
	{
		public const int SearchDepth = 3;
		public static int SelfPlayerNum;
		public const int AlphaInitialValue = int.MaxValue;
		public const int BetaInitialValue = int.MinValue;
	}
}

