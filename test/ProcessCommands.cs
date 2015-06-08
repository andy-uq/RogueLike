using System;
using carl;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace test
{
	public class ProcessCommands
	{
		[TestCase(1, 0, true)]
		[TestCase(-1, 0, false)]
		[TestCase(0, -1, false)]
		[TestCase(1, 1, true)]
		public void Move(int x, int y, bool success)
		{
			var gameEngine = new Mock<IGameEngine>();
			gameEngine.SetupGet(_ => _.Map).Returns(Data.Maps.Small);
			var cp = new CommandProcessor(gameEngine.Object, new Player(position: new Point(1, 1)));

			cp.Move(x, y).Should().Be(success);
		}

		[Test]
		public void OpenDoor()
		{
			var gameEngine = new Mock<IGameEngine>();
			gameEngine.SetupGet(_ => _.Map).Returns(Data.Maps.HasDoor());

			var player = new Player(position:new Point(1, 1));
			var cp = new CommandProcessor(gameEngine.Object, player);
			cp.Move(1, 0).Should().BeTrue();

			player.Position.X.Should().Be(1);
			player.Position.Y.Should().Be(1);

			gameEngine.Object.Map[1, 2].Should().Be(Tiles.OpenDoor);
		}

		[Test]
		public void Quit()
		{
			var gameEngine = new Mock<IGameEngine>();
			var cp = new CommandProcessor(gameEngine.Object, new Player(position:new Point(1, 1)));

			cp.Add("quit");

			gameEngine.Verify(x => x.EndGame());
		}

		[Test]
		public void UnknownCommand()
		{
			string status = null;

			var gameEngine = new Mock<IGameEngine>();
			gameEngine.Setup(x => x.SetStatus(Moq.It.IsAny<string>(), It.IsAny<object[]>()))
				.Callback<string, object[]>((format, args) => status = string.Format(format, args));

			var cp = new CommandProcessor(gameEngine.Object, new Player(position: new Point(1, 1)));

			cp.Add("bleh");

			status.Should().Be("Unknown command: bleh");
		}
	}
}