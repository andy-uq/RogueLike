using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace RogueLike.Tests
{
	public class GameActions
	{
		[Test]
		public void Move()
		{
			var origin = new Point(1, 1);
			var player = new Player(position: origin);

			var gameEngine = new Mock<IGameEngine>(MockBehavior.Strict);
			gameEngine.SetupGet(_ => _.Map).Returns(Data.Maps.Small);
			gameEngine.SetupGet(_ => _.Player).Returns(player);

			var context = new GameActionContext(gameEngine.Object);
			var action = new MovePlayerAction(new Point(2, 1));
			action.Act(context);

			player.Position.X.Should().Be(2);
			player.Position.Y.Should().Be(1);
		}

		[Test]
		public void OpenDoor()
		{
			var player = new Player(position: new Point(1, 1));
			var map = Data.Maps.HasDoor();

			var gameEngine = new Mock<IGameEngine>();
			gameEngine.SetupGet(_ => _.Map).Returns(map);
			gameEngine.SetupGet(_ => _.Player).Returns(player);

			var context = new GameActionContext(gameEngine.Object);
			var action = new OpenDoorAction(new Point(1, 2));
			action.Act(context);

			player.Position.X.Should().Be(1);
			player.Position.Y.Should().Be(1);

			map[1, 2].Should().Be(Tiles.OpenDoor);
		}
	}
}