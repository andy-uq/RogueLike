using FluentAssertions;
using NUnit.Framework;

namespace RogueLike.Tests
{
	public class SavePlayer
	{
		[Test]
		public void RoundTrip()
		{
			var player = new Player();

			var state = player.Save();
			state.Inventory.Should().BeEmpty();

			var restored = new Player(state);

			restored.Position.Should().Be(player.Position);
			restored.Inventory.Should().Equal(player.Inventory);
		}
	}
}