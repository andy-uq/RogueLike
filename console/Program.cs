using System.Threading.Tasks;
using RogueLike.Win32;

namespace RogueLike.Console
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
				ObjectLoader = new ObjectLoader(),
				Player = new Player(position: new Point(x: 1, y: 7)),
			};

			var reader = new MapReader(game);
			game.Map = reader.LoadLevel("level01.txt");
			game.CommandProcessor = new CommandProcessor(game);
			game.Map.InitialiseLevel(game.Player);

			_renderLoop = new RenderLoop(game, new RawConsole());
			_inputLoop = new InputLoop(game, new RawConsole());

			var render = Task.Run(() => _renderLoop.RenderLoopAsync());
			var input = Task.Run(() => _inputLoop.InputLoopAsync());

			Task.WaitAll(input, render);
		}
	}
}
