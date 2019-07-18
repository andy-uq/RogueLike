using System;
using System.Threading.Tasks;
using RogueLike.Win32;

namespace RogueLike.Console
{
	public static class Program
	{
		private static RenderLoop _renderLoop;
		private static GameLoop _gameLoop;
		private static InputLoop _inputLoop;

		private static Task Main(string[] args)
		{
			var engine = new GameEngine(new SaveGameStore())
			{
				CommandLine = "",
				Player = new Player()
			};

			var reader = new MapReader(new ObjectLoader());
			engine.Map = reader.LoadLevel("level01.txt");

			engine.SaveGameStore.LoadPlayer(state => engine.Player = new Player(state));
			engine.Map.InitialiseLevel(engine.Player);

			_renderLoop = new RenderLoop(engine, new RawConsole());
			_inputLoop = new InputLoop(engine, new RawConsole());
			_gameLoop = new GameLoop(engine);

			var render = Task.Run(() => _renderLoop.RenderLoopAsync());
			var game = Task.Run(() => _gameLoop.GameLoopAsync());
			var input = Task.Run(() => _inputLoop.InputLoopAsync());

			return Task.WhenAll(input, render, game);
		}
	}
}
