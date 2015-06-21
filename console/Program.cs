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

		private static void Main(string[] args)
		{
			var engine = new GameEngine
			{
				IsActive = true,
				CommandLine = "",
				ObjectLoader = new ObjectLoader(),
				SaveGameStore = new SaveGameStore(),
				Player = new Player()
			};

			engine.CommandProcessor = new CommandProcessor(engine);

			var reader = new MapReader(engine);
			engine.Map = reader.LoadLevel("level01.txt");

			var playerState = engine.SaveGameStore.Load();
			playerState.Match(
					state => engine.Player = new Player(state),
					() => engine.Map.InitialiseLevel(engine.Player));

			_renderLoop = new RenderLoop(engine, new RawConsole());
			_inputLoop = new InputLoop(engine, new RawConsole());
			_gameLoop = new GameLoop(engine);

			var render = Task.Run(() => _renderLoop.RenderLoopAsync());
			var game = Task.Run(() => _gameLoop.GameLoopAsync());
			var input = Task.Run(() => _inputLoop.InputLoopAsync());

			var task = Task.WhenAny(input, render, game);
			var awaiter = task.GetAwaiter();
			awaiter.GetResult();
		}
	}
}
