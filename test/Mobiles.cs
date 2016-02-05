using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace RogueLike.Tests
{
	public class Mobiles
	{
		[Test]
		public void MovesTowardsPlayer()
		{
			var game = new GameEngine();
			game.Map = Data.Maps.HasMob();
			game.Player = new Player(new Point(1, 1));

			var mob = game.Map.Mobiles.Single();

			mob.Act(new GameActionContext(game));

			mob.Position.Should().Be(new Point(2, 1));
		}

		[Test]
		public void MovesToTarget()
		{
			var game = new GameEngine();
			game.Map = Data.Maps.HasMob();

			var mob = game.Map.Mobiles.Single();
			mob.Target = new Point(1, 1);

			mob.Act(new GameActionContext(game));
			mob.Position.Should().Be(new Point(2, 1));
		}

		[Test]
		public void WantsToReturnHome()
		{
			var game = new GameEngine();
			game.Map = Data.Maps.HasMob();

			var mob = game.Map.Mobiles.Single();
			mob.Home = new Point(3, 1);
			mob.Position = new Point(2, 1);
			mob.Target = new Point(1, 1);

			mob.Act(new GameActionContext(game));
			mob.Position.Should().Be(new Point(1, 1));
			mob.Target.Should().Be(mob.Home);
		}

		[Test]
		public void ReturnsHome()
		{
			var game = new GameEngine();
			game.Map = Data.Maps.HasMob();

			var mob = game.Map.Mobiles.Single();
			mob.Home = new Point(3, 1);
			mob.Position = new Point(2, 1);
			mob.Target = mob.Home;

			mob.Act(new GameActionContext(game));

			mob.Position.Should().Be(mob.Home);
			mob.Target.Should().BeNull();
		}
	}
}