namespace carl
{
	public class CommandProcessor
	{
		private readonly IGameEngine _game;
		private readonly Player _player;

		public CommandProcessor(IGameEngine game, Player player)
		{
			_game = game;
			_player = player;
		}

		public void Add(string command)
		{
			if (command == "quit")
			{
				_game.EndGame();
				return;
			}

			_game.SetStatus("Unknown command: {0}", command);
		}

		public bool Move(int xDelta = 0, int yDelta = 0)
		{
			var position = _player.Position.Add(xDelta, yDelta);

			var map = _game.GetCurrentMap();
			if (map.IsOccupied(position))
				return Bump(map, position);

			_player.Position = position;
			return true;
		}

		private bool Bump(Tile[,] map, Point point)
		{
			var tile = map[point.Y, point.X];
			if (tile == Tiles.ClosedDoor)
			{
				return OpenDoor(map, point);
			}

			return false;
		}

		private bool OpenDoor(Tile[,] map, Point point)
		{
			map[point.Y, point.X] = Tiles.OpenDoor;
			return true;
		}
	}
}