using System;

namespace carl
{
	public interface IGameEngine
	{
		void EndGame();
      void SetStatus(string format, params object[] args);
		Tile[,] GetCurrentMap();
	}

	public class GameEngine : IGameEngine
	{
		public Player Player;
		public CommandProcessor CommandProcessor;
		public string CommandLine;
		public Tile[,] Map;

		public bool IsActive;

		public string StatusLine;
		public int StatusTtl;

		public void EndGame()
		{
			IsActive = false;
		}

		public void SetStatus(string format, params object[] args)
		{
			StatusLine = string.Format(format, args);
			StatusTtl = 60;
		}

		public Tile[,] GetCurrentMap()
		{
			return Map;
		}
	}
}