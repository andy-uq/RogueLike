using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RogueLike
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
				RenderFrame();
				_frame++;

				var delay = (1_000.0 / 60.0) - frameTimer.ElapsedMilliseconds;
				if (delay > 0)
					await Task.Delay((int)delay);

				UpdateFps(frameTimer);
				Console.SwapBuffers();

				frameTimer.Restart();
			}

			Console.Restore();
		}

		private void RenderFrame()
		{
			RenderMap(Game.Player.Position, Game.Map);

			if (Game.CommandLine != null)
			{
				Console.Write(Position.CommandLine, Game.CommandLine);
				Console.SetCursorPosition(Position.CommandLine.OffsetX(Game.CommandLine.Length));
			}
			else
			{
				Console.ClearCursorPosition();
			}

			var status = Game.GetStatusLine();
			if (status != null)
			{
				Console.Write(Position.StatusLine, status, ConsoleColor.DarkRed);
			}
		}

		private void UpdateFps(Stopwatch frameTimer)
		{
			if (_frame%20 == 0)
			{
				_fps = 1000.0/frameTimer.ElapsedMilliseconds;
			}

			Console.Write(Position.FpsCounter, $"{_fps:n0}", ConsoleColor.Magenta);
		}

		private void RenderMap(Point player, Map map)
		{
			var origin = player.Add(- 15, - 8);

			for (var y = 0; y < 16; y++)
			{
				for (var x = 0; x < 30; x++)
				{
					if (origin.X + x < 0 || origin.Y + y < 0)
						continue;

					if (origin.X + x >= map.Dimensions.X || origin.Y + y >= map.Dimensions.Y)
						continue;

					var mapPoint = origin.Add(x, y);
					var glyph = map.GetGlyph(mapPoint);

					Console.Write(
						Position.Map.Add(x, y), 
						glyph.ToString(), 
						ConsoleColor.Gray);
				}
			}

			Console.Write(Position.Map.Add(15, 8), "@", ConsoleColor.White);
		}
	}
}