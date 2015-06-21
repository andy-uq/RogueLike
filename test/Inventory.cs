using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LanguageExt;
using NUnit.Framework;

namespace RogueLike.Tests
{
	public class Inventory
	{
		[Test]
		public void PlayerHasEmptyInventory()
		{
			var player = new Player();
			player.Inventory.Should().BeEmpty();
		}

		[Test]
		public void CanAddItem()
		{
			var item = new PlayerItem();
			var player = new Player();
			var playerItem = player.Pickup(item);

			player.Inventory.Should().NotBeEmpty();
			playerItem.Should().Equal(Prelude.Some(item));
		}

		[Test]
		public void CannotAddItemOverCapacity()
		{
			var item = new PlayerItem();
			var state = new PlayerState() { Inventory = Enumerable.Repeat(item, Player.CarryingCapacity).ToList() };

			var player = new Player(state);
			var playerItem = player.Pickup(item);

			playerItem.IsNone.Should().BeTrue();
		}

		[Test]
		public void CanDropItem()
		{
			var item = new PlayerItem();
			var state = new PlayerState {Inventory = new List<PlayerItem>() {item}};

			var player = new Player(state);
			var droppedItem = player.DropItem(item);

			droppedItem.Should().Equal(Prelude.Some(item));
		}

		[Test]
		public void CannotDropItemDontOwn()
		{
			var item = new PlayerItem();
			
			var player = new Player();
			var droppedItem = player.DropItem(item);

			droppedItem.IsNone.Should().BeTrue();
		}
	}
}