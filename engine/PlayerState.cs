using System.Collections.Generic;

namespace RogueLike
{
	public class PlayerState
	{
		public PointXY Position { get; set; }
		public List<PlayerItem> Inventory { get; set; }

		public PlayerState()
		{
			Inventory = new List<PlayerItem>();
		}
	}

	public class MapState
	{
		public List<TileState> Tiles { get; set; }
		public List<MobileState> Mobiles { get; set; }

		public MapState()
		{
			Tiles = new List<TileState>();
			Mobiles = new List<MobileState>();
		}
	}

	public class MobileState
	{
		public int Id { get; set; }
		public PointXY Position { get; set; }
	}

	public class TileState
	{
		public PointXY Coordinates { get; set; }
	}

	public struct PointXY
	{
		public int X { get; set; }
		public int Y { get; set; }
	}
}