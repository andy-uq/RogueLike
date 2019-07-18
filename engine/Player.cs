using System.Collections.Generic;
using System.Linq;

namespace RogueLike
{
	public class Player
	{
		private readonly List<PlayerItem> _inventory;

		public Player(Point? position = null)
		{
			Position = position ?? new Point(-1, -1);
			_inventory = new List<PlayerItem>(CarryingCapacity);
		}

		public Player(PlayerState state)
		{
			Position = new Point(state.Position.X, state.Position.Y);
			_inventory = state.Inventory;
		}

		public Point Position { get; set; }
		public IEnumerable<PlayerItem> Inventory { get { return _inventory; } }
		public static int CarryingCapacity { get { return 10; } }

		public PlayerItem? Pickup(PlayerItem item)
		{
			if (_inventory.Count >= CarryingCapacity)
				return null;

			_inventory.Add(item);
			return item;
		}

		public PlayerItem? DropItem(PlayerItem item)
		{
			return _inventory.Remove(item) 
				? item 
				: null;
		}

		public PlayerState Save()
		{
			return new PlayerState
			{
				Inventory = _inventory,
				Position = new PointXY(Position.X, Position.Y)
			};
		}

		public void Load(PlayerState state)
		{
			Position = new Point(state.Position.X, state.Position.Y);			
		}
	}

	public class PlayerItem
	{
	}

	public class MapItem { }
}