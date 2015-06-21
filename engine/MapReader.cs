using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RogueLike.Definitions;

namespace RogueLike
{
	public class MapReader
	{
		private readonly IGameEngine _gameEngine;
		private readonly List<Mobile> _mobs;
		private List<Monster> _monsterDefinitions;
		private Point _startingPosition;

		public MapReader(IGameEngine gameEngine)
		{
			_gameEngine = gameEngine;
			_mobs = new List<Mobile>();
		}

		public Map LoadLevel(string name)
		{
			var level = _gameEngine.ObjectLoader.Load<Definitions.Level>(name);

			_monsterDefinitions = _gameEngine.ObjectLoader.LoadAll<Definitions.Monster>(level.Monsters);
			var tileData = _gameEngine.ObjectLoader.LoadTiles(level.Map);

			var tiles = LoadTiles(tileData);
			return new Map(tiles, _startingPosition, _mobs);
		}

		private Tile[,] LoadTiles(char[][] tileData)
		{
			var tiles = new Tile[tileData.Max(l => l.Length), tileData.Length];

			int y = 0, monsterId = 1;
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
			return new Mobile(position) { Glyph = Convert.ToChar(monster.Glyph) };
		}
	}
}