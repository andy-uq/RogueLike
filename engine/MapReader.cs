using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RogueLike
{
	public class MapReader
	{
		private readonly List<Mobile> _mobs;
		private List<Definitions.Monster> _monsterDefinitions;
		private Point _startingPosition;
		private readonly IObjectLoader _objectLoader;

		public MapReader(IObjectLoader objectLoader)
		{
			_mobs = new List<Mobile>();
			_monsterDefinitions = new List<Definitions.Monster>();

			_objectLoader = objectLoader;
		}

		public Map LoadLevel(string name)
		{
			var level = _objectLoader.Load<Definitions.Level>(name);

			_monsterDefinitions = _objectLoader.LoadAll<Definitions.Monster>(level.Monsters);
			var tileData = _objectLoader.LoadTiles(level.Map);

			var tiles = LoadTiles(tileData);
			return new Map(tiles, _startingPosition, _mobs);
		}

		private Tile[,] LoadTiles(char[][] tileData)
		{
			var tiles = new Tile[tileData.Max(l => l.Length), tileData.Length];

			int y = 0, monsterId = 0;
			foreach (var line in tileData)
			{
				for (var x = 0; x < line.Length; x++)
				{
					tiles[x, y] = tileData[y][x].ToTile();
					switch (tileData[y][x])
					{
						case '@':
							_startingPosition = new Point(x, y);
							break;

						case 'M':
							var mobile = FromMonster(monsterId++, new Point(x, y));
							_mobs.Add(mobile);
							break;
					}
				}

				y++;
			}

			return tiles;
		}

		private Mobile FromMonster(int monsterId, Point position)
		{
			var monster = _monsterDefinitions.SingleOrDefault(m => m.Id == monsterId) ?? _monsterDefinitions.First();
			return new Mobile(monsterId, position) { Glyph = Convert.ToChar(monster.Glyph) };
		}
	}
}