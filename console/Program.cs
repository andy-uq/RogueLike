using System;
using System.Threading.Tasks;
using RogueLike.Win32;

namespace RogueLike.Console
{
	public static class Program
	{
		private static RenderLoop s_renderLoop;
		private static GameLoop s_gameLoop;
		private static InputLoop s_inputLoop;

		public static Task Main()
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

			s_renderLoop = new RenderLoop(engine, new RawConsole());
			s_inputLoop = new InputLoop(engine, new RawConsole());
			s_gameLoop = new GameLoop(engine);

			var render = Task.Run(() => s_renderLoop.RenderLoopAsync());
			var game = Task.Run(() => s_gameLoop.GameLoopAsync());
			var input = Task.Run(() => s_inputLoop.InputLoopAsync());

			return Task.WhenAll(input, render, game);
		}
	}
}
