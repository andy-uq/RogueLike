using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class GameActions
	{
		[Fact]
		public void Move()
		{
			var origin = new Point(1, 1);
			var player = new Player(position: origin);

			var gameEngine = new GameEngine(new TestSaveGameStore())
			{
				Player = player,
				Map = Data.Maps.Small()
			};

			var context = new GameActionContext(gameEngine);
			var action = new MovePlayerAction(new Point(2, 1));
			action.Act(context);

			var (x, y) = player.Position;
			(x, y).ShouldBe((2, 1));
		}

		[Fact]
		public void OpenDoor()
		{
			var player = new Player(position: new Point(1, 1));
			var map = Data.Maps.HasDoor();

			var gameEngine = new GameEngine(new TestSaveGameStore())
			{
				Player = player,
				Map = map
			};

			var context = new GameActionContext(gameEngine);
			var action = new OpenDoorAction(new Point(1, 2));
			action.Act(context);

			var (x, y) = player.Position;
			(x, y).ShouldBe((1, 1));

			map[1, 2].ShouldBe(Tiles.OpenDoor);
		}
	}
}