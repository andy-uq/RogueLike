using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace RogueLike.Tests
{
	public class Inventory
	{
		[Fact]
		public void PlayerHasEmptyInventory()
		{
			var player = new Player();
			player.Inventory.ShouldBeEmpty();
		}

		[Fact]
		public void CanAddItem()
		{
			var item = new PlayerItem();
			var player = new Player();
			var playerItem = player.Pickup(item);

			player.Inventory.ShouldNotBeEmpty();
			playerItem.ShouldBe(item);
		}

		[Fact]
		public void CannotAddItemOverCapacity()
		{
			var item = new PlayerItem();
			var state = new PlayerState() { Inventory = Enumerable.Repeat(item, Player.CarryingCapacity).ToList() };

			var player = new Player(state);
			var playerItem = player.Pickup(item);

			playerItem.ShouldBeNull();
		}

		[Fact]
		public void CanDropItem()
		{
			var item = new PlayerItem();
			var state = new PlayerState {Inventory = new List<PlayerItem>() {item}};

			var player = new Player(state);
			var droppedItem = player.DropItem(item);

			droppedItem.ShouldBe(item);
		}

		[Fact]
		public void CannotDropItemDontOwn()
		{
			var item = new PlayerItem();
			
			var player = new Player();
			var droppedItem = player.DropItem(item);

			droppedItem.ShouldBeNull();
		}
	}
}