﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace carl
{
	public class RenderLoop
	{
		private double _fps;
		private int _frame;

		private static class Position
		{
			public static readonly Point Map = new Point(2, 2);
			public static readonly Point CommandLine = new Point(1, 25);
			public static readonly Point StatusLine = new Point(1, 1);
			public static readonly Point FpsCounter = new Point(77, 1);
		}

		public RenderLoop(GameEngine game, IConsole console)
		{
			Game = game;
			Console = console;
		}

		private IConsole Console { get; }
		private GameEngine Game { get; }

		public async Task RenderLoopAsync()
		{
			var frameTimer = Stopwatch.StartNew();

			while (Game.IsActive)
			{
				RenderFrame(frameTimer);
				frameTimer.Stop();

				var delay = 16.7 - frameTimer.ElapsedMilliseconds;
				if (delay > 0)
					await Task.Delay((int)delay);

				frameTimer.Restart();
			}

			Console.Restore();
		}

		private void RenderFrame(Stopwatch frameTimer)
		{
			RenderMap(Game.Player.Position, Game.Map);

			Console.Write(Position.CommandLine, Game.CommandLine);
			Console.SetCursorPosition(Position.CommandLine.OffsetX(Game.CommandLine.Length));

			if (Game.StatusTtl > 0)
			{
				Game.StatusTtl--;
				Console.Write(Position.StatusLine, Game.StatusLine, ConsoleColor.DarkRed);
			}

			_frame++;
			UpdateFps(frameTimer);

			Console.SwapBuffers();
		}

		private void UpdateFps(Stopwatch frameTimer)
		{
			if (_frame%20 == 0)
			{
				_fps = 1000.0/frameTimer.ElapsedMilliseconds;
			}

			Console.Write(Position.FpsCounter, $"{_fps:n0}", ConsoleColor.Magenta);
		}

		private void RenderMap(Point player, Tile[,] map)
		{
			var left = player.X - 15;
			var top = player.Y - 8;

			var width = map.GetLength(1);
			var height = map.GetLength(0);

			for (var y = 0; y < 16; y++)
			{
				for (var x = 0; x < 30; x++)
				{
					if (left + x < 0 || top + y < 0)
						continue;

					if (left + x >= width || top + y >= height)
						continue;

					Console.Write(
						Position.Map.Add(x, y), 
						map[top + y, left + x].Glyph.ToString(), 
						ConsoleColor.Gray);
				}
			}

			Console.Write(Position.Map.Add(15, 8), "@", ConsoleColor.White);
		}
	}
}