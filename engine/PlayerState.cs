using System.Collections.Generic;

namespace RogueLike
{
	public class PlayerState
	{
		public PointXY Position { get; set; }
		public List<PlayerItem> Inventory { get; set; }
	}

	public struct PointXY
	{
		public int X { get; set; }
		public int Y { get; set; }
	}
}