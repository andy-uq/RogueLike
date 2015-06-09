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
		public static class Maps
		{
			private readonly static char[,] _small =
			{
				{ '#', '#', '#', '#', },
				{ '#', ' ', ' ', '#', },
				{ '#', ' ', ' ', '#', },
				{ '#', '#', '#', '#', },
			};

			private readonly static char[,] _hasDoor =
{
				{ '#', '#', '#', '#', '#', },
				{ '#', ' ', '/', ' ', '#', },
				{ '#', '#', '#', '#', '#', },
			};

			public readonly static Func<Map> HasDoor = () => ToMap(_hasDoor);
			public readonly static Func<Map> Small = () => ToMap(_small);

			private static Map ToMap(char[,] data)
			{
				var height = data.GetLength(0);
				var width = data.GetLength(1);

				var tiles = new Tile[height, width];
				for (var y = 0; y < height; y++)
					for (var x = 0; x < width; x++)
						tiles[y, x] = data[y, x].ToTile();

				return new Map(tiles, default(Point), Enumerable.Empty<Mobile>());
			}
		}
	}
}
