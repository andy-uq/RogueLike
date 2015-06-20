using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using carl;

namespace test
{
	public static class Data
	{
		public static class Tiles
		{
			public static readonly char[][] Small =
			{
				new[] {'#', '#', '#', '#',},
				new[] {'#', ' ', ' ', '#',},
				new[] {'#', ' ', ' ', '#',},
				new[] {'#', '#', '#', '#',},
			};

			public static readonly char[][] HasDoor =
			{
				new[] {'#', '#', '#', '#', '#',},
				new[] {'#', ' ', '/', ' ', '#',},
				new[] {'#', '#', '#', '#', '#',},
			};
		}

		public static class Maps
		{
			public readonly static Func<Map> HasDoor = () => ToMap(Tiles.HasDoor);
			public readonly static Func<Map> Small = () => ToMap(Tiles.Small);

			private static Map ToMap(char[][] data)
			{
				var height = data.Length;

				var tiles = new Tile[height, data.Max(d => d.Length)];
				for (var y = 0; y < height; y++)
					for (var x = 0; x < data[y].Length; x++)
						tiles[y, x] = data[y][x].ToTile();

				return new Map(tiles, default(Point), Enumerable.Empty<Mobile>());
			}
		}
	}
}
