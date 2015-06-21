namespace RogueLike
{
	public class Tile
	{
		public char Glyph;
	}

	public static class Tiles
	{
		public static readonly Tile Floor = new Tile {Glyph = '.'};
		public static readonly Tile Wall = new Tile {Glyph = '#'};
		public static readonly Tile ClosedDoor = new Tile {Glyph = '/'};
		public static readonly Tile OpenDoor = new Tile {Glyph = '='};
	}

}