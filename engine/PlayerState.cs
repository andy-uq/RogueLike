using System.Collections.Generic;

namespace RogueLike
{
	public class PlayerState
	{
		public Point Position { get; private set; }
		public List<PlayerItem> Inventory { get; set; }
	}
}