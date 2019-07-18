using System.Collections.Generic;
using System.Linq;

namespace RogueLike
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
		public Point StartingPosition { get; }

		public Tile this[int x, int y] => _tiles[x, y];

		public Tile this[Point point]
		{
			get => _tiles[point.X, point.Y];
			private set => _tiles[point.X, point.Y] = value;
		}

		private Tile this[PointXY point]
		{
			get => _tiles[point.X, point.Y];
			set => _tiles[point.X, point.Y] = value;
		}

		public IEnumerable<Mobile> Mobiles => _mobs;
		
		public static readonly Map Empty = new Map(new Tile[0,0], Point.Zero, Enumerable.Empty<Mobile>());

		public bool CanSee(Point source, Point target) => new VisibilityCheck(_tiles).CanSee(source, target);

		public void InitialiseLevel(Player player)
		{
			player.Position = StartingPosition;
			foreach (var mob in _mobs)
				mob.Home = mob.Position;
		}

		public bool IsOccupied(Point point)
		{
			if (point.X < 0 || point.Y < 0
			    || point.X >= Dimensions.X || point.Y >= Dimensions.Y)
				return true;

			var tile = this[point];
			return tile == Tiles.Wall || tile == Tiles.ClosedDoor;
		}

		public bool OpenDoor(Point point)
		{
			this[point] = Tiles.OpenDoor;
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

		public MapState Save()
		{
			return new MapState
			{
				Tiles = SaveTiles().ToList(),
				Mobiles = Mobiles.Select(mob => mob.Save()).ToList()
			};
		}

		public void Load(MapState state)
		{
			foreach (var tile in state.Tiles)
			{
				this[tile.Coordinates] = this[tile.Coordinates].Load(tile);
			}

			foreach (var mob in state.Mobiles)
			{
				_mobs[mob.Id].Load(mob);
			}
		}

		private IEnumerable<TileState> SaveTiles()
		{
			for (var y = 0; y < Dimensions.Y; y++)
			{
				for (var x = 0; x < Dimensions.X; x++)
				{
					var state = _tiles[x, y].Save();
					if (state == null)
						continue;

					state.Coordinates = new PointXY(x,y);
					yield return state;
				}
			}
		}

	}
}