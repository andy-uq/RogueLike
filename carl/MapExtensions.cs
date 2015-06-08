namespace carl
{
	public static class MapExtensions
	{
		public static bool IsOccupied(this Tile[,] map, int x, int y)
		{
			if (x < 0 || y < 0)
				return true;

			var maxX = map.GetLength(1);
			var maxY = map.GetLength(0);
			if (x < maxX && y < maxY)
			{
				var tile = map[y, x];
				return tile == Tiles.Wall || tile == Tiles.ClosedDoor;
			}

			return true;
		}

		public static Tile ToTile(this char c)
		{
			switch (c)
			{
				case '#':
					return Tiles.Wall;
				case '/':
					return Tiles.ClosedDoor;
				default:
					return Tiles.Floor;
			}
		}

	}
}