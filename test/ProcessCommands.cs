using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class ProcessCommands
	{
		[Theory]
		[InlineData(1, 0, true)]
		[InlineData(-1, 0, false)]
		[InlineData(0, -1, false)]
		[InlineData(1, 1, true)]
		public async Task Move(int x, int y, bool success)
		{
			var origin = new Point(1, 1);
			var player = new Player(position: origin);

			var game = new GameEngine(new TestSaveGameStore())
			{
				Map = Data.Maps.Small(),
				Player = player,
			};
			
			var cp = new CommandProcessor(game);
			var move = await cp.Move(x, y);

			if (success)
			{
				move.ShouldBeOfType<MovePlayerAction>();
			}
			else
			{
				move.ShouldBeNull();
			}
		}

		[Fact]
		public async Task OpenDoor()
		{
			var player = new Player(position: new Point(1, 1));
			var map = Data.Maps.HasDoor();

			var game = new GameEngine(new TestSaveGameStore())
			{
				Map = map,
				Player = player,
			};

			var cp = new CommandProcessor(game);
			var move = await cp.Move(1, 0);

			move.ShouldBeOfType<OpenDoorAction>();
		}

		[Fact]
		public void Quit()
		{
			var gameEngine = new GameEngine(new TestSaveGameStore());
			var cp = new CommandProcessor(gameEngine);

			cp.Add("quit");

			gameEngine.IsActive.ShouldBeFalse();
		}

		[Fact]
		public void UnknownCommand()
		{
			var game = new GameEngine(new TestSaveGameStore());

			var cp = new CommandProcessor(game);
			cp.Add("bleh");

			game.StatusLine.ShouldBe("Unknown command: bleh");
		}
	}
}