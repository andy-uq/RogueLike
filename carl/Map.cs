using System.IO;
using System.Linq;

namespace carl
{
	public class Map
	{
		private readonly Tile[,] _tiles;

		public Map(Tile[,] tiles)
		{
			_tiles = tiles;
			Dimensions = new Point(_tiles.GetLength(0), _tiles.GetLength(1));
		}

		public Tile this[int x, int y] => _tiles[y, x];
		public Tile this[Point point] => _tiles[point.Y, point.X];

		public Point Dimensions { get; }

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
			_tiles[point.Y, point.X] = Tiles.OpenDoor;
			return true;
		}
	}

	public class MapReader
	{
		public static Map LoadMap(GameEngine game, string mapFileName)
		{
			var mapData = File.ReadAllLines(mapFileName).Select(t => t.ToArray()).ToArray();
			var map = new Tile[mapData.Count(), mapData.Max(l => l.Length)];

			int y = 0;
			foreach (var line in mapData)
			{
				for (var x = 0; x < line.Length; x++)
				{
					map[y, x] = mapData[y][x].ToTile();
					if (mapData[y][x] == '@')
					{
						game.Player.Position = new Point(x, y);
					}
				}

				y++;
			}

			return new Map(map);
		}
	}
}