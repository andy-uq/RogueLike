﻿using System;
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
				Player = new Player(position: new Point(x: 1, y: 7)),
			};

			game.Map = MapReader.LoadMap(game, "map01.txt");
			game.CommandProcessor = new CommandProcessor(game, game.Player);
			
			_renderLoop = new RenderLoop(game, new RawConsole());
			_inputLoop = new InputLoop(game, new RawConsole());

			var render = Task.Run(() => _renderLoop.RenderLoopAsync());
			var input = Task.Run(() => _inputLoop.InputLoopAsync());

			Task.WaitAll(input, render);
		}
	}
}