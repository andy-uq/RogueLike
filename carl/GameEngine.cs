using System;

namespace carl
{
	public interface IGameEngine
	{
		void EndGame();
      void SetStatus(string format, params object[] args);
		Map Map { get; }
	}

	public class GameEngine : IGameEngine
	{
		public Player Player;
		public CommandProcessor CommandProcessor;
		public string CommandLine { get; set; }
		public Map Map { get; set; }

		public bool IsActive;

		public string StatusLine;
		public int StatusTtl;

		public GameEngine()
		{
			CommandLine = string.Empty;
		}

		public void EndGame()
		{
			IsActive = false;
		}

		public void SetStatus(string format, params object[] args)
		{
			StatusLine = string.Format(format, args);
			StatusTtl = 60;
		}
	}
}