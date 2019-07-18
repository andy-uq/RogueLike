using System.Linq;
using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class Mobiles
	{
		[Fact]
		public void MovesTowardsPlayer()
		{
			var game = new GameEngine(new TestSaveGameStore());
			game.Map = Data.Maps.HasMob();
			game.Player = new Player(new Point(1, 1));

			var mob = game.Map.Mobiles.Single();

			mob.Act(new GameActionContext(game));

			mob.Position.ShouldBe(new Point(2, 1));
		}

		[Fact]
		public void MovesToTarget()
		{
			var game = new GameEngine(new TestSaveGameStore());
			game.Map = Data.Maps.HasMob();

			var mob = game.Map.Mobiles.Single();
			mob.Target = new Point(1, 1);

			mob.Act(new GameActionContext(game));
			mob.Position.ShouldBe(new Point(2, 1));
		}

		[Fact]
		public void WantsToReturnHome()
		{
			var game = new GameEngine(new TestSaveGameStore());
			game.Map = Data.Maps.HasMob();

			var mob = game.Map.Mobiles.Single();
			mob.Home = new Point(3, 1);
			mob.Position = new Point(2, 1);
			mob.Target = new Point(1, 1);

			mob.Act(new GameActionContext(game));
			mob.Position.ShouldBe(new Point(1, 1));
			mob.Target.ShouldBe(mob.Home);
		}

		[Fact]
		public void ReturnsHome()
		{
			var game = new GameEngine(new TestSaveGameStore());
			game.Map = Data.Maps.HasMob();

			var mob = game.Map.Mobiles.Single();
			mob.Home = new Point(3, 1);
			mob.Position = new Point(2, 1);
			mob.Target = mob.Home;

			mob.Act(new GameActionContext(game));

			mob.Position.ShouldBe(mob.Home);
			mob.Target.ShouldBeNull();
		}
	}
}