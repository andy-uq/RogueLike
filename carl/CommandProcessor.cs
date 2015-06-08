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
			var x = _player.X + xDelta;
			var y = _player.Y + yDelta;

			var map = _game.GetCurrentMap();
			if (map.IsOccupied(x, y))
				return Bump(map, x, y);

			_player.Y = y;
			_player.X = x;
			return true;
		}

		private bool Bump(Tile[,] map, int x, int y)
		{
			var tile = map[y, x];
			if (tile == Tiles.ClosedDoor)
			{
				return OpenDoor(map, x, y);
			}

			return false;
		}

		private bool OpenDoor(Tile[,] map, int x, int y)
		{
			map[y, x] = Tiles.OpenDoor;
			return true;
		}
	}
}