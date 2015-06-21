using System.Collections.Generic;

namespace RogueLike
{
	public interface IObjectLoader
	{
		T Load<T>(string name);
		List<T> LoadAll<T>(string name);

		char[][] LoadTiles(string mapName);
	}
}