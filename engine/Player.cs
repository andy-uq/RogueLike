using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RogueLike
{
	public class Player
	{
		private readonly List<PlayerItem> _inventory;

		public Player(Point position = default(Point))
		{
			Position = position;
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

		public Option<PlayerItem> Pickup(PlayerItem item)
		{
			if (_inventory.Count >= CarryingCapacity)
				return None;

			_inventory.Add(item);
			return Some(item);
		}

		public Option<PlayerItem> DropItem(PlayerItem item)
		{
			return _inventory.Remove(item) 
				? Some(item) 
				: None;
		}

		public PlayerState Save()
		{
			return new PlayerState
			{
				Inventory = _inventory,
				Position = new PointXY {X = Position.X, Y = Position.Y}
			};
		}
	}

	

	public class PlayerItem
	{
	}

	public class Mobile
	{
		public Mobile(Point position)
		{
			Position = position;
		}

		public Point Position { get; set; }
		public char Glyph { get; set; }
	}
}