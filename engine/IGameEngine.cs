using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RogueLike
{
	public interface IGameEngine
	{
		void EndGame();
		void SetStatus(string format, params object[] args);
		Map Map { get; }
		Player Player { get; }

		bool IsActive { get; }

		Task<IPlayerAction> EnqueueActionAsync(IPlayerAction action);
		Task<bool> TakeNextActionAsync(GameActionContext context);

		void Load();
		void Save();
	}
}