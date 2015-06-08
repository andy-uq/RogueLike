using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace carl
{
	public static class Program
	{
		private static async Task RenderLoopAsync(GameEngine game)
		{
			var console = new RawConsole();

			var sw = Stopwatch.StartNew();
			long frame = 0;
			double fps = 0;
			while (game.IsActive)
			{
				RenderMap(console, 2, 2, game.Player, game.Map);
				
				console.Write(2, 25, game.CommandLine);
				console.SetCursorPosition(2 + game.CommandLine.Length, 25);

				if (game.StatusTtl > 0)
				{
					game.StatusTtl--;
					console.Write(1, 1, game.StatusLine, ConsoleColor.DarkRed);
				}

				var delay = 16.7 - sw.ElapsedMilliseconds;
				if (delay > 0)
					await Task.Delay((int) delay);

				if (frame % 20 == 0)
				{
					fps = 1000.0 / sw.ElapsedMilliseconds;
				}

				console.Write(77, 1, string.Format("{0:n0}", fps), ConsoleColor.Magenta);
				console.SwapBuffers();
				frame++;

				sw.Restart();
			}

			console.Restore();
		}

		private static void Main(string[] args)
		{
			var game = new GameEngine
			{
				IsActive = true,
				CommandLine = "",
				Player = new Player() {X = 1, Y = 7},
			};
			game.Map = LoadMap(game);
			game.CommandProcessor = new CommandProcessor(game, game.Player);
			
			var render = Task.Run(() => RenderLoopAsync(game));
			var input = Task.Run(() => InputLoopAsync(game));

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
						game.Player.X = x;
						game.Player.Y = y;
					}
				}

				y++;
			}

			return map;
		}

		private static void InputLoopAsync(GameEngine game)
		{
			char[] commandBuffer = new char[80];
			int i = 0;
			bool extendedCommand = false;

			while (game.IsActive)
			{
				var key = Console.ReadKey(intercept: true);

				if (!char.IsControl(key.KeyChar))
				{
					if (extendedCommand)
					{
						commandBuffer[i] = key.KeyChar;
						i++;
						game.CommandLine = new string(commandBuffer, 0, i);
					}
				}

				switch (key.KeyChar)
				{
					case '`':
						extendedCommand = true;
						break;
				}

				switch (key.Key)
				{
					case ConsoleKey.Q:
						game.CommandProcessor.Add("quit");
						break;

					case ConsoleKey.Backspace:
						i = i == 0 ? 0 : i - 1;
						continue;

					case ConsoleKey.Enter:
						game.CommandProcessor.Add(game.CommandLine);
						extendedCommand = false;
						game.CommandLine = "";
						i = 0;
						break;

					case ConsoleKey.Escape:
						extendedCommand = false;
						game.CommandLine = "";
						i = 0;
						break;

					case ConsoleKey.UpArrow:
						game.CommandProcessor.Move(yDelta: -1);
						break;

					case ConsoleKey.DownArrow:
						game.CommandProcessor.Move(yDelta: 1);
						break;

					case ConsoleKey.LeftArrow:
						game.CommandProcessor.Move(xDelta: -1);
						break;

					case ConsoleKey.RightArrow:
						game.CommandProcessor.Move(xDelta: +1);
						break;
				}
			}
		}

		private static void RenderMap(RawConsole console, int xOffset, int yOffset, Player player, Tile[,] map)
		{
			int mapX = player.X - 15;
			int mapY = player.Y - 8;

			int width = map.GetLength(1);
			int height = map.GetLength(0);

			for (var y = 0; y < 16; y++)
				for (var x = 0; x < 30; x++)
				{
					if (mapX + x < 0 || mapY + y < 0)
						continue;

					if (mapX + x >= width || mapY + y >= height)
						continue;

					console.Write(x + xOffset, y + yOffset, map[mapY + y, mapX + x].Glyph.ToString(), ConsoleColor.Gray);
				}

			console.Write(15 + xOffset, 8 + yOffset, "@", ConsoleColor.White);
		}
	}
}
