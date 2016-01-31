using System.Threading.Tasks;
using LanguageExt;
using Nito.AsyncEx;

namespace RogueLike
{
	public interface IGameEngine
	{
		void EndGame();
		void SetStatus(string format, params object[] args);
		Map Map { get; }
		Player Player { get; }
		IObjectLoader ObjectLoader { get; }

		bool IsActive { get; }

		Task<Option<IPlayerAction>> EnqueueActionAsync(IPlayerAction action);
      Task<IPlayerAction> TakeNextActionAsync();

		void Load();
		void Save();
	}
}