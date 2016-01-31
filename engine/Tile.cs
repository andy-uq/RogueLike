namespace RogueLike
{
	public class Tile
	{
		public char Glyph;

		public virtual TileState Save()
		{
			return null;
		}

		public virtual Tile Load(TileState state)
		{
			return this;
		}
	}

	public static class Tiles
	{
		public static readonly Tile Floor = new Tile {Glyph = '.'};
		public static readonly Tile Wall = new Tile {Glyph = '#'};
		public static readonly Tile ClosedDoor = new DoorTile {Glyph = '/'};
		public static readonly Tile OpenDoor = new DoorTile(isOpen: true) {Glyph = '=' };
	}

	public class DoorTile : Tile
	{
		public DoorTile(bool isOpen = false)
		{
			IsOpen = isOpen;
		}

		public bool IsOpen { get; }

		public override Tile Load(TileState state)
		{
			var door = state as DoorTileState;
			if (door != null && door.IsOpen)
				return Tiles.OpenDoor;
			
			return base.Load(state);
		}

		public override TileState Save()
		{
			if (IsOpen)
				return new DoorTileState { IsOpen = IsOpen };

			return base.Save();
		}
	}

	public class DoorTileState : TileState
	{
		public bool IsOpen { get; set; }
	}
}