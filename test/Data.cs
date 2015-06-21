using System;
using System.Collections.Generic;
using System.Linq;
using RogueLike.Definitions;

namespace RogueLike.Tests
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

				var tiles = new Tile[data.Max(d => d.Length), height];
				for (var y = 0; y < height; y++)
					for (var x = 0; x < data[y].Length; x++)
						tiles[x, y] = data[y][x].ToTile();

				return new Map(tiles, default(Point), Enumerable.Empty<Mobile>());
			}
		}

		public static List<Monster> Monsters = new List<Monster>() {new Monster()};
		public static Level Level = new Level() {Map = "map", Monsters = "monsters"};
	}
}
