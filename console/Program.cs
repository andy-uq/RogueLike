using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace carl
{
	public static class Program
	{
		private static RenderLoop _renderLoop;
		private static InputLoop _inputLoop;

		private static void Main(string[] args)
		{
			var game = new GameEngine
			{
				IsActive = true,
				CommandLine = "",
				Player = new Player() {Position = new Point(x: 1, y: 7)},
			};

			game.Map = LoadMap(game);
			game.CommandProcessor = new CommandProcessor(game, game.Player);
			
			_renderLoop = new RenderLoop(game, new RawConsole());
			_inputLoop = new InputLoop(game, new RawConsole());

			var render = Task.Run(() => _renderLoop.RenderLoopAsync());
			var input = Task.Run(() => _inputLoop.InputLoopAsync());

			Task.WaitAll(input, render);
		}

		private static Tile[,] LoadMap(GameEngine game)
		{
			var mapData = File.ReadAllLines("map01.txt").Select(t => t.ToArray()).ToArray();

			var map = new Tile[mapData.Count(), mapData.Max(l => l.Length)];

			int y = 0;
			foreach (var line in mapData)
			{
				for (var x = 0; x < line.Length; x++)
				{
					map[y, x] = mapData[y][x].ToTile();
					if (mapData[y][x] == '@')
					{
						game.Player.Position = new Point(x, y);
					}
				}

				y++;
			}

			return map;
		}
	}
}
