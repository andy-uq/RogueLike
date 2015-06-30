using System.Collections.Concurrent;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Nito.AsyncEx;

namespace RogueLike
{
	public class GameEngine : IGameEngine
	{
		private readonly AsyncCollection<IPlayerAction> _actionQueue;
		public CommandProcessor CommandProcessor { get; set; }

		public GameEngine()
		{
			_actionQueue = new AsyncCollection<IPlayerAction>(new ConcurrentQueue<IPlayerAction>(), 10);
			CommandLine = string.Empty;
		}

		public string CommandLine { get; set; }
		public string StatusLine { get; set; }
		public int StatusTtl { get; set; }
		public Player Player { get; set; }
		public IObjectLoader ObjectLoader { get; set; }
		public ISaveGameStore SaveGameStore { get; set; }
      public Map Map { get; set; }
		public bool IsActive { get; set; }

		public Task<IPlayerAction> TakeNextActionAsync()
		{
			return _actionQueue.TakeAsync();
		}

		public void Save()
		{
			var player = Player.Save();
			SaveGameStore.Save(player);
		}

		public async Task<Option<IPlayerAction>> EnqueueActionAsync(IPlayerAction action)
		{
			await _actionQueue.AddAsync(action);
			return Some(action);
		}

		public void EndGame()
		{
			IsActive = false;
			_actionQueue.CompleteAdding();
		}

		public void SetStatus(string format, params object[] args)
		{
			StatusLine = string.Format(format, args);
			StatusTtl = 60;
		}

		public Option<string> GetStatusLine()
		{
			if (StatusTtl > 0)
			{
				StatusTtl--;
				return Some(StatusLine);
			}

			return None;
		}
	}

	public interface ISaveGameStore
	{
		Option<PlayerState> Load();
		void Save(PlayerState player);
	}
}