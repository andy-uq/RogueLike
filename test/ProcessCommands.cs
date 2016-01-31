using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using static LanguageExt.Prelude;
using NUnit.Framework;

namespace RogueLike.Tests
{
	public class ProcessCommands
	{
		[TestCase(1, 0, true)]
		[TestCase(-1, 0, false)]
		[TestCase(0, -1, false)]
		[TestCase(1, 1, true)]
		public async Task Move(int x, int y, bool success)
		{
			var origin = new Point(1, 1);
			var player = new Player(position: origin);

			var gameEngine = new GameEngine()
			{
				Map = Data.Maps.Small(),
				Player = player,
			};
			
			var cp = new CommandProcessor(gameEngine);
			var move = await cp.Move(x, y);

			if (success)
			{
				move.Match(
					m => m.Should().BeOfType<MovePlayerAction>(),
					() => { throw new InvalidOperationException(); });
			}
			else
			{
				move.Some(m => m.Should().BeNull());
			}
		}

		[Test]
		public async Task OpenDoor()
		{
			var player = new Player(position: new Point(1, 1));
			var map = Data.Maps.HasDoor();

			var gameEngine = new GameEngine()
			{
				Map = map,
				Player = player,
			};

			var cp = new CommandProcessor(gameEngine);
			var move = await cp.Move(1, 0);

			move.Match(
				m => m.Should().BeOfType<OpenDoorAction>(),
				() => { throw new InvalidOperationException(); });
		}

		[Test]
		public void Quit()
		{
			var gameEngine = new GameEngine();
			var cp = new CommandProcessor(gameEngine);

			cp.Add("quit");

			gameEngine.IsActive.Should().BeFalse();
		}

		[Test]
		public void UnknownCommand()
		{
			var gameEngine = new GameEngine();

			var cp = new CommandProcessor(gameEngine);
			cp.Add("bleh");

			gameEngine.StatusLine.Should().Be("Unknown command: bleh");
		}
	}
}