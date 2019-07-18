using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RogueLike
{
	public class CommandProcessor
	{
		private readonly IGameEngine _game;

		private Player Player => _game.Player;

		public CommandProcessor(IGameEngine game)
		{
			_game = game;
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

		public async Task<IPlayerAction?> Move(int xDelta = 0, int yDelta = 0)
		{
			var position = Player.Position.Add(xDelta, yDelta);
			if (_game.Map.IsOccupied(position))
			{
				return await BumpAsync(position);
			}

			var movePlayer = new MovePlayerAction(position);
			return await _game.EnqueueActionAsync(movePlayer);
		}

		private async Task<IPlayerAction?> BumpAsync(Point point)
		{
			var tile = _game.Map[point];
			if (tile == Tiles.ClosedDoor)
			{
				return await _game.EnqueueActionAsync(new OpenDoorAction(point));
			}

			return null;
		}
	}
}