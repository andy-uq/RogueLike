using System;

namespace RogueLike
{
	public interface ISaveGameStore
	{
		PlayerState? LoadPlayer(Action<PlayerState> action);
		MapState? LoadMap(Action<MapState> action);
		void Save(PlayerState player);
		void Save(MapState map);
	}
}