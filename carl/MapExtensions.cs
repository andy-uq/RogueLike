namespace carl
{
	public static class MapExtensions
	{
		public static bool IsOccupied(this Tile[,] map, Point position)
		{
			if (position.X < 0 || position.Y < 0)
				return true;

			var maxX = map.GetLength(1);
			var maxY = map.GetLength(0);
			if (position.X < maxX && position.Y < maxY)
			{
				var tile = map[position.Y, position.X];
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