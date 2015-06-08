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

			if (_game.Map.IsOccupied(position))
				return Bump(_game.Map, position);

			_player.Position = position;
			return true;
		}

		private bool Bump(Map map, Point point)
		{
			var tile = map[point];
			if (tile == Tiles.ClosedDoor)
			{
				return map.OpenDoor(point);
			}

			return false;
		}
	}
}