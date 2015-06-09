using System.Collections.Generic;
using System.Linq;

namespace carl
{
	public class Map
	{
		private readonly Tile[,] _tiles;
		private readonly List<Mobile> _mobs;

		public Map(Tile[,] tiles, Point startingPosition, IEnumerable<Mobile> mobs)
		{
			_tiles = tiles;
			_mobs = mobs.ToList();

			Dimensions = new Point(_tiles.GetLength(0), _tiles.GetLength(1));
			StartingPosition = startingPosition;
		}

		public Point Dimensions { get; }
		public Point StartingPosition { get; private set; }

		public Tile this[int x, int y] => _tiles[x, y];
		public Tile this[Point point] => _tiles[point.X, point.Y];

		public IEnumerable<Mobile> Mobiles => _mobs;

		public void InitialiseLevel(Player player)
		{
			player.Position = StartingPosition;
		}

		public bool IsOccupied(Point point)
		{
			if (point.X < 0 || point.Y < 0)
				return true;

			if (point.X < Dimensions.X && point.Y < Dimensions.Y)
			{
				var tile = this[point];
				return tile == Tiles.Wall || tile == Tiles.ClosedDoor;
			}

			return true;
		}

		public bool OpenDoor(Point point)
		{
			_tiles[point.X, point.Y] = Tiles.OpenDoor;
			return true;
		}

		public char GetGlyph(Point point)
		{
			var mobile = GetMobile(point);
			return mobile?.Glyph ?? this[point].Glyph;
		}

		private Mobile GetMobile(Point point)
		{
			return Mobiles.FirstOrDefault(m => m.Position == point);
		}
	}
}