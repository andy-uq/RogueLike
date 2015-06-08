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

			public readonly static Func<Tile[,]> HasDoor = () => ToMap(_hasDoor);
			public readonly static Func<Tile[,]> Small = () => ToMap(_small);

			private static Tile[,] ToMap(char[,] data)
			{
				var height = data.GetLength(0);
				var width = data.GetLength(1);

				var map = new Tile[height, width];
				for (var y = 0; y < height; y++)
					for (var x = 0; x < width; x++)
						map[y, x] = data[y, x].ToTile();

				return map;
			}
		}
	}
}
