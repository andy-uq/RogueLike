
using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class SavePlayer
	{
		[Fact]
		public void RoundTrip()
		{
			var player = new Player();

			var state = player.Save();
			state.Inventory.ShouldBeEmpty();

			var restored = new Player(state);

			restored.Position.ShouldBe(player.Position);
			restored.Inventory.ShouldBe(player.Inventory);
		}
	}
}