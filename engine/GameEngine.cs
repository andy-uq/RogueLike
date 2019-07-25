using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RogueLike
{
	public class GameEngine : IGameEngine
	{
		private readonly ConcurrentQueue<IPlayerAction> _actionQueue;
		private TaskCompletionSource<bool> _actionAvailable;

		public GameEngine(ISaveGameStore saveGameStore)
		{
			_actionQueue = new ConcurrentQueue<IPlayerAction>();
			_actionAvailable = new TaskCompletionSource<bool>();

			SaveGameStore = saveGameStore;
			CommandLine = null;
			StatusLine = null;
			Player = new Player();
			Map = Map.Empty;

			IsActive = true;
		}

		public ISaveGameStore SaveGameStore { get; }

		public string? StatusLine { get; private set; }
		public int StatusTtl { get; private set; }

		public string? CommandLine { get; set; }
		public Player Player { get; set; }
		public Map Map { get; set; }
		
		public bool IsActive { get; private set; }

		public async Task<bool> TakeNextActionAsync(GameActionContext context)
		{
			IPlayerAction action;

			while (!_actionQueue.TryDequeue(out action))
			{
				var finished = await _actionAvailable.Task;
				if (finished)
				{
					return true;
				}
			}

			if (_actionQueue.IsEmpty)
			{
				_actionAvailable = new TaskCompletionSource<bool>(false);
			}

			action.Act(context);
			return false;
		}

		public void Save()
		{
			var player = Player.Save();
			var map = Map.Save();

			SaveGameStore.Save(player);
			SaveGameStore.Save(map);
		}

		public void Load()
		{
			SaveGameStore.LoadPlayer(Player.Load);
			SaveGameStore.LoadMap(Map.Load);
		}

		public Task<IPlayerAction> EnqueueActionAsync(IPlayerAction action)
		{
			_actionQueue.Enqueue(action ?? throw new ArgumentNullException(nameof(action)));
			_actionAvailable.TrySetResult(false);

			return Task.FromResult(action);
		}

		public void EndGame()
		{
			IsActive = false;
			_actionAvailable.TrySetResult(true);
		}

		public void SetStatus(string format, params object[] args)
		{
			StatusLine = string.Format(format, args);
			StatusTtl = 60;
		}

		public string? GetStatusLine()
		{
			if (StatusTtl > 0)
			{
				StatusTtl--;
				return StatusLine;
			}

			return null;
		}
	}
}