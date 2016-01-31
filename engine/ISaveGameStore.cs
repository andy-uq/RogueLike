using LanguageExt;

namespace RogueLike
{
	public interface ISaveGameStore
	{
		Option<PlayerState> LoadPlayer();
		Option<MapState> LoadMap();
		void Save(PlayerState player);
		void Save(MapState map);
	}
}