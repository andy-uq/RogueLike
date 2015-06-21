using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

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

		public Task<Option<IPlayerAction>> Move(int xDelta = 0, int yDelta = 0)
		{
			var position = Player.Position.Add(xDelta, yDelta);
			if (_game.Map.IsOccupied(position))
			{
				return BumpAsync(position);
			}

			var movePlayer = new MovePlayerAction(position);
			return _game.EnqueueActionAsync(movePlayer);		
		}

		private Task<Option<IPlayerAction>> BumpAsync(Point point)
		{
			var tile = _game.Map[point];
			if (tile == Tiles.ClosedDoor)
			{
				return _game.EnqueueActionAsync(new OpenDoorAction(point));
			}

			return Task.FromResult<Option<IPlayerAction>>(None);
		}
	}
}