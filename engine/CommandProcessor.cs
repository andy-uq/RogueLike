using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RogueLike
{
	public class CommandProcessor
	{
		private readonly IGameEngine _game;
		private readonly Queue<IPlayerAction> _actions;

		private Player Player => _game.Player;
		public bool HasActions => _actions.Any();

		public CommandProcessor(IGameEngine game)
		{
			_game = game;
			_actions = new Queue<IPlayerAction>();
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

		public Option<IPlayerAction> Move(int xDelta = 0, int yDelta = 0)
		{
			var position = Player.Position.Add(xDelta, yDelta);
			if (_game.Map.IsOccupied(position))
			{
				return Bump(position);
			}

			var movePlayer = new MovePlayerAction(position);
			return Add(movePlayer);
			
		}

		private Option<IPlayerAction> Add(IPlayerAction action)
		{
			_actions.Enqueue(action);
			return Some(action);
		}

		private Option<IPlayerAction> Bump(Point point)
		{
			var tile = _game.Map[point];
			if (tile == Tiles.ClosedDoor)
			{
				return Add(new OpenDoorAction(point));
			}

			return None;
		}

		public void Act()
		{
			var context = new GameActionContext(_game);
			var action = _actions.Dequeue();

			action.Act(context);
		}
	}
}