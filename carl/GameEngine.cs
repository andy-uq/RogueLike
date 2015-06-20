using System;
using System.Collections;
using System.Collections.Generic;

namespace carl
{
	public interface IGameEngine
	{
		void EndGame();
      void SetStatus(string format, params object[] args);
		Map Map { get; }
		Player Player { get; }
		IObjectLoader ObjectLoader { get; }
	}

	public interface IObjectLoader
	{
		T Load<T>(string name);
		List<T> LoadAll<T>(string name);

		char[][] LoadTiles(string mapName);
	}

	public class GameEngine : IGameEngine
	{
		public Player Player { get; set; }
		public IObjectLoader ObjectLoader { get; set; }
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